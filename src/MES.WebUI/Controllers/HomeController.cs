using MES.Core.Features.ViewProcessOrder;
using MES.WebUI.Controllers.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MES.WebUI.Controllers
{
    public class HomeController : BaseController
    {        
        public ActionResult Index()
        {
            var response = Mediator.Send(new SearchQuery() { }).Result;
            return View(response);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Modal()
        {
            ViewBag.Message = "Your modal.";

            return PartialView("_Modal");
        }
    }
}