using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VirtoCommerce.OrderModule.Web.Controllers.Model
{
    public class OrganizationWorkflowModelApi
    {
        public string OrganizationId { get; set; }
        public string WorkflowName { get; set; }
        public string JsonPath { get; set; }
        public bool Status { get; set; }
    }
}
