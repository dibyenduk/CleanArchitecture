using MediatR;
using MES.WebUI.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MES.WebUI.Controllers.Shared
{
    public abstract class BaseController : Controller
    {
        //public Mediator Mediator { get; set; }

        private IMediator _mediator;

        protected IMediator Mediator => _mediator ?? (_mediator = StructuremapMvc.StructureMapDependencyScope.Container.GetInstance<IMediator>());

        //public BaseController(Mediator mediator)
        //{
        //    this.Mediator = mediator;
        //}
    }
}