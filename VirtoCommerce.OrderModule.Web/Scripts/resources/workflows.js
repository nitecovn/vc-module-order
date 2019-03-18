angular.module('virtoCommerce.orderModule')
    .factory('virtoCommerce.orderModule.workflows', ['$resource', function ($resource) {
    return $resource('api/organizationWorkflow/:id', {},
        {
            search: { method: 'POST', url: 'api/organizationWorkflow/search' },
            updateStatus: { method: 'POST' },
            updateFile: { method: 'POST', url: 'api/organizationWorkflow/import' }
        });
}]);
