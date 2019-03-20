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
    public class OrganizationWorkflowController : ApiController
    {
        private readonly IWorkflowService _workflowService;

        public OrganizationWorkflowController(IWorkflowService workflowService)
        {
            _workflowService = workflowService;
        }

        [HttpPost]
        [Route("")]
        [ResponseType(typeof(OrderWorkflow))]
        [CheckPermission(Permission = WorkflowPredefinedPermissions.Upload)]
        public IHttpActionResult Upload([FromBody] OrderWorkflow model)
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
    }
}
