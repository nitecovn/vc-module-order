using VirtoCommerce.Domain.Commerce.Model.Search;

namespace VirtoCommerce.OrderModule.Core.Models
{
    public class WorkflowSearchCriteria: SearchCriteriaBase
    {
        public string OrganizationId { get; set; }
        public string WorkflowName { get; set; }
        public bool? Active { get; set; }       
    }
}
