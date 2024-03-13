using System;
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
using Business.Commons.Utils;
using FoneClube.Business.Commons.Entities.FoneClube;
using System.Data.SqlClient;
using HttpService;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Data.Entity.Core.EntityClient;
using System.Configuration;
using FoneClube.Business.Commons.Entities.Claro.proceduresResults;
using FoneClube.Business.Commons.Entities.woocommerce;
using System.IO;
using FoneClube.Business.Commons.Entities.woocommerce.order;
using FoneClube.DataAccess.security;
using System.Net;

namespace FoneClube.WebAPI.Tests.Controllers
{
    [TestClass]
    public class ProfilesControllerTest
    {

        [TestMethod]
        public void CanInsertPerson()
        {

            var controller = new ProfileController();
            var document = 10767111787;

            //TODO
            //criar caso de insert perfeito
            //depois quebrar em cada caso de validação pra testar remoção de nova pessoa  após a quebra em casa parte

            var insertPerson = controller.InsertCadastro(new Person
            {
                Name = "Rodrigo Cardozo",
                NickName = "nicknameteste",
                Email = "rodrigocardozop@gmail.com",
                DocumentNumber = document.ToString(),
                Register = DateTime.Now,
                Born = "08/11/1988",
                Gender = 0,
                IdPagarme = 0,
                IdRole = 0,
                PhoneNumberParent = "21999999999",
                IdPlanOption = 4
            });
            // }

            using (var ctx = new FoneClubeContext())
            {
                Assert.IsNotNull(ctx.tblPersons.FirstOrDefault(p => p.txtDocumentNumber == document.ToString()).txtEmail);
            }
        }

        [TestMethod]
        public void InsertDataBasict()
        {
            using (var ctx = new FoneClubeContext())
            {
                var teste = ctx.tblPersons.Any(p => p.txtDocumentNumber.Trim() == "14445613700");
            }
        }

        [TestMethod]
        public void TestListPlansPhones()
        {
            var person = new Person { Phones = new List<Phone>() };
            person.Phones.Add(new Phone { DDD = "21", Number = "22222222", IdPlanOption = 1 });
            person.Phones.Add(new Phone { DDD = "21", Number = "22222223" });
            person.Phones.Add(new Phone { DDD = "21", Number = "22222224" });

            var hasPlan = person.Phones.Any(p => p.IdPlanOption > 0);
        }


        [TestMethod]
        public void InsertDataBasic()
        {
            using (var ctx = new FoneClubeContext())
            {
                var tblPersons = ctx.tblPersons.ToList();
                var listaPessoas = new List<tblPersons>();
                foreach (var person in tblPersons)
                {
                    var hasAdress = ctx.tblPersonsAddresses.Any(p => p.intIdPerson == person.intIdPerson);
                    var hasPhone = ctx.tblPersonsPhones.Any(p => p.intIdPerson == person.intIdPerson);

                    if (!hasAdress || !hasPhone)
                    {
                        listaPessoas.Add(person);
                    }
                }

                var teste = listaPessoas;


                foreach (var person in listaPessoas)
                {



                    ctx.tblPersonsAddresses.Add(new tblPersonsAddresses
                    {
                        intIdPerson = person.intIdPerson,
                        txtCity = "Empty",
                        txtCountry = "Empty",
                        txtNeighborhood = "Empty",
                        txtState = "Empty",
                        txtStreet = "Empty",
                        txtCep = "23912400",
                        intStreetNumber = 0
                    });


                    ctx.tblPersonsPhones.Add(new tblPersonsPhones
                    {
                        intIdPerson = person.intIdPerson,
                        intDDD = 21,
                        intPhone = 22222222
                    });

                    ctx.SaveChanges();

                }





            }
        }

        [TestMethod]
        public void CanUpdatePerson()
        {

            var controller = new ProfileController();
            var document = 10667103767;


            var insertPerson = controller.UpdatePerson(new Person
            {
                Name = "Rodrigo Cardozo",
                DocumentNumber = document.ToString(),
                Email = "rodrigocardozop@gmail.com",
                Gender = 0
            });

            using (var ctx = new FoneClubeContext())
            {
                Assert.IsNotNull(ctx.tblPersons.FirstOrDefault(p => p.txtDocumentNumber == document.ToString()).txtEmail);
            }
        }

        [TestMethod]
        public void CanSendEmail()
        {
            var sendEmail = new Utils().SendEmail("rodrigocardozop@gmail.com", "Seu Boleto de cobrança foi gerado.", "Aqui corpo da mensagem");
            //var sendEmail = new Utils().SendEmail(Utils.EmailTo.Vendas, "Cobrança realizada.", string.Format(" Foi realizado uma cobrança no valor de {0} Reais para o cliente: {1}\n Id:{2} \n Observação:{3} \n Tipo de cobrança:{4} \n Agente:{4}", (Convert.ToDouble(1000) / 100), "Nome cliente", 20, "teste cobrança", "boleto", "cardozo"));
            //var sendEmail = new Utils().SendEmail(Utils.EmailTo.Vendas, "Cobrança realizada.", string.Format(" Foi realizado uma cobrança no valor de {0} Reais para o cliente id: {1}\n Observação:{2} \n Tipo de cobrança:{3} \n Agente:{4}", (Convert.ToDouble(1000) / 100), 1, "teste de comentário", "boleto", "cardozo"));
            Assert.IsTrue(sendEmail);
        }

        [TestMethod]
        public void CanSendEmailDinamic()
        {
            var sendEmail = new EmailAccess().SendEmailDinamic(new Email
            {
                TemplateType = 24,
                To = "rodrigocardozop@gmail.com",
                TargetName = "Rodrigo",
                TargetTextBlue = "<a href='https://www.google.com.br/'>https://www.google.com.br/</a>",
                TargetSecondaryText = string.Empty,
                DiscountPrice = "<a href='https://www.google.com.br/'>https://www.google.com.br/</a>"

            });
            //var sendEmail = new Utils().SendEmail(Utils.EmailTo.Vendas, "Cobrança realizada.", string.Format(" Foi realizado uma cobrança no valor de {0} Reais para o cliente: {1}\n Id:{2} \n Observação:{3} \n Tipo de cobrança:{4} \n Agente:{4}", (Convert.ToDouble(1000) / 100), "Nome cliente", 20, "teste cobrança", "boleto", "cardozo"));
            //var sendEmail = new Utils().SendEmail(Utils.EmailTo.Vendas, "Cobrança realizada.", string.Format(" Foi realizado uma cobrança no valor de {0} Reais para o cliente id: {1}\n Observação:{2} \n Tipo de cobrança:{3} \n Agente:{4}", (Convert.ToDouble(1000) / 100), 1, "teste de comentário", "boleto", "cardozo"));
            Assert.IsTrue(sendEmail);
        }

        [TestMethod]
        public void CanSendEmailMultiple()
        {
            var sendEmail = new Utils().SendEmailMultiple("rodrigocardozop@gmail.com;rod_cardozo@hotmail.com",
                "Teste multiplos destinatários.",
                "Aqui corpo da mensagem",
                "isaabelafreis@gmail.com",
                "");
        }

