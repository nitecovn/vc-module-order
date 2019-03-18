using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VirtoCommerce.OrderModule.Data.Model;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.OrderModule.Data.Repositories
{
    public interface IOrganizationWorkflowRepository : IRepository
    {
        IQueryable<OrganizationWorkflowEntity> OrganizationWorkflows { get; }
        OrganizationWorkflowEntity Get(string id);
        Task<OrganizationWorkflowEntity[]> GetByOrganizationIdAsync(string organizationId);
        Task UpdateAsync(OrganizationWorkflowEntity entity);
        void AddWorkflow(OrganizationWorkflowEntity entity);
        OrganizationWorkflowEntity[] Search(Expression<Func<OrganizationWorkflowEntity, bool>> filter);
    }
}
