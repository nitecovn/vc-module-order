using System;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.OrderModule.Core.Models;
using VirtoCommerce.OrderModule.Core.Services;
using VirtoCommerce.OrderModule.Web.Security;
using VirtoCommerce.Platform.Core.Web.Security;

namespace VirtoCommerce.OrderModule.Web.Controllers.Api
{
    [RoutePrefix("api/orderworkflow")]
    [CheckPermission(Permission = WorkflowPredefinedPermissions.Read)]
    public class WorkflowController : ApiController
    {
        private readonly IWorkflowService _workflowService;

        public WorkflowController(IWorkflowService workflowService)
        {
            _workflowService = workflowService;
        }

        [HttpPost]
        [Route("")]
        [ResponseType(typeof(Workflow))]
        [CheckPermission(Permission = WorkflowPredefinedPermissions.Upload)]
        public IHttpActionResult Upload([FromBody] Workflow model)
        {
            if (model == null)
                return BadRequest();
            try
            {
                var workflow = _workflowService.ImportOrUpdateWorkflow(model);
                return Ok(new { data = workflow });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("{organizationId}")]
        [ResponseType(typeof(Workflow))]
        public IHttpActionResult GetDetail(string organizationId)
        {
            var workflow = _workflowService.GetByOrganizationId(organizationId);
            return Ok(new { data = workflow });
        }
    }
}
