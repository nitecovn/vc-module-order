using System.Collections.Generic;

namespace VirtoCommerce.OrderModule.Core.Models
{
    /// <summary>
    /// Workflow states
    /// </summary>
    public class WorkflowState
    {
        public string Status { get; set; }
        public Dictionary<string, string[]> NextState { get; set; }
    }
}
