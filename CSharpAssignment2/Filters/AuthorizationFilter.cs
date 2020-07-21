using System.Web;

namespace CSharpAssignment2.Filters
{
    public class AuthorizationFilter : System.Web.Mvc.ActionFilterAttribute, System.Web.Mvc.IActionFilter
    {
        public override void OnActionExecuting(System.Web.Mvc.ActionExecutingContext filterContext)
        {
            if (HttpContext.Current.Session["UserID"] == null)
            {
                filterContext.Result = new System.Web.Mvc.RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary
                {
                    {"Controller", "LoginRegister"},
                    {"Action", "Login"}
                });
            }
            base.OnActionExecuting(filterContext);
        }
    }
}