        [TestMethod]
        public void CanSendEmailMultipleTIM()
        {
            var sendEmail = new Utils().SendEmailMultiple("rodrigocardozop@gmail.com;rod_cardozo@hotmail.com",
                "Teste multiplos destinatários.",
                "Aqui corpo da mensagem",
                "",
                "", false, 1);
        }

        [TestMethod]
        public void CanGetProcedure()
        {
            using (var ctx = new FoneClubeContext())
            {
                //var teste = ctx.tblPersons.ToList();
                var teste = ctx.Database.SqlQuery<ClaroCobrancaResult>("CobFullClaro_API 8, 2019").ToList();
            }

        }


        [TestMethod]
        public void CanSendEmailBankBillet()
        {
            bool sendEmail = new ChargingAcess().SendBankBilletMail("https://bole.to/3/zymyd.pdf", "andrefelicio@live.com", "boleto.pdf");
            //var sendEmail = new Utils().SendEmail(Utils.EmailTo.Vendas, "Cobrança realizada.", string.Format(" Foi realizado uma cobrança no valor de {0} Reais para o cliente: {1}\n Id:{2} \n Observação:{3} \n Tipo de cobrança:{4} \n Agente:{4}", (Convert.ToDouble(1000) / 100), "Nome cliente", 20, "teste cobrança", "boleto", "cardozo"));
            //var sendEmail = new Utils().SendEmail(Utils.EmailTo.Vendas, "Cobrança realizada.", string.Format(" Foi realizado uma cobrança no valor de {0} Reais para o cliente id: {1}\n Observação:{2} \n Tipo de cobrança:{3} \n Agente:{4}", (Convert.ToDouble(1000) / 100), 1, "teste de comentário", "boleto", "cardozo"));
            Assert.IsTrue(sendEmail);
        }


        [TestMethod]
        public void GetIndicatorParentId()
        {
            //insert into tblReferred(intIdComission, intIdDad, intIdCurrent)
            //values(1, 3, 4)

            //insert into tblReferred(intIdComission, intIdDad, intIdCurrent)
            //values(2, 2, 4)

            //insert into tblReferred(intIdComission, intIdDad, intIdCurrent)
            //values(3, 1, 4)


        }

        [TestMethod]
        public void GetPersons()
        {


            var persons = new ProfileAccess().GetPersons();



        }

        [TestMethod]
        public void CanDeletePerson()
        {

            var deletePerson = new ProfileAccess().HardDeletePerson(new Person { DocumentNumber = "65887858605" });
            Assert.IsTrue((deletePerson == System.Net.HttpStatusCode.OK));

        }

        [TestMethod]
        [Ignore]
        public void InsertIndicators()
        {
            //var novaPessoa = new Person { Id = 6, IdParent = 4 };
            //Assert.IsTrue(new ProfileAccess().SavePersonIndicators(novaPessoa));

        }

        [TestMethod]
        public void GetCustomers()
        {
            var controller = new ProfileController();
            var clientes = controller.GetCustomers();
            Assert.IsTrue(clientes.Count > 0);
        }

        [TestMethod]
        public void GetHistory()
        {
            var controller = new ProfileController();
            var history = controller.GetChargeHistory(1);

        }

        [TestMethod]
        public void SaveChargingHistory()
        {
            var person = new Person
            {
                Id = 1,
                Charging = new Charging
                {
                    CollectorName = "cardozo",
                    CreationDate = DateTime.Now.ToString(),
                    Comment = "teste de comentario 3",
                    PaymentType = 1,
                    Ammount = "3150"
                }
            };

            var controller = new ProfileController();
            controller.SaveChargingHistory(person);


        }

        [TestMethod]
        public void InsertServiceOrder()
        {
            var person = new Person
            {
                Id = 1,
                ServiceOrder = new ServiceOrder
                {
                    AgentName = "Cardozo",
                    Description = "Teste de ordem de serviço",
                    PendingInteraction = false
                }
            };

            //var controller = new ProfileController();
            //controller.InsertServiceOrder(person);

            new ProfileAccess().InsertServiceOrder(person);


        }


        [TestMethod]
        public void CanUpdateLine()
        {
            using (var ctx = new FoneClubeContext())
            {
                var person = new Person { Phones = new List<Phone>(), Id = 1 };
                person.Phones.Add(new Phone { DDD = "21", Number = "990629796", IdPlanOption = 14 });


                new ProfileAccess().UpdatePersonPhones(person, ctx);
            }
        }

        [TestMethod]
        public void CanUpdateLineDirect()
        {
            using (var ctx = new FoneClubeContext())
            {
                var person = new Person { Phones = new List<Phone>(), Id = 1 };
                person.Phones.Add(new Phone { DDD = "21", Number = "990629784" });


                new ProfileAccess().UpdatePersonPhones(person);
            }
        }

        [TestMethod]
        public void CanUpdateLineDirectRest()
        {
            new ProfileAccess().SavePersonPhoneNumber(new Business.Commons.Entities.FoneClube.phone.PhoneViewModel
            {
                PersonId = 2,
                PlanId = 16,
                OperatorId = 1,
                DDNumber = 22,
                PhoneNumber = 994646992,
                NickName = "teste",
                IsActive = true,
                IsPhoneClube = true,
                IsPrecoVip = false
            });
        }

        [TestMethod]
        public void CanUpdateStatusLine()
        {
            new ProfileAccess().ActivationChangeStatus(1, false);
        }

        [TestMethod]
        public void TesteCharging()
        {
            using (var ctx = new FoneClubeContext())
            {
                var clients = new ChargingAcess().GetClients(2017, 10);
            }
        }

        [TestMethod]
        public void TesteExecProc()
        {
            using (var ctx = new FoneClubeContext())
            {
                var mes = new SqlParameter("@mes", 07);
                var ano = new SqlParameter("@ano", 2017);
                //var listaCobranca = ctx.Database.SqlQuery<CobrancaResult>("GetDadosCobrancaClaro @mes, @ano", mes, ano).ToList();
                var listaCobranca = ctx.Database.SqlQuery<CobrancaResult>("GetDadosCobrancaVivo @mes, @ano", mes, ano).ToList();

            }
        }

        public partial class CobrancaResult
        {

            public string txtConta { get; set; }
            public string txtReferenciaInicio { get; set; }
            public string txtTelefone { get; set; }
            public string txtNome { get; set; }
            public string txtNickname { get; set; }
            public string txtCPF { get; set; }

            public string txtPrecoUnico { get; set; }
            public string txtPrecoUnicoFracao { get; set; }
            public string txtValorCobranca { get; set; }
            public string txtIdPai { get; set; }

            //por enquanto deixar tudo string e convert pela API mais safe
            //public double? intPrecoUnico { get; set; }
            //public double? intPrecoUnicoFracao { get; set; }
            //public Int32? intValorCobranca { get; set; }
            //public Int32? intIdPai { get; set; }

        }


        [TestMethod]
        public void TesteSavePersonParent()
        {
            using (var ctx = new FoneClubeContext())
            {
                var clients = new ProfileAccess().SetCustomerParentPhone(new Person
                {
                    PhoneDDDParent = "",
                    PhoneNumberParent = "",
                    NameParent = "Simplicio da simplicidade simples",
                    Id = 1
                });

                Assert.IsTrue(clients);
            }
        }

