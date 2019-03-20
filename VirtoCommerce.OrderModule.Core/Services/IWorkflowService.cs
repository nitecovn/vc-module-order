using VirtoCommerce.Domain.Commerce.Model.Search;
using VirtoCommerce.OrderModule.Core.Models;

namespace VirtoCommerce.OrderModule.Core.Services
{
    public interface IWorkflowService
    {
        OrderWorkflow ImportOrUpdateWorkflow(OrderWorkflow workflowModel);

        OrderWorkflow GetByOrganizationId(string organizationId);

        GenericSearchResult<OrderWorkflow> Search(WorkflowSearchCriteria searchWorkflowCriteria);
    }
}
