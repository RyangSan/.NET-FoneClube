using System.Web.Http;
using FoneClube.DataAccess;
using Newtonsoft.Json.Linq;

namespace FoneClube.WebAPI.Controllers
{
    [RoutePrefix("callbacks/boletosimples")]
    public class BoletoSimplesController : ApiController
    {

        // POST callbacks/boletosimples/bankbillets
        [HttpPost]
        [Route("bankbillets")]
        public IHttpActionResult UpdateBankbilletStatus([FromBody] JToken body)
        {

            var result = new ChargingAcess().UpdateBankBilletStatus(body);
            return ResponseMessage(result);
        }


      


    }
}