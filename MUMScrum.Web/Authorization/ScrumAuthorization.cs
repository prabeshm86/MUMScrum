using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MUMScrum.Web.Helpers;
using MUMScrum.Model;

namespace MUMScrum.Web.Authorization
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
    public class ScrumAuthorization : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (Utility.GetEmployeeSession(httpContext.Session) != null)
            {
                var emp = Utility.GetEmployeeSession(httpContext.Session);
                if (string.IsNullOrEmpty(Roles)) return true;
                var arrRoles = Roles.Split(',');
                foreach (var role in arrRoles)
                {
                    if (emp.Role.RoleName.ToLower() == role.ToLower().Trim())
                        return true;
                }
            }
            return base.AuthorizeCore(httpContext);
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            // If they are authorized, handle accordingly
            if (this.AuthorizeCore(filterContext.HttpContext))
            {
                base.OnAuthorization(filterContext);
            }
            else
            {
                // Otherwise redirect to your specific authorized area
                filterContext.HttpContext.Session.Clear();
                filterContext.Result = new RedirectResult("~/Account/Login");
            }
        }
    }
}
