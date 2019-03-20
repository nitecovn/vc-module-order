using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CacheManager.Core;
using Newtonsoft.Json;
using VirtoCommerce.Domain.Commerce.Model.Search;
using VirtoCommerce.OrderModule.Core.Models;
using VirtoCommerce.OrderModule.Core.Services;
using VirtoCommerce.OrderModule.Data.Model;
using VirtoCommerce.OrderModule.Data.Repositories;
using VirtoCommerce.Platform.Core.Assets;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Common;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.OrderModule.Data.Services
{
    public class WorkflowService : ServiceBase, IWorkflowService
    {
        private readonly IBlobStorageProvider _blobStorageProvider;
        private readonly ICacheManager<object> _cacheManager;
        private readonly IOrderWorkflowRepository _repositoryFactory;

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

        public OrderWorkflow GetByOrganizationId(string organizationId)
        {
            _repositoryFactory.DisableChangesTracking();
            var retValue = _repositoryFactory.OrderWorkflows
                .FirstOrDefault(x => x.OrganizationId == organizationId)?
                .ToModel();

            if (retValue != null)
            {
                LoadWorkflowStates(retValue);
            }
            return retValue;
        }

        public GenericSearchResult<OrderWorkflow> Search(WorkflowSearchCriteria searchWorkflowCriteria)
        {
            var result = new GenericSearchResult<OrderWorkflow>();
            if (searchWorkflowCriteria == null) return result;
            var expandPredicate = PredicateBuilder.True<OrderWorkflowEntity>();
            if (!string.IsNullOrEmpty(searchWorkflowCriteria.OrganizationId))
            {
                expandPredicate = expandPredicate.And(x => x.OrganizationId == searchWorkflowCriteria.OrganizationId);
            }
            if (!string.IsNullOrEmpty(searchWorkflowCriteria.WorkflowName))
            {
                expandPredicate = expandPredicate.And(x => x.WorkflowName == searchWorkflowCriteria.WorkflowName);
            }
            if (searchWorkflowCriteria.Status.HasValue)
            {
                expandPredicate = expandPredicate.And(x => x.Status == searchWorkflowCriteria.Status);
            }

            _repositoryFactory.DisableChangesTracking();
            var workflows = _repositoryFactory
                .OrderWorkflows
                .Where(expandPredicate)
                .Select(x => x.ToModel())
                .ToList();

            result.Results = workflows;
            return result;
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

                throw new KeyNotFoundException(nameof(organizationId));
            }
        }

        private void LoadWorkflowStates(OrderWorkflow workflow)
        {
            if (workflow == null) throw new ArgumentNullException(nameof(workflow));

            var cacheKey = $"Order_WorkflowStates_{workflow.Id}";
            var workflowStates = _cacheManager.Get(cacheKey, CacheRegion, () =>
            {
                string jsonValue;
                using (var stream = _blobStorageProvider.OpenRead(workflow.JsonPath))
                {
                    var reader = new StreamReader(stream);
                    jsonValue = reader.ReadToEnd();
                }
                return JsonConvert.DeserializeObject<WorkflowStates>(jsonValue);
            });
            workflow.WorkflowStates = workflowStates;
        }
    }
}
