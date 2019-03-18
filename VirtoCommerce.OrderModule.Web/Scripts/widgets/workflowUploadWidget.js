angular.module('virtoCommerce.orderModule')
    .controller('virtoCommerce.orderModule.workflowWidgetController',
        ['$scope', 'platformWebApp.bladeNavigationService', 'virtoCommerce.orderModule.workflows',
            function ($scope, bladeNavigationService, workflows) {
                blade = $scope.blade;
                $scope.workflow = "0";

                workflows.get({ id: blade.currentEntity.id })
                    .$promise
                    .then(function (workflow) {
                        workflow = workflow.data;
                        $scope.workflow = typeof workflow !== 'undefined' && typeof workflow.workflowName !== 'undefined' && workflow.workflowName.length ? "1" : "0";
                        blade.workflow = workflow;
                    }, function(response) {
                        bladeNavigationService.setError(response, blade);
                    });

                $scope.openWorkflowBlade = function () {
                    var newBlade = {
                        id: 'workflowBlade',
                        title: 'orders.blades.organization-workflow.title',
                        controller: 'virtoCommerce.orderModule.uploadFlowController',
                        template: 'Modules/$(VirtoCommerce.Orders)/Scripts/blades/upload-workflow.tpl.html',
                        isClosingDisabled: true
                    };
                    bladeNavigationService.showBlade(newBlade, $scope.blade);
                };
}]);