        [TestMethod]
        public void TesteGetPersonParent()
        {
            using (var ctx = new FoneClubeContext())
            {
                var clients = new ProfileAccess().GetCustomerParent(new Person
                {
                    Id = 1
                });
                Assert.IsTrue(clients.NameParent.Length > 0);

            }
        }

        [TestMethod]
        public void TesteGetSearchPhones()
        {
            using (var ctx = new FoneClubeContext())
            {
                var clients = new ProfileAccess().GetNickNameResults("iva");


            }
        }

        [TestMethod]
        public void TestPagarme()
        {
            //https://api.pagar.me/1/customers/?count=10000&api_key=ak_live_fP7ceLSpdBe8gCXGTywVRmC5VTkvN0
            ApiGateway.EndPointApi = "https://api.pagar.me/1/customers/";
            var link = "?count=1000000&api_key=ak_live_fP7ceLSpdBe8gCXGTywVRmC5VTkvN0";


            using (var ctx = new FoneClubeContext())
            {
                var clients = ctx.tblPersons.Distinct().ToList();
                var clientsSemPagarme = ctx.tblPersons.Where(p => p.intIdPagarme == null).Distinct().ToList();


                var result = ApiGateway.GetConteudo(link);
                var customers = JsonConvert.DeserializeObject<JArray>(result);
                var pagarmeCustomers = new List<CustomerPagarme>();
                foreach (var customer in customers)
                {
                    var id = customer["id"].Value<int>();
                    var nome = customer["name"].Value<string>();
                    var documentNumber = customer["document_number"].Value<string>();

                    pagarmeCustomers.Add(new CustomerPagarme
                    {
                        ID = id,
                        Name = nome,
                        Document = documentNumber
                    });
                }

                var listIdsLivres = new List<int>();

                var adriano = pagarmeCustomers.FirstOrDefault(p => p.Name.ToLower().Contains("adriano"));

                var customerAdri = clients.FirstOrDefault(c => c.txtName.ToLower().Contains("adriano"));
                customerAdri.intIdPagarme = adriano.ID;
                ctx.SaveChanges();
                //foreach(var foneclubeCustomer in clients)
                //{
                //    foreach(var pagarmeCustomer in pagarmeCustomers)
                //    {
                //        try
                //        {
                //            if (pagarmeCustomer.Name.ToLower().Contains("adriano"))
                //            {
                //                var teste = pagarmeCustomer;
                //            }

                //            if (pagarmeCustomer.Document.Trim() == foneclubeCustomer.txtDocumentNumber.Trim())
                //            {



                //                var IdPagarme = pagarmeCustomer.ID;
                //                listIdsLivres.Add(pagarmeCustomer.ID);
                //                 //foneclubeCustomer.intIdPagarme = IdPagarme;
                //            }
                //        }
                //        catch (Exception)
                //        {

                //        }

                //    }
                //}
                //ctx.SaveChanges();
                //Assert.IsTrue(listIdsLivres.Count > 0);

            }
        }

        [TestMethod]
        public void CustomersPagarme()
        {
            ApiGateway.EndPointApi = "https://api.pagar.me/1/customers/";
            var link = "?count=1000000&api_key=ak_live_fP7ceLSpdBe8gCXGTywVRmC5VTkvN0";


            using (var ctx = new FoneClubeContext())
            {
                var clients = ctx.tblPersons.Distinct().ToList();
                var clientsSemPagarme = ctx.tblPersons.Where(p => p.intIdPagarme == null).Distinct().ToList();

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var result = ApiGateway.GetConteudo(link);
                var customers = JsonConvert.DeserializeObject<JArray>(result.Replace("object","objecta"));
                var pagarmeCustomers = new List<CustomerPagarme>();
                foreach (var customer in customers)
                {
                    var id = customer["id"].Value<int>();
                    var nome = customer["name"].Value<string>();
                    var documentNumber = customer["document_number"].Value<string>();

                    pagarmeCustomers.Add(new CustomerPagarme
                    {
                        ID = id,
                        Name = nome,
                        Document = documentNumber
                    });
                }

                var clientes = ctx.tblPersons.Select(a => a.txtDocumentNumber).ToList();

                //db.Questions.Where(q => !db.QuestionCounters.Any(qc => qc.QuestionsID == q.QuestionsID))

                var clientesSomentePagarme = pagarmeCustomers.Where(c => !clientes.Any(a => a == c.Document)).ToList();

                //RetornoCompleto(customers);
                //RetornoFracionado(clientesSomentePagarme);

            }
        }

        


        private void RetornoFracionado(List<CustomerPagarme> clientesSomentePagarme)
        {
            throw new NotImplementedException();
        }

        private void RetornoCompleto(JArray customers)
        {
            throw new NotImplementedException();
        }

        private class restoreAddres
        {
            public string street { get; set; }
            public string complementary { get; set; }
            public string street_number { get; set; }
            public string neighborhood { get; set; }
            public string city { get; set; }
            public string state { get; set; }
            public string zipcode { get; set; }
            public string country { get; set; }
            public int? id { get; set; }
            public string objecta { get; set; }
        }



        [TestMethod]
        public void TestPagarmeSegundo()
        {
            //https://api.pagar.me/1/customers/?count=10000&api_key=ak_live_fP7ceLSpdBe8gCXGTywVRmC5VTkvN0
            ApiGateway.EndPointApi = "https://api.pagar.me/1/customers/";
            var link = "?count=1000000&api_key=ak_live_fP7ceLSpdBe8gCXGTywVRmC5VTkvN0";


            using (var ctx = new FoneClubeContext())
            {
                var clients = ctx.tblPersons.Distinct().ToList();
                var fernanda = ctx.tblPersons.Where(p => p.intIdPerson == 12).Distinct().ToList();
                var clientsSemPagarme = ctx.tblPersons.Where(p => p.intIdPagarme == null).Distinct().ToList();


                var result = ApiGateway.GetConteudo(link);
                var customers = JsonConvert.DeserializeObject<JArray>(result);
                var pagarmeCustomers = new List<CustomerPagarme>();
                foreach (var customer in customers)
                {
                    var id = customer["id"].Value<int>();
                    var nome = customer["name"].Value<string>();
                    var documentNumber = customer["document_number"].Value<string>();

                    pagarmeCustomers.Add(new CustomerPagarme
                    {
                        ID = id,
                        Name = nome,
                        Document = documentNumber
                    });
                }

                var listIdsLivres = new List<int>();



                foreach (var pagarmeCustomer in pagarmeCustomers)
                {
                    try
                    {

                        foreach (var foneclubeCustomer in clientsSemPagarme)
                        {
                            if (foneclubeCustomer.txtDocumentNumber.Trim() == pagarmeCustomer.Document.Trim())
                            {
                                var teste = foneclubeCustomer;
                            }
                        }

                        //if (pagarmeCustomer.Document)
                        //{
                        //    var teste = pagarmeCustomer;
                        //}

                    }
                    catch (Exception)
                    {

                    }

                }


                //Assert.IsTrue(listIdsLivres.Count > 0);

            }
        }

        public class CustomerPagarme
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string Document { get; set; }
        }

        //EXEC CobFullClaro_Extract 2,2018		-- to extract Detailed report

        //[TestMethod]
        //public void TestsConsumeProcs()
        //{
        //    using (var ctx = new FoneClubeContext())
        //    {
        //        var teste = ctx.Database.SqlQuery<CobrancaClaroResult>("CobFullClaro_Extract @mes, @ano",
        //            new SqlParameter("@mes", 02),
        //            new SqlParameter("@ano", 2018)).ToList();

