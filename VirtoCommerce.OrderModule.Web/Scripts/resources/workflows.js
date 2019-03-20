angular.module('virtoCommerce.orderModule')
    .factory('virtoCommerce.orderModule.workflows', ['$resource', function ($resource) {
        return $resource('api/workflows/:id', {},
            {
                search: { method: 'POST', url: 'api/workflows/search' },
                updateWorkflow: { method: 'POST', url: 'api/workflows/' },
                getStatuses: { method: 'GET', url: 'api/workflows/statuses/:id'},
                getWorkflowByOrderId: { method: 'GET', url: 'api/orderworkflows/:id' },
                getOrganizationsByMemberId: { method: 'GET', url: 'api/members/:id/organizations', isArray : true}
        });
    }]);
