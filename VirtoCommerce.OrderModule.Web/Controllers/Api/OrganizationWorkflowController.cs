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
    //[CheckPermission(Permission = WorkflowPredefinedPermissions.Read)]
    public class OrganizationWorkflowController : ApiController
    {
        private readonly IMemberService _memberService;
        private readonly IMemberSearchService _memberSearchService;
        private readonly IImportWorkflowService _importWorkflowService;

        public OrganizationWorkflowController(IMemberService memberService, IMemberSearchService memberSearchService, IImportWorkflowService importWorkflowService)
        {
            _memberService = memberService;
            _memberSearchService = memberSearchService;
            _importWorkflowService = importWorkflowService;
        }

       
        [HttpGet]
        [Route("{organizationId}")]
        [ResponseType(typeof(OrganizationWorkflowModel))]
        public IHttpActionResult Get(string organizationId)
        {
            var workflow = _importWorkflowService.GetWorkFlowByOrganizationId(organizationId);
            return Ok(new { data = workflow });
        }

        [HttpGet]
        [Route("detail/{organizationId}")]
        [ResponseType(typeof(WorkflowModel))]
        public IHttpActionResult GetDetail(string organizationId)
        {
            var workflow = _importWorkflowService.GetWorkFlowDetailByOrganizationId(organizationId);
            return Ok(new { data = workflow });
        }

        [HttpPost]
        [Route("")]
        [ResponseType(typeof(OrganizationWorkflowModel))]
        //[CheckPermission(Permission = WorkflowPredefinedPermissions.Upload)]
        public IHttpActionResult Upload([FromBody] OrganizationWorkflowModelApi workflowApiModel)
        {
            if (workflowApiModel == null)
                return Ok(new { });
            
            try
            {
                var model = new OrganizationWorkflowModel()
                {
                    JsonPath = workflowApiModel.JsonPath,
                    OrganizationId = workflowApiModel.OrganizationId,
                    Status = workflowApiModel.Status,
                    WorkflowName = workflowApiModel.WorkflowName
                };
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