        //        var teste2 = ctx.Database.SqlQuery<CobrancaResult>("GetDadosCobrancaClaro @mes, @ano",
        //            new SqlParameter("@mes", 09),
        //            new SqlParameter("@ano", 2018)).ToList();



        //    }
        //}

        [TestMethod]
        public void TestsConsumeProcs()
        {
            using (var ctx = new FoneClubeContext())
            {
                //var listaReportVivo = ctx.CobFullVivo_Sum.Where(c => c.Report_Month == 2 && c.Report_Year == 2018).ToList();

                var listaReportClaro = ctx.CobFullClaro_Sum.Where(c => c.Report_Month == 2 && c.Report_Year == 2018).ToList();

                var dataGeral = (from t in ctx.CobFullClaro_Sum
                                 where t.Report_Month == 2 && t.Report_Year == 2018
                                 select new
                                 {
                                     Telefone = t.Telefone,
                                     Result = Convert.ToInt64(t.Resultado___linha)
                                 }).ToList();


                var dataVivo = (from t in ctx.CobFullVivo_Sum
                                where t.Report_Month == 2 && t.Report_Year == 2018
                                select new
                                {
                                    Telefone = t.Telefone.Replace(" ", ""),
                                    Result = Convert.ToInt64(t.Resultado___da_linha)
                                }).ToList();


                dataGeral.AddRange(dataVivo);
                //var registro = listaReportClaro.Where(l => l.Nome == "Marcio G Franco").ToList();

            }
        }

        //private List<object> TesteTipo()
        //{
        //    throw new NotImplementedException();
        //}

        public class CobrancaClaroResult
        {

            public string Conta { get; set; }
            public string Mês { get; set; }
            public string txtTelefone { get; set; }
            public string txtNome { get; set; }
            public string txtNickname { get; set; }
            public string txtCPF { get; set; }

            public string txtPrecoUnico { get; set; }
            public string txtPrecoUnicoFracao { get; set; }
            public string txtValorCobranca { get; set; }
            public string txtIdPai { get; set; }

            //por enquanto deixar tudo string e convert pela API mais safe
            //public double? intPrecoUnico { get; set; }
            //public double? intPrecoUnicoFracao { get; set; }
            //public Int32? intValorCobranca { get; set; }
            //public Int32? intIdPai { get; set; }
        }

        [TestMethod]
        public void CanDeletePersonSP()
        {
            using (var ctx = new FoneClubeContext())
            {
                var delete = Convert.ToBoolean(ctx.sp_DeletePerson(4272).FirstOrDefault());


            }
        }

        [TestMethod]
        public void CanGetHistoryPayments()
        {
            var history = new ProfileController().GetChargeAndServiceOrderHistoryDocument("90616693753");
            Assert.IsNotNull(history);
        }

