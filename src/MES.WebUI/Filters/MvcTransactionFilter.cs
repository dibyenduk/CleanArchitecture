using MES.Persistence;
using MES.WebUI.App_Start;
using System.Web.Mvc;
using RequestContext = System.Web.Routing.RequestContext;

public class MvcTransactionFilter : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        if (ShouldFilter(filterContext.RequestContext))
        {
            var mesContext = StructuremapMvc.StructureMapDependencyScope.CurrentNestedContainer.GetInstance<MESDbContext>();
            mesContext.BeginTransaction();
        }
    }

    public override void OnActionExecuted(ActionExecutedContext filterContext)
    {
        if (ShouldFilter(filterContext.RequestContext))
        {
            var exception = filterContext.Exception;

            if (exception == null && filterContext.Controller.ViewData != null)
            {
                //var errorInfo = filterContext.Controller.ViewData.Model as ErrorInfoModel;

                //if (errorInfo != null)
                //{
                //    exception = errorInfo.Exception;
                //}
            }

            var mesContext = StructuremapMvc.StructureMapDependencyScope.CurrentNestedContainer.GetInstance<MESDbContext>();
            mesContext.CloseTransaction(exception);
        }
    }

    private bool ShouldFilter(RequestContext requestContext)
    {
        return !requestContext.HttpContext.Request.CurrentExecutionFilePathExtension.Contains("js");
    }
}