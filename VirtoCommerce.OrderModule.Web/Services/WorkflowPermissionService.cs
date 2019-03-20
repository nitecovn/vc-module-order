using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.OrderModule.Core.Services;
using VirtoCommerce.OrderModule.Web.Security;

namespace VirtoCommerce.OrderModule.Web.Services
{
    public class WorkflowPermissionService : IWorkflowPermissionService
    {
        public string GetManagerPermission()
        {
            return OrderPredefinedPermissions.Manager;
        }
    }
}
