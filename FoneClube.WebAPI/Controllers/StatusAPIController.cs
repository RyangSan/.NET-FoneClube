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
using FoneClube.Business.Commons.Entities;
using FoneClube.DataAccess;
using System.Net;
using FoneClube.Business.Commons.Entities.FoneClube;
using System.Configuration;

namespace FoneClube.WebAPI.Controllers
{
    [RoutePrefix("api/status")]
    public class StatusAPIController : ApiController
    {
        [Route("database/name")]
        public string GetCliente()
        {
            return new StatusAPIAccess().GetDatabaseName();
        }

        [Route("version")]
        public string GetVersion()
        {
            return ConfigurationManager.AppSettings["VersaoAPI"];
        }

        [Route("pagarme")]
        public string GetStatusPagarme()
        {
            return ConfigurationManager.AppSettings["APIKEY"].Substring(0, 8) + " - " +ConfigurationManager.AppSettings["ENCRYPTIONKEY"].Substring(0, 8);
        }

        [Route("financeiro")]
        public string GetEmailFinanceiro()
        {
            return ConfigurationManager.AppSettings["EmailFinanceiro"];
        }

        [Route("localhost")]
        public string GetLocalHost()
        {
            return ConfigurationManager.AppSettings["ExecutandoLocalHost"];
        }

        [Route("full")]
        public FoneClube.Business.Commons.Entities.Configuration GetfullStatus()
        {
            return new FoneClube.Business.Commons.Entities.Configuration {
                QrCode = ConfigurationManager.AppSettings["qrcodelink"],
                Database = new StatusAPIAccess().GetDatabaseName(),
                Version = ConfigurationManager.AppSettings["VersaoAPI"],
                Financeiro = ConfigurationManager.AppSettings["EmailFinanceiro"],
                Localhost = ConfigurationManager.AppSettings["ExecutandoLocalHost"],
                Pagarme = ConfigurationManager.AppSettings["APIKEY"].Substring(0, 8) + " - " + ConfigurationManager.AppSettings["ENCRYPTIONKEY"].Substring(0, 8)
            };
        }


    }
}