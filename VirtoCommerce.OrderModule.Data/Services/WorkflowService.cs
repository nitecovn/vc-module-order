using System.Collections.Generic;
using System.Linq;
using CacheManager.Core;
using VirtoCommerce.OrderModule.Core.Models;
using VirtoCommerce.OrderModule.Core.Services;
using VirtoCommerce.OrderModule.Data.Model;
using VirtoCommerce.OrderModule.Data.Repositories;
using VirtoCommerce.Platform.Core.Assets;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.OrderModule.Data.Services
{
    public class WorkflowService : ServiceBase, IWorkflowService
    {
        private readonly IBlobStorageProvider _blobStorageProvider;
        private readonly ICacheManager<object> _cacheManager;
        private IOrderWorkflowRepository _repositoryFactory;

        private const string CacheRegion = "WorkflowRegion";

        public WorkflowService(
            IBlobStorageProvider blobStorageProvider,
            ICacheManager<object> cacheManager,
            IOrderWorkflowRepository repositoryFactory)
        {
            _blobStorageProvider = blobStorageProvider;
            _cacheManager = cacheManager;
            _repositoryFactory = repositoryFactory;
        }

        /// <summary>
        /// in case if jsonPath is empty, will call ChangeWorkflowStatus, else call ImportWorkflow method
        /// </summary>
        /// <param name="workflowModel"></param>
        /// <returns></returns>
        public OrderWorkflow ImportOrUpdateWorkflow(OrderWorkflow workflowModel)
        {
            return string.IsNullOrEmpty(workflowModel.JsonPath) ?
                UpdateStatus(workflowModel.Status, workflowModel.OrganizationId) :
                ImportWorkflow(workflowModel);
        }

        private OrderWorkflow ImportWorkflow(OrderWorkflow workflowModel)
        {
            //[TODO] Validate workflow here

            var workflow = AbstractTypeFactory<OrderWorkflowEntity>.TryCreateInstance();
            workflow.FromModel(workflowModel);

            using (var changeTracker = GetChangeTracker(_repositoryFactory))
            {
                changeTracker.Attach(workflow);
                _repositoryFactory.Add(workflow);
                CommitChanges(_repositoryFactory);
            }
            return workflow.ToModel();
        }

        private OrderWorkflow UpdateStatus(bool status, string organizationId)
        {
            using (var changeTracker = GetChangeTracker(_repositoryFactory))
            {
                var orderWorkflow = _repositoryFactory.OrderWorkflows.FirstOrDefault(x => x.OrganizationId == organizationId);

                if (orderWorkflow != null)
                {
                    changeTracker.Attach(orderWorkflow);
                    orderWorkflow.Status = status;
                    CommitChanges(_repositoryFactory);
                    return orderWorkflow.ToModel();
                }

                throw new KeyNotFoundException(organizationId);
            }
        }
    }
}
