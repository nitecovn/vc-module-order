using System.Collections.Generic;
using VirtoCommerce.OrderModule.Core.Models;

namespace VirtoCommerce.OrderModule.Core.Services
{
    public interface IOrderWorkflowService
    {
        string[] GetWorkflowStatus(string orderId);

        bool HasCustomWorkflow(string orderId);
        OrderWorkflowModel GetOrderWorkflow(string orderId);
        OrderWorkflowModel AddOrderWorkflow(OrderWorkflowModel orderWorkflowModel);        
    }
}
