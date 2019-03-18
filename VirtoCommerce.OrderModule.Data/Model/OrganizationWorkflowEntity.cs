using System.ComponentModel.DataAnnotations;
using VirtoCommerce.OrderModule.Core.Models;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.OrderModule.Data.Model
{
    public class OrganizationWorkflowEntity : AuditableEntity
    {
        [StringLength(128)]
        [Required]
        public string OrganizationId { get; set; }
        [StringLength(128)]
        [Required]
        public string WorkflowName { get; set; }
        [StringLength(500)]
        [Required]
        public string JsonPath { get; set; }
        public bool Status { get; set; }
        
        public OrganizationWorkflowModel ToModel()
        {
            return new OrganizationWorkflowModel
            {
                Id = Id,
                OrganizationId = OrganizationId,
                WorkflowName = WorkflowName,
                JsonPath = JsonPath,
                Status = Status,
                CreatedDate = CreatedDate,
                ModifiedDate = ModifiedDate
            };
        }
    }


}
