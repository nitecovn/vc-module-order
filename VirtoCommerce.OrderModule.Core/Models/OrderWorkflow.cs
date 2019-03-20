using System;
using System.Collections.Generic;

namespace VirtoCommerce.OrderModule.Core.Models
{
    public class OrderWorkflow
    {
        public string Id { get; set; }
        public string OrganizationId { get; set; }
        public string WorkflowName { get; set; }
        public string JsonPath { get; set; }
        public bool Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public WorkflowStates WorkflowStates { get; set; }
    }
}
