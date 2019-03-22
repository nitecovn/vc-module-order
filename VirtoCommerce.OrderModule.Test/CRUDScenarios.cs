using Moq;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.Domain.Order.Services;
using VirtoCommerce.OrderModule.Data.Model;
using VirtoCommerce.OrderModule.Data.Repositories;
using VirtoCommerce.OrderModule.Data.Services;
using Xunit;

namespace VirtoCommerce.OrderModule.Test
{
    [Trait("Category", "Unit")]
    public class CRUDScenarios
    {
        [Fact]
        public void Can_search_by_OrganizationId()
        {
            //Arrange
            var mockRepository = new Moq.Mock<IOrderRepository>();
            var sampleData = new List<CustomerOrderEntity>(new[] { new CustomerOrderEntity { Id = "1", OrganizationId = "myOrg1" }, new CustomerOrderEntity { Id = "2" } });
            mockRepository.Setup(x => x.CustomerOrders).Returns(sampleData.AsQueryable());
            mockRepository.Setup(x => x.GetCustomerOrdersByIds(It.IsAny<string[]>(), It.IsAny<CustomerOrderResponseGroup>())).Returns(new[] { sampleData.First() });
            var orderService = new CustomerOrderServiceImpl(() => mockRepository.Object, null, null, null, null, null, null, null, new Moq.Mock<ICustomerOrderTotalsCalculator>().Object);

            //act
            var result = orderService.SearchCustomerOrders(new CustomerOrderSearchCriteria { OrganizationId = "myOrg1" });
            //assert
            Assert.Single(result.Results, x => x.Id == "1");
            Assert.Equal(1, result.TotalCount);
        }

    }
}
