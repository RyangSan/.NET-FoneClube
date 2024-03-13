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
using System.Configuration;
using Newtonsoft.Json;
using System.Net;
using PagarMe;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using FoneClube.Business.Commons.Utils;
using System.Collections.Specialized;
using System.Web.Http.ModelBinding;
using Nop.Plugin.Api.ModelBinders;
using Nop.Plugin.Api.DTOs.Customers;
using Nop.Plugin.Api.Delta;
using Nop.Plugin.Api.Factories;

namespace FoneClube.WebAPI.Tests.Controllers
{
    [TestClass]
    public class ExternalTest
    {
        [TestMethod]
        public void GetTransaction()
        {
            //var teste = new PagarMeController().UpdateTransactions();
            var ping = new System.Net.NetworkInformation.Ping();
            //var result = ping.Send("http://default-environment.p2badpmtjj.us-east-2.elasticbeanstalk.com/");
            //var oPing = new System.Net.NetworkInformation.Ping().Send("https://www.google.com.br/", 1000);
            //var teste = UrlIsValid("http://default-environment.p2badpmtjj.us-east-2.elasticbeanstalk.com/");
            var teste = UrlIsValid("http://djfj0fj90f4f9jf409f490fj09f.p2badpmtjj.us-east-2.elasticbeanstalk.com/");

        }

