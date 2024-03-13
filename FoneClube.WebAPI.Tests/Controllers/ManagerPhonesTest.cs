using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FoneClube.WebAPI;
using FoneClube.WebAPI.Controllers;
using Business.Commons.Entities;
using FoneClube.Business.Commons.Entities;
using FoneClube.DataAccess;
using FoneClube.Business.Commons.Entities.FoneClube;
using HttpService;
using System.Net.Mail;
using Business.Commons.Utils;

namespace FoneClube.WebAPI.Tests.Controllers
{
    [TestClass]
    public class ManagerPhonesTest
    {
        [TestMethod]
        public void CanCheckStatusClaro()
        {
            var linhaAtiva = new ClaroAccess().GetLineDetails("21982008200");
            Assert.AreEqual(linhaAtiva.Ativa, true);
            Assert.AreEqual(linhaAtiva.Bloqueada, false);


            var linhaBloqueada = new ClaroAccess().GetLineDetails("21966783658");
            Assert.AreEqual(linhaBloqueada.Ativa, false);
            Assert.AreEqual(linhaBloqueada.Bloqueada, true);

        }


        [TestMethod]
        public void CanCheckStatusVivo()
        {
            var linhaAtiva = new PhoneAccess().GetStatusLinhasOperadora();
            Assert.IsTrue(linhaAtiva.Count > 0);
            Assert.IsNotNull(linhaAtiva);
        }

        [TestMethod]
        public void GetAllPhones()
        {
            var linhaAtiva = new PhoneAccess().GetLinhasFoneclubeMinimal();

        }

        [TestMethod]
        public void GetTest()
        {
            //var APIStatus = GetStatusAPICorreto();
            //var teste = SendEmail("rodrigocardozop@gmail.com", "Título teste", "Corpo de email");

            //new Utils().DisparoAlertaZoho();new Email
           
            SendEmail("rodrigocardozop@gmail.com", "Teste", "Corpo do email");
        }

        public bool SendEmail(string to, string title, string body, bool bodyHtml = false)
        {
            try
            {

                var email = "foneclube@foneclube.com.br";
                var password = "foneclube01x02x03x";

                SmtpClient client = new SmtpClient();
                client.Port = 587;
                client.Host = "smtp.zoho.com";
                client.EnableSsl = true;
                client.Timeout = 10000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(email, password);

                MailMessage mm = new MailMessage(email, to, title, body);
                mm.IsBodyHtml = bodyHtml;
                mm.BodyEncoding = UTF8Encoding.UTF8;
                mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                client.Send(mm);


                return true;
            }
            catch (Exception ex)
            {
                try
                {
                    new Utils().DisparoAlertaZoho();
                }
                catch (Exception) { }
                
                return false;
            }
        }


        public bool GetStatusAPICorreto()
        {
            var pagarme = GetStatusPagarme();
            var database = GetDatabaseName();
            var localhost = GetLocalhostExecution();
            var financeiro = GetStatusFinanceiro();

            if (database != "\"foneclube-producao\"" || pagarme != "\"ak_live_ - ek_live_\"" || localhost != "\"false\"" || financeiro != "\"financeiro@foneclube.com.br\"")
            {
                return false;
            }

            return true;
        }

        public string GetStatusPagarme()
        {
            var link = "api/status/pagarme";
            ApiGateway.EndPointApi = "http://default-environment.p2badpmtjj.us-east-2.elasticbeanstalk.com/";
            var result = ApiGateway.GetConteudo(link);
            return result.ToString();
        }

        public string GetDatabaseName()
        {
            var link = "api/status/database/name";
            ApiGateway.EndPointApi = "http://default-environment.p2badpmtjj.us-east-2.elasticbeanstalk.com/";
            var result = ApiGateway.GetConteudo(link);
            return result.ToString();
        }

        public string GetVersion()
        {
            var link = "api/status/version";
            ApiGateway.EndPointApi = "http://default-environment.p2badpmtjj.us-east-2.elasticbeanstalk.com/";
            var result = ApiGateway.GetConteudo(link);
            return result.ToString();
        }

        public string GetLocalhostExecution()
        {
            var link = "api/status/localhost";
            ApiGateway.EndPointApi = "http://default-environment.p2badpmtjj.us-east-2.elasticbeanstalk.com/";
            var result = ApiGateway.GetConteudo(link);
            return result.ToString();
        }

        public string GetStatusFinanceiro()
        {
            var link = "api/status/financeiro";
            ApiGateway.EndPointApi = "http://default-environment.p2badpmtjj.us-east-2.elasticbeanstalk.com/";
            var result = ApiGateway.GetConteudo(link);
            return result.ToString();
        }
    }
}
