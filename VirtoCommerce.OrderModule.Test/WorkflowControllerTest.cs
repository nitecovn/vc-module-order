using System;
using System.Collections.Generic;
using System.Linq;
using CacheManager.Core;
using Moq;
using VirtoCommerce.OrderModule.Core.Models;
using VirtoCommerce.OrderModule.Data.Model;
using VirtoCommerce.OrderModule.Data.Repositories;
using VirtoCommerce.OrderModule.Data.Services;
using VirtoCommerce.Platform.Core.Assets;
using Xunit;

namespace VirtoCommerce.OrderModule.Test
{
    [CLSCompliant(false)]
    [Trait("Workflow", "WF")]
    public class WorkflowServiceTest
    {

        [Fact]
        public void GetSearchWorkflowShouldWork()
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
            Assert.Equal(TestData.Count, result.Results.Count);
        }
    }
}
