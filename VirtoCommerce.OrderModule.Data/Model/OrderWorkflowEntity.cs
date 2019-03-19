using System.ComponentModel.DataAnnotations;
using VirtoCommerce.OrderModule.Core.Models;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.OrderModule.Data.Model
{
    public class OrderWorkflowEntity : AuditableEntity
    {
        [StringLength(128)]
        [Required]
        public string OrderId { get; set; }
        [StringLength(128)]
        [Required]
        public string WorkflowId { get; set; }
        public OrderWorkflowModel ToModel()
        {
            return new OrderWorkflowModel
            {
                OrderId = OrderId,
                WorkflowId = WorkflowId              
            };
        }
    }


}
