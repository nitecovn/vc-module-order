using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.OrderModule.Data.Model
{
    public class CustomerOrderWorkflowEntity : CustomerOrderEntity
    {
        public string WorkflowId { get; set; }

        public override void Patch(OperationEntity operation)
        {
            var target = operation as CustomerOrderWorkflowEntity;
            if (target != null)
            {
                target.WorkflowId = this.WorkflowId;
            }

            base.Patch(operation);

        }
    }
}
