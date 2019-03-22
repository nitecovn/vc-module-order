using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CacheManager.Core;
using Moq;
using VirtoCommerce.Domain.Commerce.Model.Search;
using VirtoCommerce.Domain.Customer.Model;
using VirtoCommerce.OrderModule.Core.Models;
using VirtoCommerce.OrderModule.Core.Services;
using VirtoCommerce.OrderModule.Data.Model;
using VirtoCommerce.OrderModule.Data.Repositories;
using VirtoCommerce.OrderModule.Data.Services;
using VirtoCommerce.Platform.Core.Assets;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Infrastructure;
using Xunit;

namespace VirtoCommerce.OrderModule.Test
{
    [CLSCompliant(false)]
    [Trait("Workflow", "WF")]
    public class WorkflowControllerTest
    {

        [Fact]
        public void GetSearchWorkflowTest()
        {
            //arrange
            var orderRepositoryMock = new Mock<IOrderRepository>();
            var cacheManagerMock = new Mock<ICacheManager<object>>();
            Mock<IBlobStorageProvider> blobStorageProviderMock = new Mock<IBlobStorageProvider>();
            string filenameRead = "../../workflow/workflowjsonRead.json";

            string organizationId = Guid.NewGuid().ToString();
            List<WorkflowEntity> TestData = new List<WorkflowEntity>
            {
                new WorkflowEntity
                {
                    WorkflowName = "workflowname",
                    JsonPath = filenameRead,
                    OrganizationId = organizationId,
                    Status = true,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    Id = Guid.NewGuid().ToString()
                },
            };
            orderRepositoryMock.Setup(x => x.Workflows).Returns(TestData.AsQueryable());


            //act
            var workflowService = new WorkflowService(blobStorageProviderMock.Object, cacheManagerMock.Object, orderRepositoryMock.Object);
            WorkflowSearchCriteria searchWorkflowCriteria = new WorkflowSearchCriteria()
            {
                OrganizationId = organizationId
            };
            var result = workflowService.Search(searchWorkflowCriteria);


            //assert
            Assert.Equal(1, result.Results.Count);
        }
    }
}
