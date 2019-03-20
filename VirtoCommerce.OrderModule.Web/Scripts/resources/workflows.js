angular.module('virtoCommerce.orderModule')
    .factory('virtoCommerce.orderModule.workflows', ['$resource', function ($resource) {
        return $resource('api/workflows/:id', {},
        {
            search: { method: 'POST', url: 'api/workflows/search' },
            updateWorkflow: { method: 'POST', url: 'api/workflows/' },
            getStatuses: { method: 'POST', url: 'api/workflows/statuses/:id' }
        });
    }]);
