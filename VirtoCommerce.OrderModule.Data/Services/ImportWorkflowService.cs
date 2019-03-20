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
using VirtoCommerce.Domain.Commerce.Model.Search;

namespace VirtoCommerce.OrderModule.Data.Services
{
    public class ImportWorkflowService : ServiceBase, IWorkflowService
    {
        private readonly IBlobStorageProvider _blobStorageProvider;
        private readonly ICacheManager<object> _cacheManager;
        private IOrganizationWorkflowRepository _repositoryFactory;

        private const string CacheRegion = "WorkflowRegion";

        public ImportWorkflowService(IBlobStorageProvider blobStorageProvider,
            ICacheManager<object> cacheManager,
            IOrganizationWorkflowRepository repositoryFactory)
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
        public OrganizationWorkflow ImportOrUpdateWorkflow(OrganizationWorkflow workflowModel)
        {
            if (string.IsNullOrEmpty(workflowModel.JsonPath))
            {
                return ChangeWorkflowStatus(workflowModel.Status, workflowModel.OrganizationId);
            }
            return ImportWorkflow(workflowModel);
        }

        private OrganizationWorkflow ImportWorkflow(OrganizationWorkflow workflowModel)
        {
            var workflow = AbstractTypeFactory<OrganizationWorkflowEntity>.TryCreateInstance();
            workflow.FromModel(workflowModel);

            string result = IsValidJson(workflowModel.JsonPath);
            if (!string.IsNullOrEmpty(result))
                throw new Exception(result);
            using (var changeTracker = GetChangeTracker(_repositoryFactory))
            {
                changeTracker.Attach(workflow);
                _repositoryFactory.Add(workflow);
                CommitChanges(_repositoryFactory);
            }
            return workflow.ToModel();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="jsonPath"></param>
        /// <returns>if return empty -> is valid, else error</returns>
        private string IsValidJson(string jsonPath)
        {
            string jsonValue;
            using (var stream = _blobStorageProvider.OpenRead(jsonPath))
            {
                if (stream.Length == 0)
                    return "File is not empty";
                if (stream.Length > (1024 * 1024))
                    return "File is not over 1MB";

                var reader = new StreamReader(stream);
                jsonValue = reader.ReadToEnd();
            }

            try
            {
                var workFlow = JsonConvert.DeserializeObject<WorkflowDetail>(jsonValue);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return string.Empty;
        }

        public WorkflowDetail GetWorkFlowDetailByOrganizationId(string organizationId)
        {
            var model = GetWorkFlowByOrganizationId(organizationId);
            return GetWorkflowDetail(model);
        }

        public OrganizationWorkflow GetWorkFlowByOrganizationId(string organizationId)
        {
            _repositoryFactory.DisableChangesTracking();
            var workflows = _repositoryFactory.GetByOrganizationIdAsync(organizationId);

            if (workflows.Result.Any())
            {
                var workflow = workflows.Result[0];
                return workflow.ToModel();
            }
            return null;
        }

        private WorkflowDetail GetWorkflowDetail(OrganizationWorkflow model)
        {
            if (model == null)
                return null;
            string cacheKey = "Workflow_" + model.Id;
            return _cacheManager.Get(cacheKey, CacheRegion, () =>
            {
                string jsonValue;
                using (var stream = _blobStorageProvider.OpenRead(model.JsonPath))
                {
                    var reader = new StreamReader(stream);
                    jsonValue = reader.ReadToEnd();
                }
                var workFlow = JsonConvert.DeserializeObject<WorkflowDetail>(jsonValue);
                return workFlow;
            });
        }

        public OrganizationWorkflow ChangeWorkflowStatus(bool status, string organizationId)
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
                else
                {
                    throw new Exception("No workflow is updated");
                }
            }

        }
        public GenericSearchResult<OrganizationWorkflow> Search(WorkflowSearchCriteria searchWorkflowCriteria)
        {
            GenericSearchResult<OrganizationWorkflow> result = new GenericSearchResult<OrganizationWorkflow>();
            if (searchWorkflowCriteria == null) return result;

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
            var organizationWorkflowDtos = new List<OrganizationWorkflow>();
            foreach (var item in workflowEntites)
            {
                organizationWorkflowDtos.Add(item.ToModel());
            }
            result.Results = organizationWorkflowDtos;
            return result;

        }

        public OrganizationWorkflow Get(string workflowId)
        {
            var workflow = _repositoryFactory.Get(workflowId);
            if (workflow.Result != null)
            {
                return workflow.Result.ToModel();
            }
            return null;
        }

        public WorkflowDetail GetDetail(string workflowId)
        {
            var workflow = Get(workflowId);
            return GetWorkflowDetail(workflow);
        }
    }
}
