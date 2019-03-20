using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.OrderModule.Core.Models;

namespace VirtoCommerce.OrderModule.Web.Controllers.Model
{
    public class OrganizationWorkflowApi
    {
        public string OrganizationId { get; set; }
        public string WorkflowName { get; set; }
        public string JsonPath { get; set; }
        public bool Status { get; set; }

        public OrganizationWorkflow ToModel()
        {
            var model = new OrganizationWorkflow()
            {
                JsonPath = this.JsonPath,
                OrganizationId = this.OrganizationId,
                Status = this.Status,
                WorkflowName = this.WorkflowName
            };
            return model;
        }
    }
}
