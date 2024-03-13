using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FoneClube.DataAccess;
using FoneClube.Business.Commons.Entities;
using FoneClube.Business.Commons.Entities.FoneClube;
using FoneClube.DataAccess.security;
using FoneClube.DataAccess.Utilities;

namespace FoneClube.WebAPI.Controllers
{
    [RoutePrefix("api/intl")]
    public class HomeController : Controller
    {
        [Route("action")]
        [HttpPost]
        public FacilActionResponse PerformAction(FacilActionRequest actions)
        {
           return new FacilIntlAccess().PerformAction(actions);
        }
    }
}
