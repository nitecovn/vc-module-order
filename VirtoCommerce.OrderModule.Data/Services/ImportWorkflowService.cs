using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using CacheManager.Core;
using Newtonsoft.Json;
using VirtoCommerce.OrderModule.Core.Models;
using VirtoCommerce.OrderModule.Core.Services;
using VirtoCommerce.OrderModule.Data.Model;
using VirtoCommerce.OrderModule.Data.Repositories;
using VirtoCommerce.Platform.Core.Assets;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Common;
using VirtoCommerce.Platform.Data.Infrastructure;
using LinqKit;

namespace VirtoCommerce.OrderModule.Data.Services
{
    public class ImportWorkflowService : ServiceBase, IImportWorkflowService
    {
        private readonly IBlobStorageProvider _blobStorageProvider;
        private readonly ICacheManager<object> _cacheManager;
        private IOrganizationWorkflowRepository _repositoryFactory;

        public ImportWorkflowService(IBlobStorageProvider blobStorageProvider,
            ICacheManager<object> cacheManager,
            IOrganizationWorkflowRepository repositoryFactory)
        {
            _blobStorageProvider = blobStorageProvider;
            _cacheManager = cacheManager;
            _repositoryFactory = repositoryFactory;
        }

        public OrganizationWorkflowModel ImportOrUpdateWorkflow(OrganizationWorkflowModel workflowModel)
        {
            if (string.IsNullOrEmpty(workflowModel.JsonPath))
            {
                return ChangeWorkflowStatus(workflowModel.Status, workflowModel.OrganizationId);
            }
            return ImportWorkflow(workflowModel);
        }

        public OrganizationWorkflowModel ImportWorkflow(OrganizationWorkflowModel workflowModel)
        {
            var workflow = new OrganizationWorkflowEntity
            {
                OrganizationId = workflowModel.OrganizationId,
                WorkflowName = workflowModel.WorkflowName,
                JsonPath = workflowModel.JsonPath,
                Status = workflowModel.Status
            };


            using (var changeTracker = GetChangeTracker(_repositoryFactory))
            {
                var workflows = _repositoryFactory.GetByOrganizationIdAsync(workflowModel.OrganizationId);
                changeTracker.Attach(workflow);
                _repositoryFactory.Add(workflow);
                CommitChanges(_repositoryFactory);
            }
            return workflow.ToModel();
        }

        public WorkflowModel GetWorkFlowDetailByOrganizationId(string organizationId)
        {

            _repositoryFactory.DisableChangesTracking();
            var workflows = _repositoryFactory.GetByOrganizationIdAsync(organizationId);

            if (workflows.Result.Any())
            {
                var workflow = workflows.Result[0];
                return _cacheManager.Get("Workflow", "WorkflowRegion", () =>
                {
                    string jsonValue;
                    using (var stream = _blobStorageProvider.OpenRead(workflow.JsonPath))
                    {
                        var reader = new StreamReader(stream);
                        jsonValue = reader.ReadToEnd();
                    }
                    var workFlow = JsonConvert.DeserializeObject<WorkflowModel>(jsonValue);
                    return workFlow;
                });
            }
            return null;

        }
        public OrganizationWorkflowModel ChangeWorkflowStatus(bool status, string organizationId)
        {
            //Update status for organization workflow
            var workflowSettingId = string.Empty;

            using (var changeTracker = GetChangeTracker(_repositoryFactory))
            {
                var workflows = _repositoryFactory.GetByOrganizationIdAsync(organizationId);

                if (workflows.Result.Any())
                {
                    var updateWorkflow = workflows.Result[0];
                    changeTracker.Attach(updateWorkflow);
                    updateWorkflow.Status = status;
                    CommitChanges(_repositoryFactory);
                    return updateWorkflow.ToModel();
                }
                return null;
            }

        }
        public List<OrganizationWorkflowModel> Search(SearchCriteriaModel searchWorkflowCriteria)
        {
            if (searchWorkflowCriteria == null) return null;

            var expandPredicate = LinqKit.PredicateBuilder.New<OrganizationWorkflowEntity>();
            if (!string.IsNullOrEmpty(searchWorkflowCriteria.OrganizationId))
            {
                expandPredicate = expandPredicate.And(x => x.OrganizationId == searchWorkflowCriteria.OrganizationId);
            }
            if (!string.IsNullOrEmpty(searchWorkflowCriteria.WorkflowName))
            {
                expandPredicate = expandPredicate.And(x => x.WorkflowName == searchWorkflowCriteria.WorkflowName);
            }
            if (searchWorkflowCriteria.Active != null)
            {
                expandPredicate = expandPredicate.And(x => x.Status == searchWorkflowCriteria.Active);
            }
            var predicate = (Expression<Func<OrganizationWorkflowEntity, bool>>)Extensions.Expand(expandPredicate);


            _repositoryFactory.DisableChangesTracking();
            var workflowEntites = _repositoryFactory.Search(predicate).ToArray();
            var organizationWorkflowDtos = new List<OrganizationWorkflowModel>();
            foreach (var item in workflowEntites)
            {
                organizationWorkflowDtos.Add(item.ToModel());
            }
            return organizationWorkflowDtos;

        }

        public OrganizationWorkflowModel Get(string workflowId)
        {
            var workflow = _repositoryFactory.Get(workflowId);
            return workflow.ToModel();
        }

        public WorkflowModel GetActiveWorkFlowDetail()
        {
            throw new NotImplementedException();
        }
        
        public OrganizationWorkflowModel GetActiveWorkflow()
        {
            throw new NotImplementedException();
        }
    }
}
