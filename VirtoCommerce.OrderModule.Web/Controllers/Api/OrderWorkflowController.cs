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
    [RoutePrefix("api/orderworkflows")]
    public class OrderWorkflowController : ApiController
    {
        private readonly IMemberService _memberService;
        private readonly IMemberSearchService _memberSearchService;
        private readonly IOrderWorkflowService _orderWorkflowService;
        public OrderWorkflowController(IMemberService memberService, IMemberSearchService memberSearchService,
            IOrderWorkflowService orderWorkflowService
            )
        {
            _memberService = memberService;
            _memberSearchService = memberSearchService;
            _orderWorkflowService = orderWorkflowService;
        }


        [HttpGet]
        [Route("{orderId}")]
        [ResponseType(typeof(OrderWorkflowModel))]
        public IHttpActionResult Get(string orderId)
        {
            var orderWorkflow = _orderWorkflowService.GetOrderWorkflow(orderId);
            return Ok(new { data = orderWorkflow });
        }


        [HttpPost]
        [Route("")]
        [ResponseType(typeof(OrderWorkflowModel))]
        public IHttpActionResult Create([FromBody] OrderWorkflowModelApi apiModel)
        {
            OrderWorkflowModel model = new OrderWorkflowModel()
            {
                OrderId= apiModel.OrderId,
                WorkflowId = apiModel.OrganizationId
            };
            var orderWorkflow = _orderWorkflowService.AddOrderWorkflow(model);
            return Ok(new { data = orderWorkflow });
        }
    }
}

