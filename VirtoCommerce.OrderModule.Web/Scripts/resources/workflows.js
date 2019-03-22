angular.module('virtoCommerce.orderModule')
    .factory('virtoCommerce.orderModule.workflows', ['$resource', function ($resource) {
        return $resource('api/workflows/:id', {},
        {
            updateWorkflow: { method: 'POST', url: 'api/workflows/' },
            getWorkflowByOrderId: { method: 'GET', url: 'api/orderworkflows/:id' },
            getOrganizationsByMemberId: { method: 'GET', url: 'api/members/:id/organizations', isArray: true }
        });
}]);
