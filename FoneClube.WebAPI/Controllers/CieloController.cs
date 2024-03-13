using FoneClube.Business.Commons.Entities.Cielo;
using FoneClube.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;

namespace FoneClube.WebAPI.Controllers
{
    [RoutePrefix("api/cielo")]
    public class CieloController : ApiController
    {
        [Route("transacao/{debitoTransaction}")]
        [HttpGet]
        public string CreateCieloCharging(string debitoTransaction)
        {
            var response = new CieloAccess().GenerateDebitoCharge(debitoTransaction);

            if (response.Contains("http"))
                HttpContext.Current.Response.Redirect(response);
            
            return response;
        }

        [Route("transacao/link")]
        [HttpGet]
        public string CreateCieloChargingTeste(string debitoTransaction)
        {
            var response = new CieloAccess().GenerateDebitoCharge(debitoTransaction);

            if (response.Contains("http"))
                HttpContext.Current.Response.Redirect(response);

            return response;
        }


        [Route("gera/link")]
        [HttpGet]
        public string CreateCieloCharging(CieloDebitoTransaction cieloDebitoTransaction)
        {
            return new CieloAccess().GenerateFirstLinkDebito(cieloDebitoTransaction);
        }

        [Route("debito/apto/{customerId}")]
        [HttpGet]
        public bool GetStatusDebit(string customerId)
        {
            return new CieloAccess().HasDebitCard(Convert.ToInt32(customerId));
        }

        [Route("transacao/retorno")]
        [HttpGet]
        public string GetCieloMessage()
        {
            return "O gateway de pagamento não conseguiu redirecionar para o seu bankline, verifique seus dados cadastrais ou entre em contato com o atendimento Foneclube.";
        }

        [Route("transaction/restore/{senha}")]
        [HttpGet]
        public bool GetRestoreTransactionCielo(string senha)
        {
            if (senha == "!@#mudar")
                return new CieloAccess().UpdateStatusTransactionsCielo();
            else
                return false;
        }

        [Route("transaction/restore/history/{id}")]
        [HttpPost]
        public HttpResponseMessage GetRestoreHistoryTransactionCielo(string id)
        {
            return new CieloAccess().GetRestoreHistoryTransactionCielo(Convert.ToInt32(id));   
        }

        //[Route("transaction/restore/history/teste")]
        //[HttpGet]
        //public HttpResponseMessage Get()
        //{
        //    var response = new HttpResponseMessage();
        //    //23
        //    response.Content = new StringContent("<div>Hello World</div>");
        //    response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
        //    return response;
        //}

    }
}