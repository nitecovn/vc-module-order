using System;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Domain.Customer.Services;
using VirtoCommerce.OrderModule.Core.Models;
using VirtoCommerce.OrderModule.Core.Services;
using VirtoCommerce.OrderModule.Web.Controllers.Model;
using VirtoCommerce.OrderModule.Web.Security;
using VirtoCommerce.Platform.Core.Web.Security;

namespace VirtoCommerce.OrderModule.Web.Controllers.Api
{
    [RoutePrefix("api/workflows")]
    [CheckPermission(Permission = WorkflowPredefinedPermissions.Read)]
    public class OrganizationWorkflowController : ApiController
    {
        private readonly IMemberService _memberService;
        private readonly IMemberSearchService _memberSearchService;
        private readonly IWorkflowService _importWorkflowService;

        public OrganizationWorkflowController(IMemberService memberService, IMemberSearchService memberSearchService, IWorkflowService importWorkflowService)
        {
            _memberService = memberService;
            _memberSearchService = memberSearchService;
            _importWorkflowService = importWorkflowService;
        }

       
        [HttpGet]
        [Route("{organizationId}")]
        [ResponseType(typeof(OrganizationWorkflow))]
        public IHttpActionResult Get(string organizationId)
        {
            var workflow = _importWorkflowService.GetWorkFlowByOrganizationId(organizationId);
            return Ok(new { data = workflow });
        }

        [HttpGet]
        [Route("detail/{organizationId}")]
        [ResponseType(typeof(WorkflowDetail))]
        public IHttpActionResult GetDetail(string organizationId)
        {
            var workflow = _importWorkflowService.GetWorkFlowDetailByOrganizationId(organizationId);
            return Ok(new { data = workflow });
        }

        [HttpPost]
        [Route("")]
        [ResponseType(typeof(OrganizationWorkflow))]
        [CheckPermission(Permission = WorkflowPredefinedPermissions.Upload)]
        public IHttpActionResult Upload([FromBody] OrganizationWorkflowApi workflowModelApi)
        {
            if (workflowModelApi == null)
                return Ok(new { });
            
            try
            {
                var model = workflowModelApi.ToModel();
                var workflow = _importWorkflowService.ImportOrUpdateWorkflow(model);
                return Ok(new { data = workflow });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
            
        }

        


       

    }
}

