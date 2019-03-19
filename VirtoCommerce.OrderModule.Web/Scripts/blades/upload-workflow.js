angular.module('virtoCommerce.orderModule')
    .controller('virtoCommerce.orderModule.uploadFlowController',
    ['$scope', 'platformWebApp.bladeNavigationService', 'virtoCommerce.orderModule.workflows', 'FileUploader',
        function ($scope, bladeNavigationService, workflows, FileUploader) {
            var blade = $scope.blade;
            var orgBlade = blade.parentBlade;
            var _item, _file = {};

            blade.isLoading = false;
            blade.enabledWorkFlow = false;
            $scope.isUploadSuccess = false;
            $scope.hasWorkflow = false;
            $scope.hasFileChanged = false;
            $scope.hasStatusChanged = false;
            
            resetWorkflowData();
            // Initialize Json Uploader
            if (!$scope.uploader) {
                // create the uploader
                var uploader = $scope.uploader = new FileUploader({
                    scope: $scope,
                    headers: { Accept: 'application/json' },
                    url: 'api/platform/assets?folderUrl=OrganizationWorkflow/' + orgBlade.currentEntity.id + '/' + new Date().getTime(),
                    method: 'POST',
                    autoUpload: false,
                    removeAfterUpload: true
                });

                uploader.onBeforeUploadItem = function (fileItem) {
                    blade.isLoading = true;
                };

                uploader.onSuccessItem = function (fileItem, asset, status, headers) {
                    _item = null;
                    _file = {
                        jsonPath: asset[0].relativeUrl,
                        workflowName: asset[0].name
                    };
                    $scope.isUploadSuccess = true;
                    $scope.hasFileChanged = true;
                    $scope.hasWorkflow = true;
                    blade.isLoading = false;
                };

                uploader.onAfterAddingFile = function (item) {
                    _item = item;
                    $scope.jsonPath = item.file.name;
                    bladeNavigationService.setError(null, blade);
                    $scope.isUploadSuccess = false;
                };

                uploader.onErrorItem = function (item, response, status, headers) {
                    _file = {};
                    resetWorkflowData();
                    $scope.isUploadSuccess = false;
                    $scope.hasFileChanged = false;
                    blade.isLoading = false;
                    bladeNavigationService.setError(item._file.name + ' failed: ' + (response.message ? response.message : status), blade);
                };

                $scope.uploadWorkflow = function () {
                    if (_item) uploader.uploadItem(_item);
                };
            }

            $scope.onStatusChanged = function () {
                if (blade.enabledWorkFlow !== orgBlade.workflow.status) $scope.hasStatusChanged = true;
                else $scope.hasStatusChanged = false;
            };

            $scope.blade.toolbarCommands = [
                {
                    name: 'platform.commands.save',
                    icon: 'fa fa-save',
                    executeMethod: function () {
                        blade.isLoading = true;
                        var workflow = angular.extend({
                            organizationId: orgBlade.currentEntity.id
                        }, _file, { status: blade.enabledWorkFlow });
                        // Save file information
                        workflows.updateWorkflow(workflow)
                            .$promise.then(function (workflow) {
                                workflow = workflow.data;
                                $scope.jsonPath = '';
                                $scope.workflowName = workflow.workflowName;
                                $scope.modifiedDate = workflow.modifiedDate;
                                blade.enabledWorkFlow = workflow.status;
                            }, function (response) {
                                bladeNavigationService.setError(response, blade);
                            }).finally(function () {
                                blade.isLoading = false;
                            });
                    },
                    canExecuteMethod: function () {
                        return $scope.hasStatusChanged || $scope.hasFileChanged;
                    },
                    permission: 'workflow:upload'
                }
            ];

            function resetWorkflowData() {
                if (typeof orgBlade.workflow !== 'undefined' && typeof orgBlade.workflow.workflowName !== 'undefined') {
                    $scope.hasWorkflow = true;
                    $scope.workflowName = orgBlade.workflow.workflowName;
                    $scope.modifiedDate = orgBlade.workflow.modifiedDate;
                    blade.enabledWorkFlow = orgBlade.workflow.status;
                }
            }
}]);