        public bool UrlIsValid(string url)
        {
            try
            {
                HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
                request.Timeout = 5000; //set the timeout to 5 seconds to keep the user from waiting too long for the page to load
                request.Method = "HEAD"; //Get only the header information -- no need to download any content

                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    int statusCode = (int)response.StatusCode;
                    if (statusCode >= 100 && statusCode < 400) //Good requests
                    {
                        return true;
                    }
                    else if (statusCode >= 500 && statusCode <= 510) //Server Errors
                    {
                        //log.Warn(String.Format("The remote server has thrown an internal error. Url is not valid: {0}", url));
                        //Debug.WriteLine(String.Format("The remote server has thrown an internal error. Url is not valid: {0}", url));
                        return false;
                    }
                }
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError) //400 errors
                {
                    return false;
                }
                else
                {
                    //log.Warn(String.Format("Unhandled status [{0}] returned for url: {1}", ex.Status, url), ex);
                }
            }
            catch (Exception ex)
            {
                //log.Error(String.Format("Could not test url {0}.", url), ex);
            }
            return false;
        }

        [TestMethod]
        public void GetCheckout()
        {
            var teste = new LojaAccess().GetPersonCheckoutLoja(15292);
        }

        [TestMethod]
        public void GetTeste()
        {
            var teste = new LojaAccess().GetPersonCheckoutLoja(15292);
        }

        [TestMethod]
        public void GetConsultaTransactionsCielo()
        {

            using (var ctx = new FoneClubeContext())
            {
                var chargings = ctx.tblChargingHistory.Where(c => c.intIdGateway == 2).ToList();
                foreach (var charge in chargings)
                {
                    var result = GetTransactionCielo(charge.txtPaymentId);
                    if (!string.IsNullOrEmpty(result))
                    {
                        var teste = 1;
                    }
                }

            }
            //string html = string.Empty;
            //string url = @"https://api.cieloecommerce.cielo.com.br/1/sales/fa221bec-f721-4321-932e-62f2c2180665";

            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            //request.ContentType = "application/json";
            //request.Accept = "application/json";
            //request.Headers["MerchantId"] = "99879411-cc78-4e23-875a-95c91639260b";
            //request.Headers["MerchantKey"] = "B5fz29Vtfj3MuJ4KQGwSLDiEFiVPmeSZ6bMG1QQw";
            //request.AutomaticDecompression = DecompressionMethods.GZip;


            //using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            //using (Stream stream = response.GetResponseStream())
            //using (StreamReader reader = new StreamReader(stream))
            //{
            //    html = reader.ReadToEnd();
            //}

            //Console.WriteLine(html);
        }

        public string GetTransactionCielo(string id)
        {
            try
            {
                string html = string.Empty;
                string url = @"https://api.cieloecommerce.cielo.com.br/1/sales/" + id;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.ContentType = "application/json";
                request.Accept = "application/json";
                request.Headers["MerchantId"] = "99879411-cc78-4e23-875a-95c91639260b";
                request.Headers["MerchantKey"] = "B5fz29Vtfj3MuJ4KQGwSLDiEFiVPmeSZ6bMG1QQw";
                request.AutomaticDecompression = DecompressionMethods.GZip;


                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    html = reader.ReadToEnd();
                }

                return html;
            }
            catch (Exception)
            {
                return string.Empty;
            }

        }



        [TestMethod]
        public void TestPostGatewayLog()
        {
            var teste = SendLogDebug();
        }

        public int SendLogDebug()
        {
            try
            {
                //TEST ENV
                string url = string.Format("http://homol-api.p2badpmtjj.us-east-2.elasticbeanstalk.com/api/charging/log/person/id/{0}", 0);
                var request = new Charging { SerializedCharging = "texto aqui 1" };

                using (var client = new HttpClient())
                {
                    var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
                    var response = client.PostAsync(url, content).GetAwaiter().GetResult();

                    if (response.IsSuccessStatusCode)
                    {
                        try
                        {
                            var result = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                            return 1;
                        }
                        catch
                        {
                        }
                    }
                    return 0;
                }
            }
            catch
            {
            }
            return 0;
        }

        [TestMethod]
        public void TestPostGatewayCustomers()
        {

            ApiGateway.EndPointApi = "http://homol-api.p2badpmtjj.us-east-2.elasticbeanstalk.com/";
            var link = "api/pagarme/cartao";
            //var link = "transactions";

            var teste = "{'Amount':5298,'DaysLimit':0,'BoletoInstructions':null,'Nome':'Rodrigo Cardozo','Email':'rodrigocardozop@gmail.com','DocumentNumber':'10667103767','Street':'Rua Enio Augusto De Mello','StreetNumber':'170','Neighborhood':'Cidade Nova','Zipcode':'28800000','Ddd':'11','Number':'23456789','CardHolderName':'MARCIO G FRANCO','CardNumber':'5491591007676748','CardExpirationDate':'1120','CardCvv':'630'}";
            var result = ApiGateway.SetConteudo(link, teste);


            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            PagarMeService.DefaultApiKey = ConfigurationManager.AppSettings["APIKEY"];
            PagarMeService.DefaultEncryptionKey = ConfigurationManager.AppSettings["ENCRYPTIONKEY"];
            var transaction = new Transaction();

            transaction.Customer = new PagarMe.Customer()
            {
                Name = "John Appleseed",
                Email = "jappleseed@apple.com",
                DocumentNumber = "92545278157",
                Address = new PagarMe.Address()
                {
                    Street = "rua teste",
                    StreetNumber = "200",
                    Neighborhood = "centro",
                    Zipcode = "01452000"
                },

                Phone = new PagarMe.Phone
                {
                    Ddd = "21",
                    Number = "26545625"
                }
            };

            transaction.Card = new Card
            {
                Id = "card_cjnholi2p014rb66dhfouvowr"
            };

            transaction.Amount = 100;

            //transaction.Customer.Save();
            transaction.Save();
        }

        public class CheckoutPagarmeJS
        {
            public string api_key { get; set; }
            public string encryption_key { get; set; }
            public string amount { get; set; }
            public string card_id { get; set; }
            public CustomerPagarmeJS customer { get; set; }
        }

        public class CustomerPagarmeJS
        {
            public string name { get; set; }
            public string document_number { get; set; }
            public string email { get; set; }
            public PagarmeAddressJS address { get; set; }
            public PagarmePhoneJS phone { get; set; }
        }

        public class PagarmePhoneJS
        {
            public string ddd { get; set; }
            public string number { get; set; }
        }

        public class PagarmeAddressJS
        {
            public string street { get; set; }
            public string street_number { get; set; }
            public string neighborhood { get; set; }
            public string zipcode { get; set; }
            public string city { get; set; }
            public string uf { get; set; }
        }

        public static int PostCustomerCpf(string cpf, string name, string email)
        {
            try
            {
                //TEST ENV
                string url = string.Format("http://homol-api.p2badpmtjj.us-east-2.elasticbeanstalk.com/api/profile/customer/partial/register");
                //string url = string.Format("http://default-environment.p2badpmtjj.us-east-2.elasticbeanstalk.com/api/profile/customer/partial/register");
                //posso colocar um envio de log com texto serializado tb

                var request = new PartialCustomerRegisterRequest(cpf, name, email);

                using (HttpClient client = new HttpClient())
                {
                    var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

                    var response = client.PostAsync(url, content).GetAwaiter().GetResult();

                    if (response.IsSuccessStatusCode)
                    {
                        try
                        {
                            string result = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                            return int.Parse(result);
                        }
                        catch
                        {
                        }
                    }
                    return 0;
                }
            }
            catch
            {
            }
            return 0;
        }

        public class CustomerByCPFResponse
        {
            public int Id { get; set; }
            public DateTime Register { get; set; }
            public int SinglePrice { get; set; }
            public bool Charged { get; set; }
            public int TotalBoletoCharges { get; set; }

        }

        public class PartialCustomerRegisterRequest
        {
            public PartialCustomerRegisterRequest(string documentNumber, string name, string email)
            {
                this.DocumentNumber = documentNumber;
                this.Name = name;
                this.Email = email;
            }
            public string DocumentNumber { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }


        }

        [TestMethod]
        public void ExampleCripto()
        {
            var utils = new CardUtils();
            string cardInfo = "teste_!@#$%¨&*(321654987+_}{`^";
            var card1 = "u2/zst0HImqg1O3smEjkglosqTTFZcAsqaamioXnJfhb49cDIMlaGjrBp8QB/q/DUYnt7ZPSm63rAzigoKP3ZZYhhgdjAC/Y+4VrT4+/b0k=";

            var teste = utils.GetCard(card1);

            var encripted = utils.EncryptFC(cardInfo);
            var decripted = utils.DecryptFC(encripted);
            Assert.AreEqual(cardInfo, decripted);

        }

        [TestMethod]
        public void ExampleValidCriptoCard()
        {
            var utils = new CardUtils();
            var stringCard = "u2/zst0HImqg1O3smEjkglosqTTFZcAsqaamioXnJfhb49cDIMlaGjrBp8QB/q/DUYnt7ZPSm63rAzigoKP3ZZYhhgdjAC/Y+4VrT4+/b0k=";
            var cartao = utils.GetCard(stringCard);
            var cartaoValido = utils.isValidCard(stringCard);

            Assert.AreEqual(true, cartaoValido);
        }

        [TestMethod]
        public void ExamplePrepareCriptoCard()
        {
            var utils = new CardUtils();
            var card = new CardFoneclube
            {
                Number = "87654567890910",
                HolderName = "Rodrigo C",
                Cvv = "238",
                ExpirationMonth = "10",
                ExpirationYear = "2020",
                Flag = "Master"
            };

            var preparedCard = utils.PrepareCard(card);
            var securedCard = utils.EncryptFC(preparedCard);
            var resultCard = utils.GetCard(securedCard);

            Assert.AreEqual(card.Number, resultCard.Number);
            Assert.AreEqual(card.HolderName, resultCard.HolderName);
            Assert.AreEqual(card.Cvv, resultCard.Cvv);
            Assert.AreEqual(card.ExpirationMonth, resultCard.ExpirationMonth);
            Assert.AreEqual(card.ExpirationYear, resultCard.ExpirationYear);
            Assert.AreEqual(card.Flag, resultCard.Flag);
        }


        [TestMethod]
        public void CanUseNopAPIGET()
        {
            var client = new ApiClient("nQML04NXduR_7ABWGf_RhvEgInxueNx6c-PUHW4Yl3HC3acStsOyzWqxrbYI6caf2ciY_eGUFdkdu-cc6Ch5vfWw_KgnqHiclGnWwQBhZsMNfAewYcPjsgDjgDkvHvdDK8tVoOEi7-WqSt_i0zWAlQbzMNcQOOuT8lCgFfv_jCmJuf7YWOWeyYyq-8fUHZqjBHSru_VC-nXESCsre06bz2kH-fE-4fEuNl1n_raOOc60rZvoiV1oVYQ5V6k0uU1jq31lE8Gy3vUR3W2Sy-mWPw", "https://loja.foneclube.com.br");
            var get = client.Get("api/customers");
            var deserialize = JsonConvert.DeserializeObject<Customers>(get.ToString());
        }

        [TestMethod]
        public void CanUseNopAPIPOST()
        {

            var client = new ApiClient("nQML04NXduR_7ABWGf_RhvEgInxueNx6c-PUHW4Yl3HC3acStsOyzWqxrbYI6caf2ciY_eGUFdkdu-cc6Ch5vfWw_KgnqHiclGnWwQBhZsMNfAewYcPjsgDjgDkvHvdDK8tVoOEi7-WqSt_i0zWAlQbzMNcQOOuT8lCgFfv_jCmJuf7YWOWeyYyq-8fUHZqjBHSru_VC-nXESCsre06bz2kH-fE-4fEuNl1n_raOOc60rZvoiV1oVYQ5V6k0uU1jq31lE8Gy3vUR3W2Sy-mWPw", "https://loja.foneclube.com.br");

            var customers = new Customers();
            customers.customers = new List<Customer>();

            customers.customers.Add(new Customer {
                username = "10667103755",
                email = "10667103755@hotmail.com",
                first_name = "Testenaldo da silva",
                last_name = "Costa",
                active = true,
                deleted = false,
                is_system_account = false
            });

            //var customerJson = JsonConvert.SerializeObject(new Customer
            //{
            //    username = "10667103755",
            //    email = "10667103755@hotmail.com",
            //    first_name = "Testenaldo da silva",
            //    last_name = "Costa",
            //    active = true,
            //    deleted = false,
            //    is_system_account = false
            //});
            //var customersJson = JsonConvert.SerializeObject(customers);

            //var dic = new Dictionary<string, object>();

            //dic.Add("customers", new object());

            //var customerDelta = new Delta<CustomerDto>(dic);
            //customerDelta.Dto.Username = "10667103755";
            //customerDelta.Dto.Email = "10667103755@hotmail.com";
            //customerDelta.Dto.FirstName = "Testenaldo da silva";
            //customerDelta.Dto.LastName = "Sila";
            //customerDelta.Dto.Active = true;
            //customerDelta.Dto.Deleted = false;
            //customerDelta.Dto.IsSystemAccount = false;
            //CreateCustomer(customerDelta);


            var teste = new CustomersRootObject();
            /*
            var customer = new CustomerDto();
            customer.Username = "10667103755";
            customer.Email = "10667103755@hotmail.com";
            customer.FirstName = "Testenaldo da silva";
            customer.LastName = "Sila";
            customer.Active = true;
            customer.Deleted = false;
            customer.IsSystemAccount = false;

            var customerJson = JsonConvert.SerializeObject(customer);

            var post = client.Post("api/customers", customerJson);

            */

            var customer = new CustomerDto();
            customer.Username = "10667103755";
            customer.Email = "10667103755@hotmail.com";
            customer.FirstName = "Testenaldo da silva";
            customer.LastName = "Sila";
            customer.Active = true;
            customer.Deleted = false;
            customer.IsSystemAccount = false;

            var root = new CustomersRootObject();
            root.Customers = new List<CustomerDto>();
            root.Customers.Add(customer);

            var customerJson = JsonConvert.SerializeObject(root);

            var post = client.Post("api/customers", customerJson);

        }


        private readonly IFactory<Customer> _factory;
        public void CreateCustomer([ModelBinder(typeof(JsonModelBinder<CustomerDto>))] Delta<CustomerDto> customerDelta)
        {
            Customer newCustomer = _factory.Initialize();
            customerDelta.Merge(newCustomer);
        }

        #region classes
        public class BillingAddress
        {
            public string id { get; set; }
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string email { get; set; }
            public string company { get; set; }
            public int? country_id { get; set; }
            public string country { get; set; }
            public int? state_province_id { get; set; }
            public string city { get; set; }
            public string address1 { get; set; }
            public string address2 { get; set; }
            public string zip_postal_code { get; set; }
            public string phone_number { get; set; }
            public string fax_number { get; set; }
            public object customer_attributes { get; set; }
            public DateTime? created_on_utc { get; set; }
            public string province { get; set; }
        }

        public class ShippingAddress
        {
            public string id { get; set; }
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string email { get; set; }
            public string company { get; set; }
            public int? country_id { get; set; }
            public string country { get; set; }
            public int? state_province_id { get; set; }
            public string city { get; set; }
            public string address1 { get; set; }
            public string address2 { get; set; }
            public string zip_postal_code { get; set; }
            public string phone_number { get; set; }
            public string fax_number { get; set; }
            public object customer_attributes { get; set; }
            public DateTime? created_on_utc { get; set; }
            public string province { get; set; }
        }

        public class Address
        {
            public string id { get; set; }
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string email { get; set; }
            public string company { get; set; }
            public int? country_id { get; set; }
            public string country { get; set; }
            public int? state_province_id { get; set; }
            public string city { get; set; }
            public string address1 { get; set; }
            public string address2 { get; set; }
            public string zip_postal_code { get; set; }
            public string phone_number { get; set; }
            public string fax_number { get; set; }
            public object customer_attributes { get; set; }
            public DateTime? created_on_utc { get; set; }
            public string province { get; set; }
        }

        public class Customer
        {
            public List<object> shopping_cart_items { get; set; }
            public BillingAddress billing_address { get; set; }
            public ShippingAddress shipping_address { get; set; }
            public List<Address> addresses { get; set; }
            public string id { get; set; }
            public string username { get; set; }
            public string email { get; set; }
            public string first_name { get; set; }
            public string last_name { get; set; }
            public object admin_comment { get; set; }
            public bool is_tax_exempt { get; set; }
            public bool has_shopping_cart_items { get; set; }
            public bool active { get; set; }
            public bool deleted { get; set; }
            public bool is_system_account { get; set; }
            public object system_name { get; set; }
            public string last_ip_address { get; set; }
            public DateTime? created_on_utc { get; set; }
            public DateTime? last_login_date_utc { get; set; }
            public DateTime? last_activity_date_utc { get; set; }
            public List<object> role_ids { get; set; }
        }

        public class Customers
        {
            public List<Customer> customers { get; set; }
        }
        #endregion

        #region exportar
        public class ApiClient
        {
            protected const string DefaultContentType = "application/json";
            private readonly string _accessToken;
            private readonly string _serverUrl;

            public ApiClient(string accessToken, string serverUrl)
            {
                _accessToken = accessToken;
                _serverUrl = serverUrl;
            }

            public object Call(HttpMethods method, string path)
            {
                return Call(method, path, string.Empty);
            }

            public object Call(HttpMethods method, string path, object callParams)
            {
                string requestUriString = string.Format("{0}/{1}", _serverUrl, path);

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUriString);

                httpWebRequest.ContentType = DefaultContentType;

                httpWebRequest.Headers.Add("Authorization", string.Format("Bearer {0}", _accessToken));

                httpWebRequest.Method = ((object)method).ToString();

                if(method == HttpMethods.Post)
                {
                    httpWebRequest.ContentType = "application/xml";
                }

                if (callParams != null)
                {
                    if (method == HttpMethods.Get || method == HttpMethods.Delete)
                    {
                        return string.Format("{0}?{1}", requestUriString, callParams);
                    }

                    if (method == HttpMethods.Post || method == HttpMethods.Put)
                    {
                        using (new MemoryStream())
                        {
                            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                            {
                                streamWriter.Write(callParams);
                                streamWriter.Close();
                            }
                        }
                    }
                }

                var httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                string encodedData = string.Empty;

                using (Stream responseStream = httpWebResponse.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        var streamReader = new StreamReader(responseStream);
                        encodedData = streamReader.ReadToEnd();
                        streamReader.Close();
                    }
                }

                return encodedData;
            }

            public object Get(string path)
            {
                return Get(path, null);
            }

            public object Get(string path, NameValueCollection callParams)
            {
                return Call(HttpMethods.Get, path, callParams);
            }

            public object Post(string path, object data)
            {
                return Call(HttpMethods.Post, path, data);
            }

            public object Put(string path, object data)
            {
                return Call(HttpMethods.Put, path, data);
            }

            public object Delete(string path)
            {
                return Call(HttpMethods.Delete, path);
            }

            public enum HttpMethods
            {
                Get,
                Post,
                Put,
                Delete,
            }
            #endregion

            #region
            

            #endregion
        }
    }
}
