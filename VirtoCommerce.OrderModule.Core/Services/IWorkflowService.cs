using System.Collections.Generic;
using VirtoCommerce.Domain.Commerce.Model.Search;
using VirtoCommerce.OrderModule.Core.Models;

namespace VirtoCommerce.OrderModule.Core.Services
{
    public interface IWorkflowService
    {
        WorkflowDetail GetWorkFlowDetailByOrganizationId(string organizationId);

        OrganizationWorkflow GetWorkFlowByOrganizationId(string organizationId);

        OrganizationWorkflow Get(string workflowId);

        WorkflowDetail GetDetail(string workflowId);

        GenericSearchResult<OrganizationWorkflow> Search(WorkflowSearchCriteria searchWorkflowCriteria);

        OrganizationWorkflow ImportOrUpdateWorkflow(OrganizationWorkflow workflowModel);
    }
}
