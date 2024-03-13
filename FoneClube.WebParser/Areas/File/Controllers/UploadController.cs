using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using VideoUpload.Areas.Video.Model;
using VideoUpload.Utilidades;
using System.IO;
using Febraban;
using FoneClube.DataAccess;
using FoneClube.ParserFebraban;

namespace VideoUpload.Areas.Video.Controllers
{
    public class UploadController : Controller
    {
        //
        // GET: /Video/Upload/
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public bool Post(string dataUpload)
        {


            //for (int i = 0; i < Request.Files.Count; i++)
            //{
            //    var fileBytes = Util.ObterByteArray(Request.Files[i]);
            //    var reader = new StreamReader(new MemoryStream(fileBytes), Encoding.UTF8);

               
            //    var conta = new FebrabanParser().ParserStreamReader(reader);
            //    //conta.TipoOperadora = 1;
            //    return new ContaAcesso().SaveConta(conta);
            //}

            for (int i = 0; i < Request.Files.Count; i++)
            {
                var fileBytes = Util.ObterByteArray(Request.Files[i]);
                return new ContaService().ProcessarConta(fileBytes);
            }


            return false;
        }

    }
}
