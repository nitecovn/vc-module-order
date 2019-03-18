using System.Collections.Generic;

namespace VirtoCommerce.OrderModule.Core.Models
{
    public class WorkflowStateModel
    {
        public string Status { get; set; }
        public Dictionary<string, string[]> NextState { get; set; }
    }
}
