using System.Collections.Generic;
using VirtoCommerce.OrderModule.Core.Models;

namespace VirtoCommerce.OrderModule.Core.Services
{
    public interface IImportWorkflowService
    {
        OrganizationWorkflowModel ImportWorkflow(OrganizationWorkflowModel workflowModel);

        WorkflowModel GetWorkFlowDetailByOrganizationId(string organizationId);

        WorkflowModel GetActiveWorkFlowDetail();

        OrganizationWorkflowModel Get(string workflowId);

        OrganizationWorkflowModel GetActiveWorkflow();

        //void ChangeWorkflowStatus(bool status, string organizationId);

        List<OrganizationWorkflowModel> Search(SearchCriteriaModel searchWorkflowCriteria);

        OrganizationWorkflowModel ImportOrUpdateWorkflow(OrganizationWorkflowModel workflowModel);
    }
}
