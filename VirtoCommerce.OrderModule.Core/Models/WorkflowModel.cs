using System.Collections.Generic;

namespace VirtoCommerce.OrderModule.Core.Models
{
    public class WorkflowDetail
    {
        public IEnumerable<WorkflowState> WorkflowStates { get; set; }
    }
}
