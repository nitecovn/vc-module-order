using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using VirtoCommerce.OrderModule.Data.Model;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;

namespace VirtoCommerce.OrderModule.Data.Repositories
{

    public class OrderWorkflowRepositoryImpl : EFRepositoryBase, IOrderWorkflowRepository
    {

        public OrderWorkflowRepositoryImpl()
        {
        }

        public OrderWorkflowRepositoryImpl(string nameOrConnectionString, params IInterceptor[] interceptors)
            : base(nameOrConnectionString, null, interceptors)
        {
            Configuration.LazyLoadingEnabled = false;
        }

        public OrderWorkflowRepositoryImpl(DbConnection existingConnection, IUnitOfWork unitOfWork = null, IInterceptor[] interceptors = null)
            : base(existingConnection, unitOfWork, interceptors)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Entity<OrderWorkflowEntity>()
                .ToTable("OrderWorkflow")
                .HasKey(x => x.Id)
                .Property(x => x.Id);

            base.OnModelCreating(modelBuilder);
        }

        public IQueryable<OrderWorkflowEntity> OrderWorkflows { get; }
    }
}
