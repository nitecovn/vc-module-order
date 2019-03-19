using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VirtoCommerce.OrderModule.Data.Model;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.OrderModule.Data.Repositories
{
    public interface IOrderWorkflowRepository : IRepository
    {
        IQueryable<OrderWorkflowEntity> OrderWorkflows { get; }
        Task<OrderWorkflowEntity> GetOrderWorkflow(string orderId);
        void AddOrderWorkflow(OrderWorkflowEntity entity);
    }
}
