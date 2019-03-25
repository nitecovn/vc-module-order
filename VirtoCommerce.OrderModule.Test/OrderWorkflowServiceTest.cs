using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using Moq;
using VirtoCommerce.OrderModule.Core.Models;
using VirtoCommerce.OrderModule.Data.Model;
using VirtoCommerce.OrderModule.Data.Repositories;
using VirtoCommerce.OrderModule.Data.Services;
using VirtoCommerce.OrderModule.Test.Base;
using VirtoCommerce.Platform.Core.Common;
using Xunit;


namespace VirtoCommerce.OrderModule.Test
{
    [CLSCompliant(false)]
    [Trait("OrderWorkflow", "OWF")]
    public class OrderWorkflowServiceTest : AutofixtureTestBase
    {
        [Fact]
        public void GetOrderWorkflow_ByOrderId_Should_Return_Workflow()
        {
            //arrange
            string orderId = Guid.NewGuid().ToString();
            string workflowId = Guid.NewGuid().ToString();
            var orderWorkflowEntity = this.AutoFixture.Build<OrderWorkflowEntity>()
                                                      .With(x => x.OrderId, orderId)
                                                      .With(x => x.WorkflowId, workflowId)
                                                      .Create();
            var orderWorkflowEntities = new List<OrderWorkflowEntity> { orderWorkflowEntity };

            var orderRepositoryMock = this.AutoFixture.Freeze<Mock<IOrderRepository>>();
            orderRepositoryMock.Setup(x => x.OrderWorkflows).Returns(orderWorkflowEntities.AsQueryable());

            var orderWorkflow = orderWorkflowEntity.ToModel(AbstractTypeFactory<OrderWorkflow>.TryCreateInstance());

            //act            
            var orderService = this.AutoFixture.Create<OrderWorkflowService>();
            var result = orderService.GetOrderWorkflow(orderId);

            //assert
            result.Should().Be(orderWorkflow);
        }
    }
}
