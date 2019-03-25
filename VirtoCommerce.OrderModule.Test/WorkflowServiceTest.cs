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
    [Trait("Workflow", "WF")]
    public class WorkflowServiceTest : AutofixtureTestBase
    {
        [Fact]
        public void SearchWorkflow_Should_Return_Workflows()
        {
            // Arrange
            var organizationId = Guid.NewGuid().ToString();

            var workflowEntity = this.AutoFixture.Build<WorkflowEntity>()
                                    .With(x => x.OrganizationId, organizationId)
                                    .With(x => x.Status, true)
                                    .Create();

            var workflowEntities = new List<WorkflowEntity> { workflowEntity };
            var workflows = workflowEntities.Select(x => x.ToModel(AbstractTypeFactory<Workflow>.TryCreateInstance()));

            var orderRepositoryMock = this.AutoFixture.Freeze<Mock<IOrderRepository>>();
            orderRepositoryMock.Setup(x => x.Workflows).Returns(workflowEntities.AsQueryable());

            // Act
            var workflowService = this.AutoFixture.Create<WorkflowService>();
            var workflowSearchCriteria = new WorkflowSearchCriteria
            {
                OrganizationId = organizationId
            };
            var result = workflowService.Search(workflowSearchCriteria);

            // Asserts
            result.Results.Should().Equal(workflows);
        }
    }
}
