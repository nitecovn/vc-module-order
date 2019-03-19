using System.Collections.Generic;
using VirtoCommerce.OrderModule.Core.Models;

namespace VirtoCommerce.OrderModule.Core.Services
{
    public interface IImportWorkflowService
    {
        OrganizationWorkflowModel ImportWorkflow(OrganizationWorkflowModel workflowModel);

        WorkflowModel GetWorkFlowDetailByOrganizationId(string organizationId);

        OrganizationWorkflowModel GetWorkFlowByOrganizationId(string organizationId);

        OrganizationWorkflowModel Get(string workflowId);

        WorkflowModel GetDetail(string workflowId);

        List<OrganizationWorkflowModel> Search(SearchCriteriaModel searchWorkflowCriteria);

        OrganizationWorkflowModel ImportOrUpdateWorkflow(OrganizationWorkflowModel workflowModel);

        string[] GetWorkflowStatusByOrganizationId(string organizationId);
    }
}
