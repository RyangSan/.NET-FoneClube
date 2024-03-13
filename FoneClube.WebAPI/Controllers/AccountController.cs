using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using FoneClube.WebAPI.Models;
using FoneClube.WebAPI.Providers;
using FoneClube.WebAPI.Results;
using Business.Commons.Entities;
using FoneClube.DataAccess;
using FoneClube.Business.Commons.Entities.FoneClube;

namespace FoneClube.WebAPI.Controllers
{

    [RoutePrefix("api/account")]
    public class AccountController : ApiController
    {


        // GET api/Account/UserInfo
        [Route("plans")]
        public List<Plan> GetPlans()
        {
            return new AccountAccess().GetPlans();
        }

        // GET api/Account/UserInfo
        [Route("plans/{id}")]
        public List<Plan> GetPlansById(int id)
        {
            return new AccountAccess().GetPlansById(id);
        }

        [Route("operators")]
        public List<Operator> GetOperators()
        {
            return new AccountAccess().GetOperators();
        }

    }
}
