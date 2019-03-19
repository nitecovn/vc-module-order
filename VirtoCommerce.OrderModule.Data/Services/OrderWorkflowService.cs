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
        private IOrderWorkflowRepository _repositoryFactory;

        public OrderWorkflowService(IOrderWorkflowRepository repositoryFactory)
        {
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
               
        public bool HasCustomWorkflow(string orderId)
        {
            _repositoryFactory.DisableChangesTracking();
            return _repositoryFactory.OrderWorkflows.Any(x => x.OrderId == orderId);
        }

        public OrderWorkflowModel GetOrderWorkflow(string orderId)
        {
            _repositoryFactory.DisableChangesTracking();           
            return _repositoryFactory.GetOrderWorkflow(orderId).Result?.ToModel();
        }

    }
}
