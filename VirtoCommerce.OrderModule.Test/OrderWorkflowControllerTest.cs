using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using VirtoCommerce.OrderModule.Data.Model;
using VirtoCommerce.OrderModule.Data.Repositories;
using VirtoCommerce.OrderModule.Data.Services;
using VirtoCommerce.Platform.Testing.Bases;
using Xunit;



namespace VirtoCommerce.OrderModule.Test
{
    [CLSCompliant(false)]
    [Trait("OrderWorkflow", "OWF")]
    public class OrderWorkflowServiceTest : TestBase
    {

        [Fact]
        public void GetOrderWorkflowShouldWork()
        {
            //arrange
            var orderRepositoryMock = new Mock<IOrderRepository>();
            string workflowId = Guid.NewGuid().ToString();
            string orderId = Guid.NewGuid().ToString();
            List<OrderWorkflowEntity> OrderWorkflowTestData = new List<OrderWorkflowEntity>
            {
                new OrderWorkflowEntity
                {
                    WorkflowId = workflowId,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    OrderId = orderId,
                    Id = Guid.NewGuid().ToString(),

                },
            };
            orderRepositoryMock.Setup(x => x.OrderWorkflows).Returns(OrderWorkflowTestData.AsQueryable());

            //act            
            var orderService = new OrderWorkflowService(orderRepositoryMock.Object);
            var result = orderService.GetOrderWorkflow(orderId);

            //assert
            Assert.Equal(workflowId, result.WorkflowId);
        }
    }
}
