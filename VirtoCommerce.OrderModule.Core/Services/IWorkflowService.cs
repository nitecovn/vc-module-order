using VirtoCommerce.OrderModule.Core.Models;

namespace VirtoCommerce.OrderModule.Core.Services
{
    public interface IWorkflowService
    {
        OrderWorkflow ImportOrUpdateWorkflow(OrderWorkflow workflowModel);
    }
}
