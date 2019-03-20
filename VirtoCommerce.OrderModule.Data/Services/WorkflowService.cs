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
        private IOrderRepository _repositoryFactory;

        private const string CacheRegion = "WorkflowRegion";

        public WorkflowService(
            IBlobStorageProvider blobStorageProvider,
            ICacheManager<object> cacheManager,
            IOrderRepository repositoryFactory)
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
        public Workflow ImportOrUpdateWorkflow(Workflow workflowModel)
        {
            return string.IsNullOrEmpty(workflowModel.JsonPath) ?
                UpdateStatus(workflowModel.Status, workflowModel.OrganizationId) :
                ImportWorkflow(workflowModel);
        }

        private Workflow ImportWorkflow(Workflow workflowModel)
        {
            var workflow = AbstractTypeFactory<WorkflowEntity>.TryCreateInstance();
            workflow.FromModel(workflowModel);

            using (var changeTracker = GetChangeTracker(_repositoryFactory))
            {
                changeTracker.Attach(workflow);
                _repositoryFactory.Add(workflow);
                CommitChanges(_repositoryFactory);
            }
            return workflow.ToModel();
        }

        private Workflow UpdateStatus(bool status, string organizationId)
        {
            using (var changeTracker = GetChangeTracker(_repositoryFactory))
            {
                var workflow = _repositoryFactory.Workflows.FirstOrDefault(x => x.OrganizationId == organizationId);

                if (workflow != null)
                {
                    changeTracker.Attach(workflow);
                    workflow.Status = status;
                    CommitChanges(_repositoryFactory);
                    return workflow.ToModel();
                }

                throw new KeyNotFoundException(organizationId);
            }
        }
    }
}
