using System.ComponentModel.DataAnnotations;
using VirtoCommerce.OrderModule.Core.Models;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.OrderModule.Data.Model
{
    public class WorkflowEntity : AuditableEntity
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

        public virtual Workflow ToModel()
        {
            return new Workflow
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

        public virtual WorkflowEntity FromModel(Workflow model)
        {
            Id = model.Id;
            OrganizationId = model.OrganizationId;
            WorkflowName = model.WorkflowName;
            JsonPath = model.JsonPath;
            Status = model.Status;
            return this;
        }

        public virtual void Patch(WorkflowEntity target)
        {
            Id = target.Id;
            OrganizationId = target.OrganizationId;
            WorkflowName = target.WorkflowName;
            JsonPath = target.JsonPath;
            Status = target.Status;
        }
    }
}