        [TestMethod]
        public void CanSavePartialPerson()
        {
            List<Phone> phones = new List<Phone>();
            phones.Add(new Phone { DDD = "21", Number = "987486291" });

            var pessoa = new Person();
            pessoa.Adresses = new List<Adress>();
            pessoa.Adresses.Add(new Adress
            {
                Cep = "298500000",
                City = "teste",
                Country = "Noma cidade",
                State = "RJ",
                Street = "Rua teste",
                StreetNumber = "100"
            });

            var response = new ProfileAccess().SetPartialCustomer(new Person
            {
                DocumentNumber = "09624952652",
                Name = "11 Vera Lúcia Barreto Seixas",
                Email = "1ivisantos45@gmail.com",
                Phones = phones,
                //IdParent = 4168,
                Adresses = pessoa.Adresses
            });

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void CanSaveAssociationLog()
        {
            var response = new ProfileAccess().SetAssociationPerson(new Person { Id = 1 });
            var response2 = new ProfileAccess().SetDisasociationPerson(new Person { Id = 1 });

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void CanGetAPI_KEY()
        {
            var teste = ConfigurationManager.AppSettings["APIKEY"].Substring(0, 8);
        }

        [TestMethod]
        public void CanSetParent()
        {
            var teste = new ProfileAccess().SetPersonParent(3001, 1, new FoneClubeContext());
            Assert.IsTrue(teste);
        }

        [TestMethod]
        public void CanSyncCustomers()
        {
            var teste = new ProfileAccess().SyncCustomers();
        }

        [TestMethod]
        public void CanGetAllPersons()
        {
            var teste = new ProfileAccess().GetAllPersons(false);
        }

        [TestMethod]
        public void CanSetCustomerCross()
        {
            var statusResponse = new ProfileAccess().InsertNewCustomerCross(new Business.Commons.Entities.ViewModel.CustomerCrossRegisterViewModel
            {
                phone = "2133334444",
                documento = "10667103116",
                documentType = "CPF",
                email = "fc1@rodrigocardozo.com.br",
                name = "Cadastronildo santos teste form",
                password = "123qwe",
                confirmPassword = "123qwe",
                idPai = "2"
            });
        }

        [TestMethod]
        public void CanSetCustomerCrossParent()
        {
            var statusResponse = new ProfileAccess().InsertNewCustomerCross(new Business.Commons.Entities.ViewModel.CustomerCrossRegisterViewModel
            {
                phone = "2133334444",
                documento = "10667103235",
                documentType = "CPF",
                email = "teste312@teste313.com",
                name = "Cadastronildo santos",
                password = "123qwe",
                confirmPassword = "123qwe",
                idPai = "2"
            });
        }

        [TestMethod]
        public void CanGetNames()
        {
            var name = new ProfileAccess().GetName("Rodrigo Cardozo Pinto Teste Solto");
            Assert.IsTrue(name.Name == "Rodrigo");
            Assert.IsTrue(name.LastName.Trim() == "Cardozo Pinto Teste Solto");
            Assert.IsTrue(name.LastWord == "Solto");
        }


        [TestMethod]
        public void CanGetLinkRecomendation()
        {
            var id = 1205;
            var encodeId = new Utils().Base64Encode(id.ToString());
            var linkCadastro = "http://cadastro.foneclube.com.br/{0}";
            var formatedLink = string.Format(linkCadastro, encodeId);
        }

        [TestMethod]
        public void CanGetLinkRecomendationBlink()
        {
            var id = 1205;
            var encodeId = new Utils().Base64Encode(id.ToString());
            var linkCadastro = "http://cadastro.foneclube.com.br/{0}";
            var formatedLink = string.Format(linkCadastro, encodeId);
        }


        [TestMethod]
        public void CanGetSugestName()
        {
            var name = "rodrigo_cardozo";
            var previousName = "rodrigo_cardozo";

            var finalName = new ProfileAccess().GetMaskName(name, previousName);
            Assert.AreEqual(finalName, "rodrigo_cardozo1");
        }

        [TestMethod]
        public void CanGetSugestName1()
        {
            var name = "rodrigo_cardozo";
            var previousName = "rodrigo_cardozo1";

            var finalName = new ProfileAccess().GetMaskName(name, previousName);
            Assert.AreEqual(finalName, "rodrigo_cardozo2");
        }


        [TestMethod]
        public void CanSetAllCustomerLinks()
        {
            var statusResponse = new ProfileAccess().SetAllCustomersLinks();
        }

        [TestMethod]
        public void CanSaveProfileLoja()
        {
            var teste = "{ \"id\": 1, \"date_created\": \"2020-02-25T17:47:45\", \"date_created_gmt\": \"2020-02-25T17:47:45\", \"date_modified\": \"2020-02-29T05:35:29\", \"date_modified_gmt\": \"2020-02-29T05:35:29\", \"email\": \"marcio.franco@foneclube.com.br\", \"first_name\": \"Marcio\", \"last_name\": \"Franco\", \"role\": \"administrator\", \"username\": \"marciofranco\", \"billing\": { \"first_name\": \"Marcio\", \"last_name\": \"Franco\", \"company\": \"\", \"address_1\": \"Avenida das Américas\", \"address_2\": \"1302 bloco - 2\", \"city\": \"Rio de Janeiro\", \"postcode\": \"22793-081\", \"country\": \"BR\", \"state\": \"RJ\", \"email\": \"marcio.franco@gmail.com\", \"phone\": \"(21) 98200-8200\", \"number\": \"7837\", \"neighborhood\": \"Barra da Tijuca\", \"persontype\": \"F\", \"cpf\": \"90616693753\", \"rg\": \"\", \"cnpj\": \"\", \"ie\": \"\", \"birthdate\": \"\", \"sex\": \"\", \"cellphone\": \"(21) 9820-0820\" } }";
            var updateLoja = new ProfileAccess().SavePersonLojaRegister(JsonConvert.DeserializeObject<CustomerWoocommerce>(teste));
        }

        [TestMethod]
        public void CanUpdateProfileLoja()
        {
            var teste = "{\"id\":1,\"date_created\":\"2020-02-25T17:47:45\",\"date_created_gmt\":\"2020-02-25T17:47:45\",\"date_modified\":\"2020-02-29T14:59:13\",\"date_modified_gmt\":\"2020-02-29T14:59:13\",\"email\":\"marcio.franco@foneclube.com.br\",\"first_name\":\"Marcio\",\"last_name\":\"Franco\",\"role\":\"administrator\",\"username\":\"marciofranco\",\"billing\":{\"first_name\":\"Marcio\",\"last_name\":\"Franco\",\"company\":\"\",\"address_1\":\"Avenida das Am\\u00e9ricas\",\"address_2\":\"1302 bloco 2\",\"city\":\"Rio de Janeiro\",\"postcode\":\"22793-081\",\"country\":\"BR\",\"state\":\"RJ\",\"email\":\"marcio.franco@gmail.com\",\"phone\":\"(21) 98200-8200\",\"number\":\"7837\",\"neighborhood\":\"Barra da Tijuca\",\"persontype\":\"F\",\"cpf\":\"90616693753\",\"rg\":\"\",\"cnpj\":\"\",\"ie\":\"\",\"birthdate\":\"\",\"sex\":\"\",\"cellphone\":\"(21) 9820-08200\"},\"shipping\":{\"first_name\":\"\",\"last_name\":\"\",\"company\":\"\",\"address_1\":\"\",\"address_2\":\"\",\"city\":\"\",\"postcode\":\"\",\"country\":\"\",\"state\":\"\",\"number\":\"\",\"neighborhood\":\"\"},\"is_paying_customer\":false,\"avatar_url\":\"https:\\/\\/secure.gravatar.com\\/avatar\\/396bed965ffeb8a2c4fe542f9efb9cfd?s=96&d=mm&r=g\",\"meta_data\":[{\"id\":18,\"key\":\"community-events-location\",\"value\":{\"ip\":\"177.47.104.0\"}},{\"id\":21,\"key\":\"jetpack_tracks_anon_id\",\"value\":\"jetpack:a2Or44gJ5ljNs+rA6jRkZ+Yf\"},{\"id\":22,\"key\":\"jetpack_tracks_wpcom_id\",\"value\":\"180715519\"},{\"id\":23,\"key\":\"dismissed_template_files_notice\",\"value\":\"1\"},{\"id\":24,\"key\":\"wc_last_active\",\"value\":\"1582934400\"},{\"id\":33,\"key\":\"dismissed_no_secure_connection_notice\",\"value\":\"1\"},{\"id\":78,\"key\":\"shipping_method\",\"value\":\"\"},{\"id\":79,\"key\":\"billing_persontype\",\"value\":\"1\"},{\"id\":80,\"key\":\"billing_cpf\",\"value\":\"906.166.937-53\"},{\"id\":81,\"key\":\"billing_cnpj\",\"value\":\"\"},{\"id\":82,\"key\":\"billing_ie\",\"value\":\"\"},{\"id\":83,\"key\":\"billing_number\",\"value\":\"7837\"},{\"id\":84,\"key\":\"billing_neighborhood\",\"value\":\"Barra da Tijuca\"},{\"id\":85,\"key\":\"billing_cellphone\",\"value\":\"(21) 9820-08200\"},{\"id\":242,\"key\":\"account_status\",\"value\":\"approved\"},{\"id\":243,\"key\":\"um_member_directory_data\",\"value\":{\"account_status\":\"approved\",\"hide_in_members\":false,\"profile_photo\":false,\"cover_photo\":false,\"verified\":false}},{\"id\":254,\"key\":\"managenav-menuscolumnshidden\",\"value\":[\"link-target\",\"css-classes\",\"xfn\",\"description\",\"title-attribute\"]},{\"id\":262,\"key\":\"shipping_number\",\"value\":\"\"},{\"id\":264,\"key\":\"shipping_neighborhood\",\"value\":\"\"}],\"_links\":{\"self\":[{\"href\":\"https:\\/\\/hloja.foneclube.com.br\\/wp-json\\/wc\\/v3\\/customers\\/1\"}],\"collection\":[{\"href\":\"https:\\/\\/hloja.foneclube.com.br\\/wp-json\\/wc\\/v3\\/customers\"}]}}";
            var updateLoja = new ProfileAccess().UpdatePersonLojaRegister(JsonConvert.DeserializeObject<CustomerWoocommerce>(teste));
        }

        [TestMethod]
        public void CanCheckoutLojaHook()
        {
            var teste = "{\"id\":3547,\"parent_id\":0,\"number\":\"3547\",\"order_key\":\"wc_order_MbcUjoMizbCCD\",\"created_via\":\"checkout\",\"version\":\"3.9.3\",\"status\":\"pending\",\"currency\":\"BRL\",\"date_created\":\"2020-03-05T18:54:50\",\"date_created_gmt\":\"2020-03-05T21:54:50\",\"date_modified\":\"2020-03-05T18:54:50\",\"date_modified_gmt\":\"2020-03-05T21:54:50\",\"discount_total\":\"0.00\",\"discount_tax\":\"0.00\",\"shipping_total\":\"0.00\",\"shipping_tax\":\"0.00\",\"cart_tax\":\"0.00\",\"total\":\"1.00\",\"total_tax\":\"0.00\",\"prices_include_tax\":false,\"customer_id\":1,\"customer_ip_address\":\"177.154.3.127\",\"customer_user_agent\":\"7.36\",\"customer_note\":\"\",\"billing\":{\"first_name\":\"Marcio\",\"last_name\":\"Franco\",\"company\":\"\",\"address_1\":\"Avenida das Am\u00e9ricas\",\"address_2\":\"1302 bloco 2\",\"city\":\"Rio de Janeiro\",\"state\":\"RJ\",\"postcode\":\"22793-081\",\"country\":\"BR\",\"email\":\"marcio.franco@gmail.com\",\"phone\":\"(21) 2131-1804\",\"number\":\"7837\",\"neighborhood\":\"Barra da Tijuca\",\"persontype\":\"F\",\"cpf\":\"90616693753\",\"rg\":\"52619913\",\"cnpj\":\"\",\"ie\":\"\",\"birthdate\":\"\",\"sex\":\"\",\"cellphone\":\"(21) 98200-8200\"},\"shipping\":{\"first_name\":\"\",\"last_name\":\"\",\"company\":\"\",\"address_1\":\"\",\"address_2\":\"\",\"city\":\"\",\"state\":\"\",\"postcode\":\"\",\"country\":\"\",\"number\":\"\",\"neighborhood\":\"\"},\"payment_method\":\"loja5_woo_bradesco_api_boleto\",\"payment_method_title\":\"Boleto Bancario\",\"transaction_id\":\"\",\"date_paid\":null,\"date_paid_gmt\":null,\"date_completed\":null,\"date_completed_gmt\":null,\"cart_hash\":\"dc6b7bb84f564aba7e88aa2f687ec123\",\"meta_data\":[{\"id\":4380,\"key\":\"_billing_cpf\",\"value\":\"906.166.937-53\"},{\"id\":4381,\"key\":\"_billing_rg\",\"value\":\"52619913\"},{\"id\":4382,\"key\":\"_billing_number\",\"value\":\"7837\"},{\"id\":4383,\"key\":\"_billing_neighborhood\",\"value\":\"Barra da Tijuca\"},{\"id\":4384,\"key\":\"_billing_cellphone\",\"value\":\"(21) 98200-8200\"},{\"id\":4385,\"key\":\"is_vat_exempt\",\"value\":\"no\"},{\"id\":4386,\"key\":\"loja5_woo_bradesco_api_boleto_dados\",\"value\":{\"nosso_numero\":\"00000003547\",\"numero_documento\":3547,\"data_vencimento\":\"09//03//2020\",\"data_documento\":\"05//03//2020\",\"link_boleto\":\"\",\"linha_digitavel\":\"23793378236000000035047000501800781890000000100\",\"token\":\"djRDTzR2eGloT0JYZlFLell0QW55RmdCaTJuWUhwWnZLOHo1Nkg4S3pzMD0.\"}}],\"line_items\":[{\"id\":77,\"name\":\"Financeiro\",\"product_id\":22,\"variation_id\":0,\"quantity\":1,\"tax_class\":\"\",\"subtotal\":\"1.00\",\"subtotal_tax\":\"0.00\",\"total\":\"1.00\",\"total_tax\":\"0.00\",\"taxes\":[],\"meta_data\":[],\"sku\":\"\",\"price\":1}],\"tax_lines\":[],\"shipping_lines\":[],\"fee_lines\":[],\"coupon_lines\":[],\"refunds\":[],\"correios_tracking_code\":\"\",\"_links\":{\"self\":[{\"href\":\"\"}],\"collection\":[{\"href\":\"\"}],\"customer\":[{}]}}";
            var teste2 = "{\"id\":409,\"parent_id\":0,\"number\":\"409\",\"order_key\":\"wc_order_nxUu7XpQZKH4g\",\"created_via\":\"checkout\",\"version\":\"3.9.3\",\"status\":\"pending\",\"currency\":\"BRL\",\"date_created\":\"2020-03-10T19:19:27\",\"date_created_gmt\":\"2020-03-10T22:19:27\",\"date_modified\":\"2020-03-10T19:19:27\",\"date_modified_gmt\":\"2020-03-10T22:19:27\",\"discount_total\":\"0.00\",\"discount_tax\":\"0.00\",\"shipping_total\":\"0.00\",\"shipping_tax\":\"0.00\",\"cart_tax\":\"0.00\",\"total\":\"2.00\",\"total_tax\":\"0.00\",\"prices_include_tax\":false,\"customer_id\":1,\"customer_ip_address\":\"177.154.3.127\",\"customer_user_agent\":\"M37.36\",\"customer_note\":\"\",\"billing\":{\"first_name\":\"Marcio\",\"last_name\":\"Franco\",\"company\":\"\",\"address_1\":\"Avenida das Am\u00e9ricas\",\"address_2\":\"1302 bloco 2\",\"city\":\"Rio de Janeiro\",\"state\":\"RJ\",\"postcode\":\"22793-081\",\"country\":\"BR\",\"email\":\"marcio.franco@gmail.com\",\"phone\":\"(21) 98200-8200\",\"number\":\"7837\",\"neighborhood\":\"Barra da Tijuca\",\"persontype\":\"F\",\"cpf\":\"90616693753\",\"rg\":\"\",\"cnpj\":\"\",\"ie\":\"\",\"birthdate\":\"\",\"sex\":\"\",\"cellphone\":\"(21) 98200-8200\"},\"shipping\":{\"first_name\":\"\",\"last_name\":\"\",\"company\":\"\",\"address_1\":\"\",\"address_2\":\"\",\"city\":\"\",\"state\":\"\",\"postcode\":\"\",\"country\":\"\",\"number\":\"\",\"neighborhood\":\"\"},\"payment_method\":\"loja5_woo_bradesco_api_boleto\",\"payment_method_title\":\"Boleto Bancario\",\"transaction_id\":\"\",\"date_paid\":null,\"date_paid_gmt\":null,\"date_completed\":null,\"date_completed_gmt\":null,\"cart_hash\":\"13132ccfc312f59e494e7d250e77e471\",\"meta_data\":[{\"id\":3714,\"key\":\"_billing_persontype\",\"value\":\"1\"},{\"id\":3715,\"key\":\"_billing_cpf\",\"value\":\"906.166.937-53\"},{\"id\":3716,\"key\":\"_billing_cnpj\",\"value\":\"\"},{\"id\":3717,\"key\":\"_billing_ie\",\"value\":\"\"},{\"id\":3718,\"key\":\"_billing_number\",\"value\":\"7837\"},{\"id\":3719,\"key\":\"_billing_neighborhood\",\"value\":\"Barra da Tijuca\"},{\"id\":3720,\"key\":\"_billing_cellphone\",\"value\":\"(21) 98200-8200\"},{\"id\":3721,\"key\":\"is_vat_exempt\",\"value\":\"no\"},{\"id\":3722,\"key\":\"loja5_woo_bradesco_api_boleto_dados\",\"value\":{\"nosso_numero\":\"00000000409\",\"numero_documento\":409,\"data_vencimento\":\"\",\"link_boleto\":\"\",\"linha_digitavel\":\"23793378236000000004609000501800481940000000200\",\"token\":\"WTMzOSswUVBiSVlWc3BVUnR5YVYrcWJmdklwRm9EUlc3SmtzWDF5dTRnUT0.\"}}],\"line_items\":[{\"id\":72,\"name\":\"Financeiro\",\"product_id\":22,\"variation_id\":0,\"quantity\":2,\"tax_class\":\"\",\"subtotal\":\"2.00\",\"subtotal_tax\":\"0.00\",\"total\":\"2.00\",\"total_tax\":\"0.00\",\"taxes\":[],\"meta_data\":[],\"sku\":\"\",\"price\":1}],\"tax_lines\":[],\"shipping_lines\":[],\"fee_lines\":[],\"coupon_lines\":[],\"refunds\":[],\"correios_tracking_code\":\"\",\"currency_symbol\":\"R$\",\"_links\":{\"self\":[{\"href\":\"\"}],\"collection\":[{\"href\":\"\"}],\"customer\":[{\"href\":\"\"}]}}";
            var updateLoja = new ProfileAccess().CheckoutPersonLojaRegister(JsonConvert.DeserializeObject<Order>(teste2));
        }

        [TestMethod]
        public void CanUpdateCheckoutLojaHook()
        {
            var teste = "{\"id\":507,\"parent_id\":0,\"number\":\"504\",\"order_key\":\"wc_order_x78WYUwDqHczz\",\"created_via\":\"checkout\",\"version\":\"3.9.3\",\"status\":\"pending\",\"currency\":\"BRL\",\"date_created\":\"2020-03-11T22:03:53\",\"date_created_gmt\":\"2020-03-12T01:03:53\",\"date_modified\":\"2020-03-11T22:03:56\",\"date_modified_gmt\":\"2020-03-12T01:03:56\",\"discount_total\":\"0.00\",\"discount_tax\":\"0.00\",\"shipping_total\":\"0.00\",\"shipping_tax\":\"0.00\",\"cart_tax\":\"0.00\",\"total\":\"1.00\",\"total_tax\":\"0.00\",\"prices_include_tax\":false,\"customer_id\":4,\"customer_ip_address\":\"170.79.98.192\",\"customer_user_agent\":\"MozileWebK37.36\",\"customer_note\":\"\",\"billing\":{\"first_name\":\"RODRIGO\",\"last_name\":\"PINTO\",\"company\":\"Casa\",\"address_1\":\"Rua Enio Augusto De Mello 170\",\"address_2\":\"170\",\"city\":\"Rio Bonito\",\"state\":\"RJ\",\"postcode\":\"28800-000\",\"country\":\"BR\",\"email\":\"rodrigocardozop@gmail.com\",\"phone\":\"(21) 99464-6991\",\"number\":\"170\",\"neighborhood\":\"cidde nova\",\"persontype\":\"F\",\"cpf\":\"10667103767\",\"rg\":\"\",\"cnpj\":\"10667103767\",\"ie\":\"PINTO\",\"birthdate\":\"\",\"sex\":\"\",\"cellphone\":\"(21) 2734-3110\"},\"shipping\":{\"first_name\":\"\",\"last_name\":\"\",\"company\":\"\",\"address_1\":\"\",\"address_2\":\"\",\"city\":\"\",\"state\":\"\",\"postcode\":\"\",\"country\":\"\",\"number\":\"\",\"neighborhood\":\"\"},\"payment_method\":\"pagarme-credit-card\",\"payment_method_title\":\"Cart\u00e3o de Credito Pagar.me\",\"transaction_id\":\"171411579\",\"date_paid\":\"2020-03-11T22:03:56\",\"date_paid_gmt\":\"2020-03-12T01:03:56\",\"date_completed\":\"2020-03-11T22:03:56\",\"date_completed_gmt\":\"2020-03-12T01:03:56\",\"cart_hash\":\"dc6b7bb84f564aba7e88aa2f687ec123\",\"meta_data\":[{\"id\":4861,\"key\":\"_billing_persontype\",\"value\":\"1\"},{\"id\":4862,\"key\":\"_billing_cpf\",\"value\":\"106.671.037-67\"},{\"id\":4863,\"key\":\"_billing_cnpj\",\"value\":\"\"},{\"id\":4864,\"key\":\"_billing_ie\",\"value\":\"PINTO\"},{\"id\":4865,\"key\":\"_billing_number\",\"value\":\"170\"},{\"id\":4866,\"key\":\"_billing_neighborhood\",\"value\":\"cidde nova\"},{\"id\":4867,\"key\":\"_billing_cellphone\",\"value\":\"(21) 2734-3110\"},{\"id\":4868,\"key\":\"is_vat_exempt\",\"value\":\"no\"},{\"id\":4869,\"key\":\"Link do boleto banc\u00e1rio\",\"value\":\"\"},{\"id\":4870,\"key\":\"Cart\u00e3o de cr\u00e9dito\",\"value\":\"MasterCard\"},{\"id\":4871,\"key\":\"Parcelas\",\"value\":\"1\"},{\"id\":4872,\"key\":\"Total pago\",\"value\":\"1,00\"},{\"id\":4873,\"key\":\"Pontua\u00e7\u00e3o no Antifraude\",\"value\":\"\"},{\"id\":4874,\"key\":\"_wc_pagarme_transaction_data\",\"value\":{\"payment_method\":\"credit_card\",\"installments\":\"1\",\"card_brand\":\"MasterCard\",\"antifraud_score\":\"\",\"boleto_url\":\"\"}},{\"id\":4875,\"key\":\"_wc_pagarme_transaction_id\",\"value\":\"171411579\"}],\"line_items\":[{\"id\":108,\"name\":\"Financeiro\",\"product_id\":22,\"variation_id\":0,\"quantity\":1,\"tax_class\":\"\",\"subtotal\":\"1.00\",\"subtotal_tax\":\"0.00\",\"total\":\"1.00\",\"total_tax\":\"0.00\",\"taxes\":[],\"meta_data\":[],\"sku\":\"\",\"price\":1}],\"tax_lines\":[],\"shipping_lines\":[],\"fee_lines\":[],\"coupon_lines\":[],\"refunds\":[],\"_links\":{\"self\":[{\"href\":\"\"}],\"collection\":[{\"href\":\"\"}],\"customer\":[{\"href\":\"\"}]}}";
            var updateLoja = new ProfileAccess().CheckoutPersonLojaRegister(JsonConvert.DeserializeObject<Order>(teste));
        }

        [TestMethod]
        public void CanUpdateCheckoutLojaHookStatus()
        {
            var teste = "{\"action\":\"woocommerce_payment_complete\",\"arg\":507}";
            var updateLoja = new ChargingAcess().UpdateChargingHistoryLojaComplete(JsonConvert.DeserializeObject<StatusPagamento>(teste));
        }

        [TestMethod]
        public void GetLastCustomers()
        {
            using (var ctx = new FoneClubeContext())
            {
                var teste = ctx.tblPersons
                    .Any(p => p.txtDocumentNumber.Trim() == "14445613700");
            }
        }

        [TestMethod]
        public void GettesteRegistroAddres2()
        {
            var updateLoja = new ProfileAccess()
                .SavePersonAddressFC(
                new Business.Commons.Entities.ViewModel.CustomerAddressViewModel
                {
                    bairro = "centro",
                    cep = "20209202",
                    cidade = "niteroi",
                    complemento = "",
                    documento = "00091817187",
                    estado = "RJ",
                    idCliente = 6123,
                    numero = 100,
                    rua = "rua centro"
                });
        }

        [TestMethod]
        public void InsereSenhas()
        {
            using (var ctx = new FoneClubeContext())
            {
                //var clientes = ctx.tblPersons.Where(p => p.txtPassword == null).ToList();
                var clientes = ctx.tblPersons.Where(p => p.txtDocumentNumber == "10667103767").ToList();


                foreach (var cliente in clientes)
                {
                    var primeirosDigitos = cliente.txtDocumentNumber.Substring(0, 5);
                    var ultimosDigitos = cliente.txtDocumentNumber.Substring(cliente.txtDocumentNumber.Length - 1);
                    var hashedPassword = new Security().EncryptPassword("111111");

                    cliente.txtPassword = hashedPassword.Password;
                }

                ctx.SaveChanges();
            }
        }

        [TestMethod]
        public void InsereSenhasTelefonesContato()
        {
            using (var ctx = new FoneClubeContext())
            {
                //var clientes = ctx.tblPersons.Where(p => p.txtPassword == null).ToList();
                var contatos = ctx.tblPersonsPhones.Where(p => p.bitPhoneClube == false).ToList();
                var clienteIds = contatos.Select(a => a.intIdPerson).ToList();
                var clientes = ctx.tblPersons.Where(p => clienteIds.Contains(p.intIdPerson)).ToList();
                //var clientes = ctx.tblPersons.Where(p => p.intIdPerson == 4307).ToList();



                foreach (var cliente in clientes)
                {
                    var contato = contatos.FirstOrDefault(a => a.intIdPerson == cliente.intIdPerson);
                    var numeroContato = contato.intDDD.ToString() + contato.intPhone.ToString();
                    var hashedPassword = new Security().EncryptPassword(numeroContato);

                    cliente.txtPassword = hashedPassword.Password;

                    ctx.SaveChanges();
                }

                ctx.SaveChanges();
            }
        }

        [TestMethod]
        public void InsereSenhaTelefonesContato()
        {
            using (var ctx = new FoneClubeContext())
            {
                var clientes = ctx.tblPersons.Where(p => p.intIdPerson == 4957).ToList();

                foreach (var cliente in clientes)
                {
                    var hashedPassword = new Security().EncryptPassword("21995906164");
                    cliente.txtPassword = hashedPassword.Password;
                }

                ctx.SaveChanges();
            }
        }

        [TestMethod]
        public void AtualizaLinks()
        {
            using (var ctx = new FoneClubeContext())
            {

                var links = ctx.tblPersosAffiliateLinks.ToList();

                foreach (var link in links)
                {
                    var updateLink = link.txtOriginalLink.Replace("http://cadastro.foneclube.com.br/", "https://foneclube.com.br/convite/");
                    link.txtOriginalLink = updateLink;
                    link.txtBlinkLink = updateLink;
                }

                ctx.SaveChanges();
            }
        }

        [TestMethod]
        public void CanGetStatusCadastroFeito()
        {
            var updateLoja = new ProfileAccess().GetCadastrosRealizados(6185);
        }

        [TestMethod]
        public void PopulaDadosPendentes()
        {
            using (var ctx = new FoneClubeContext())
            {
                var todasPessoas = ctx.tblPersons.ToList();
                var pessoasDadosPessoaisPendentes = ctx.tblPersons.Where(p => p.txtEmail.Contains("@pendente.com")).ToList();

                foreach (var pessoa in todasPessoas)
                {
                    var dadosPendentes = pessoasDadosPessoaisPendentes.FirstOrDefault(p => p.intIdPerson == pessoa.intIdPerson);

                    if (dadosPendentes != null)
                        pessoa.bitDadosPessoaisCadastrados = false;
                    else
                        pessoa.bitDadosPessoaisCadastrados = true;

                    pessoa.bitSenhaCadastrada = false;
                }

                ctx.SaveChanges();
            }
        }

        [TestMethod]
        public void CanTestCustomersLog()
        {
            var teste = new PhoneAccess().GetRegisteredPersonsLog();
        }

        [TestMethod]
        public void CanPlanosCliente()
        {
            var matricula = 1;
            var teste = new PhoneAccess().GetPlanosCliente(matricula);
        }

        [TestMethod]
        public void CanGetPlans()
        {
            using (var ctx = new FoneClubeContext())
            {
                var teste = "27,31,27";
                var result = GetCheckoutPlans(teste);



            }
        }

        public List<Plan> GetCheckoutPlans(string planos)
        {
            using (var ctx = new FoneClubeContext())
            {
                var checkoutPlans = new List<Plan>();
                var plansParams = planos.Split(',');
                var plansOptions = ctx.tblPlansOptions.ToList();

                foreach (var option in plansParams)
                {
                    var planOption = Convert.ToInt32(option);
                    var current = plansOptions.FirstOrDefault(p => p.intIdPlan == planOption);
                    if (current != null)
                    {
                        checkoutPlans.Add(new Plan { Id = planOption, Description = current.txtDescription});
                    }
                }

                return checkoutPlans;
            }
        }

        [TestMethod]
        public void CanTestPlanoAmigos()
        {
            using (var ctx = new FoneClubeContext())
            {
                var matricula = 1;

                var filhos = new List<Person>();

                var filhosIds = ctx.tblPersonsParents
                                   .Where(p => p.intIdParent == matricula)
                                   .Select(a => a.intIdSon.Value)
                                   .ToList();

                //depois vem bitAmigos
                var telefones = ctx.tblPersonsPhones
                    .Where(p => filhosIds.Contains(p.intIdPerson.Value) && p.bitPhoneClube == true && p.bitAtivo == true)
                    .ToList();

                var filhosComTelefonesFonclubeAtivos = telefones.Select(a => a.intIdPerson.Value).Distinct().ToList();

                foreach (var id in filhosComTelefonesFonclubeAtivos)
                {
                    var person = new Person
                    {
                        Id = id,
                        Phones = new List<Phone>()
                    };

                    var filhoPhones = telefones.Where(t => t.intIdPerson == id).ToList();

                    foreach (var telefone in filhoPhones)
                    {
                        person.Phones.Add(new Phone
                        {
                            Id = telefone.intId,
                            DDD = telefone.intDDD.ToString(),
                            Number = telefone.intPhone.ToString()
                        });
                    }

                    var historico = ctx.GetHistoricoPagamento(id).FirstOrDefault();
                    if (historico != null)
                    {
                        person.LastChargePaid = historico.dteCreate;
                        //remove da lista os que não tem pagamento feito ou coloca pra adicionar aqui
                        filhos.Add(person);
                    }

                }


                var linhasAmigos = ctx.tblPersonsPhones.Where(p => p.bitAmigos == true);

                

                // desconto de 50%  para linha que tem duas linhas a baixo.
                // desconto de 100% para linha que tem duas linhas a baixo.
                // se tenho pelo menos dois clientes a baixo
                // se pelo menos dois pagaram linha
                //20 dias sem cortar beneficios
            }
        }



        [TestMethod]
        public void CanGetRecover()
        {
            var updateLoja = new ProfileAccess().GetRecoverPassword("10667103767");
        }

        [TestMethod]
        public void CanGetReset()
        {
            var updateLoja = new ProfileAccess().GetMensagemTrocaSenha("834C7643-2472-400C-B49F-BD6EBFBFD762");
        }

        [TestMethod]
        public void GetAllCustomers()
        {
            var updateLoja = new ProfileAccess().GetAllCustomers();
        }
       


    }


}
