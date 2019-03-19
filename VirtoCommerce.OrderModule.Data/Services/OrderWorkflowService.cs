using System.Linq;
using VirtoCommerce.OrderModule.Core.Models;
using VirtoCommerce.OrderModule.Core.Services;
using VirtoCommerce.OrderModule.Data.Model;
using VirtoCommerce.OrderModule.Data.Repositories;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.OrderModule.Data.Services
{
    public class OrderWorkflowService : ServiceBase, IOrderWorkflowService
    {
        private IImportWorkflowService _importWorkflowService;
        private IOrderWorkflowRepository _repositoryFactory;

        public OrderWorkflowService(IImportWorkflowService importWorkflowService,
            IOrderWorkflowRepository repositoryFactory)
        {
            _importWorkflowService = importWorkflowService;
            _repositoryFactory = repositoryFactory;
        }

        public OrderWorkflowModel AddOrderWorkflow(OrderWorkflowModel orderWorkflowModel)
        {
            var orderWorkflow = new OrderWorkflowEntity
            {
                OrderId = orderWorkflowModel.OrderId,
                WorkflowId = orderWorkflowModel.WorkflowId
            };
            using (var changeTracker = GetChangeTracker(_repositoryFactory))
            {
                changeTracker.Attach(orderWorkflow);
                _repositoryFactory.Add(orderWorkflow);
                CommitChanges(_repositoryFactory);
            }
            return orderWorkflow.ToModel();
        }

        public string[] GetWorkflowStatus(string orderId)
        {
            string[] result = null;
            _repositoryFactory.DisableChangesTracking();
            var orderWorkflow = _repositoryFactory.GetOrderWorkflow(orderId).Result;
            if (orderWorkflow != null)
            {
                var workflow = _importWorkflowService.GetDetail(orderWorkflow.WorkflowId);
                result = workflow.WorkflowStates.Select(x => x.Status).ToArray<string>();
            }
            return result;
        }

        public bool HasCustomWorkflow(string orderId)
        {
            _repositoryFactory.DisableChangesTracking();
            return _repositoryFactory.GetOrderWorkflow(orderId) != null;
        }
        public OrderWorkflowModel GetOrderWorkflow(string orderId)
        {
            _repositoryFactory.DisableChangesTracking();           
            return _repositoryFactory.GetOrderWorkflow(orderId).Result?.ToModel();
        }

    }
}
