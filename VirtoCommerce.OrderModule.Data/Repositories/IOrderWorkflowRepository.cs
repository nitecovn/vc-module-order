using System.Linq;
using VirtoCommerce.OrderModule.Data.Model;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.OrderModule.Data.Repositories
{
    public interface IOrderWorkflowRepository : IRepository
    {
        IQueryable<OrderWorkflowEntity> OrderWorkflows { get; }
    }
}
