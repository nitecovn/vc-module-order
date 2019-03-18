angular.module('virtoCommerce.orderModule')
    .controller('virtoCommerce.orderModule.uploadFlowController',
    ['$scope', 'platformWebApp.bladeNavigationService', 'virtoCommerce.orderModule.workflows', 'FileUploader',
        function ($scope, bladeNavigationService, workflows, FileUploader) {
            var blade = $scope.blade;
            var orgBlade = blade.parentBlade;
            var _enabledWorkFlow = false;
            var _item, _file;

            blade.isLoading = false;
            blade.enabledWorkFlow = false;
            $scope.workflowName = null;
            $scope.hasChanged = false;
            $scope.hasWorkflow = typeof orgBlade.workflow !== 'undefined' && typeof orgBlade.workflow.workflowStates !== 'undefined' && orgBlade.workflow.workflowStates.length;

            // Get Organization Workflow Information
            // workflows.search({ organizationId: orgBlade.currentEntityId })
            //     .$promise
            //     .then(function (data) {
            //         var _result = data.result;
            //         if (_result && _result.length) {
            //             $scope.hasWorkflow = true;
            //             $scope.workflowName = _result[0].workflowName;
            //             blade.enabledWorkFlow = _result[0].status;
            //             _enabledWorkFlow = _result[0].status;
            //         }
            //     }).finally(function () {
            //         blade.isLoading = false;
            //     });

            // Initialize Json Uploader
            if (!$scope.uploader) {
                // create the uploader
                var uploader = $scope.uploader = new FileUploader({
                    scope: $scope,
                    headers: { Accept: 'application/json' },
                    url: 'api/platform/assets?folderUrl=OrganizationWorkflow/' + orgBlade.currentEntity.id,
                    method: 'POST',
                    autoUpload: false,
                    removeAfterUpload: true
                });

                uploader.onBeforeUploadItem = function (fileItem) {
                    blade.isLoading = true;
                };

                uploader.onSuccessItem = function (fileItem, asset, status, headers) {
                    $scope.hasChanged = true;
                    _file = {
                        "organizationid": orgBlade.currentEntity.id,
                        "fileurl": asset[0].relativeUrl,
                        "filename": asset[0].name
                    };
                };

                uploader.onAfterAddingFile = function (item) {
                    _item = item;
                    bladeNavigationService.setError(null, blade);
                };

                uploader.onErrorItem = function (item, response, status, headers) {
                    bladeNavigationService.setError(item._file.name + ' failed: ' + (response.message ? response.message : status), blade);
                };

                $scope.uploadWorkflow = function () {
                    uploader.uploadItem(_item);
                };
            }

            $scope.blade.toolbarCommands = [
                {
                    name: 'platform.commands.save',
                    icon: 'fa fa-save',
                    executeMethod: function () {
                        blade.isLoading = true;
                        // Save file information
                        workflows.updateFile(_file).then(function () {
                            $scope.fileUrl = _file.fileurl;
                            $scope.fileName = _file.filename;
                            $scope.hasWorkflow = true;
                            blade.enabledWorkFlow = data.result.status;
                            _enabledWorkFlow = data.result.status;

                            console.log('Saved : ' + _enabledWorkFlow + "  " + blade.enabledWorkFlow);

                            workflows.updateStatus({id : orgBlade.currentEntity.id, status : blade.enabledWorkFlow })
                            .then(function (data) {
                                console.log(data);
                            }).finally(function () {
                                blade.isLoading = false;
                            });

                        }, function (response) {
                            bladeNavigationService.setError(response, blade);
                        }).finally(function () {
                            blade.isLoading = false;
                        });
                    },
                    canExecuteMethod: function () {
                        return $scope.hasChanged;
                    }
                }
            ];
}]);
