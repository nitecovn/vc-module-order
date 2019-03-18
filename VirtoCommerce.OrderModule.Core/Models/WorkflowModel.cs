using System.Collections.Generic;

namespace VirtoCommerce.OrderModule.Core.Models
{
    public class WorkflowModel
    {
        public IEnumerable<WorkflowStateModel> WorkflowStates { get; set; }
    }
}
