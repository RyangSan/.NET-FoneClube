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
using FoneClube.BoletoSimples.APIs.BankBillets.Models;
using FoneClube.BoletoSimples.Common;
using System.Threading.Tasks;
using FoneClube.Business.Commons.Entities.Cielo;
using FoneClube.Business.Commons.Utils;
using System.Configuration;
using FoneClube.Business.Commons.Entities.ViewModel.Plano;

namespace FoneClube.WebAPI.Tests.Controllers
{


    [TestClass]
    public class ChargingControllerTest
    {
        [TestMethod]
        public void CanCreateBoleto()
        {

            var controller = new ChargingController();


            //TODO
            //criar caso de insert perfeito
            //depois quebrar em cada caso de validação pra testar remoção de nova pessoa  após a quebra em casa parte

            //var rest = controller.ChargeClient(new Charging());

            //Assert.AreEqual(rest, 1);


        }



        [TestMethod]
        public void CanMonthChargings()
        {

            var controller = new ChargingController();
            var result = controller.GetMonthChargings(1, 2019);

            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void CanValidityChargings()
        {

            var controller = new ChargingController();
            var result = controller.GetStatusVingencia(1, 2019);

            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void BasicPost()
        {
            //se ficar dando erro criar outro objs
            var addresses = new List<Adress>();
            addresses.Add(new Adress
            {
                Street = "Rua da Glória",
                Neighborhood = "Rua da Glória",
                State = "RJ",
                City = "Rio de Janeiro",
                Cep = "22420006",
                Complement = "Centro",
                StreetNumber = "100"
            });

            var person = new Person
            {
                DocumentNumber = "16214852500",
                Name = "Joao silva",
                Email = "rodrigocardozop@gmail.com",
                Adresses = addresses
            };
            var charging = new Charging { Ammount = "200" };


            var task = StartAssyncBillet(person, charging).Result;
            Assert.IsTrue(task.BoletoId.ToString().Length > 0);
        }

        private async Task<Charging> StartAssyncBillet(Person person, Charging charging)
        {
            return await new ChargingAcess().CreateBankBilletDirect(person, charging);
        }

        [TestMethod]
        public void CanSendEmailCard()
        {
            var planos = new PhoneAccess().GetPlanosCliente(1);
            var complemento = new PhoneAccess().GetCorpoPlanos(planos);

            var post = new EmailAccess().SendEmail(new Email
            {
                To = "rodrigocardozop@gmail.com",
                TargetName = "Rodrigo",
                TargetTextBlue = "19,91",
                TemplateType = Convert.ToInt32(Email.TemplateTypes.CardCharged),
                TargetSecondaryText = complemento
            });

            Assert.IsTrue(post);

        }

        [TestMethod]
        public void CanSendEmailCardComPlanos()
        {            
            var planos = new PhoneAccess().GetPlanosCliente(1);
            var complemento = new PhoneAccess().GetCorpoPlanos(planos);

            var post = new EmailAccess().SendEmail(new Email
            {
                To = "rodrigocardozop@gmail.com",
                TargetName = "Rodrigo teste",
                TargetTextBlue = "19,99",
                TemplateType = Convert.ToInt32(Email.TemplateTypes.CardCharged),
                TargetSecondaryText = complemento
            });

            Assert.IsTrue(post);
        }

        //24
        [TestMethod]
        public void CanSendBoasVindas()
        {

            var post = new EmailAccess().SendEmail(new Email
            {
                To = "rodrigocardozop@gmail.com",
                TargetName = "Rodrigo",
                TargetTextBlue = string.Empty,
                TemplateType = 24,
                TargetSecondaryText = String.Empty
            });

            Assert.IsTrue(post);

        }

        [TestMethod]
        public void CanSendEmailCardWithDiscount()
        {

            var post = new EmailAccess().SendEmail(new Email
            {
                To = "@gmail.com",
                TargetName = "Rodrigo",
                TargetTextBlue = "19,91",
                TemplateType = Convert.ToInt32(Email.TemplateTypes.CardCharged),
                TargetSecondaryText = "Prezado cliente essa mensagem foi escrita no client side",
                DiscountPrice = "10,00"
            });

            Assert.IsTrue(post);

        }

        [TestMethod]
        public void CanSendEmailBoleto()
        {

            var post = new EmailAccess().SendEmail(new Email
            {
                To = "rodrigocardozop@gmail.com",
                TargetName = "Rodrigo",
                //TargetTextBlue = @"https://docs.pagar.me/",
                TargetSecondaryText = @"https://docs.pagar.me/" + "\n Prezado cliente essa mensagem foi escrita no client side",
                TemplateType = Convert.ToInt32(Email.TemplateTypes.BoletoCharged)
            });

            Assert.IsTrue(post);
        }

        [TestMethod]
        public void CanSendEmailBoletoWithoutDetail()
        {

            var post = new EmailAccess().SendEmail(new Email
            {
                To = "rodrigocardozop@gmail.com",
                TargetName = "Rodrigo",
                //TargetTextBlue = @"https://docs.pagar.me/",
                TargetSecondaryText = @"https://docs.pagar.me/",
                TemplateType = Convert.ToInt32(Email.TemplateTypes.BoletoCharged)
            });

            Assert.IsTrue(post);
        }

        [TestMethod]
        public void CanSendEmailPIXWithoutDetail()
        {

            var post = new EmailAccess().SendEmail(new Email
            {
                To = "rodrigocardozop@gmail.com",
                TargetName = "Rodrigo",
                TargetTextBlue = ConfigurationManager.AppSettings["qrcodelink"] + 4557,
                TargetSecondaryText = "Valor total: testeR$",
                TemplateType = Convert.ToInt32(Email.TemplateTypes.Pix)
            });

            Assert.IsTrue(post);
        }
        //GetQRCodeImage(int chargingId)
        [TestMethod]
        public void CanGetQrCode()
        {

            var teste = new TransactionAccess().GetQRCodeImage(4526);
        }

        [TestMethod]
        public void TestAmountConvertion()
        {

            var cem = (10000 / 100).ToString("F");
            var trintaum = (3110 / 100).ToString("F");


        }

        [TestMethod]
        public void CanSendEmailDebito()
        {

            var post = new EmailAccess().SendEmail(new Email
            {
                To = "rodrigocardozop@gmail.com",
                TargetName = "Rodrigo",
                TargetTextBlue = @"https://docs.pagar.me/",
                TargetSecondaryText = @"Valor total da sua fatura: 19,90 R$. Ao clicar no botão acima você será redirecionado para o pagamento por débito.",
                TemplateType = Convert.ToInt32(Email.TemplateTypes.Debito)
            });

            Assert.IsTrue(post);
        }

        [TestMethod]
        public void CanSendEmailBoletoWithDiscount()
        {

            var post = new EmailAccess().SendEmail(new Email
            {
                To = "rodrigocardozop@gmail.com",
                TargetName = "Rodrigo",
                //TargetTextBlue = @"https://docs.pagar.me/",
                TargetSecondaryText = @"https://docs.pagar.me/" + "\n Prezado cliente essa mensagem foi escrita no client side",
                TemplateType = Convert.ToInt32(Email.TemplateTypes.BoletoCharged),
                DiscountPrice = "10,00"
            });

            Assert.IsTrue(post);
        }



        [TestMethod]
        public void CanTestDate()
        {
            var date = new DateTime(2008, 2, 1, 0, 0, 0);
        }

        [TestMethod]
        public void CanGetCharginHistory()
        {
            //var listaPagamentos = new ChargingAcess().GetValidityCharginHistory(new Charging
            //{
            //    AnoVingencia = "2018",
            //    MesVingencia = "02"
            //});


            var listaPagamentos = new ChargingAcess().GetValidityPayments(new Charging
            {
                AnoVingencia = "2018",
                MesVingencia = "02"
            });


        }

        [TestMethod]
        public void CanGetLastPaidTransaction()
        {
            var listaPagamentos = new TransactionAccess().GetLastTransactionPaid(new Person { IdPagarme = 4312036 });
        }

        [TestMethod]
        public void CanGetAllLastPaidTransaction()
        {
            var listaPagamentos = new TransactionAccess().GetAllLastTransactionPaid();
        }


        [TestMethod]
        public void CanGetStatusValidity()
        {
            var listaPagamentos = new ChargingController().GetCustomerStatusVingencia(1, 5, 2018);
        }

        [TestMethod]
        public void CanGetChargingHistory()
        {
            //var listaPagamentos = new ProfileController().GetChargeAndServiceOrderHistory(4155);
        }

        [TestMethod]
        public void CanGetCobrancaFullVivoExtract()
        {
            var listaPagamentos = new ChargingAcess().GetCobrancaFullVivoExtract(3, 2018);
        }


        [TestMethod]
        public void CanChangeStatusCharging()
        {
            var update = new ChargingAcess().UpdateCharging(1, true);
            Assert.IsTrue(update);
        }

        [TestMethod]
        public void CanChangeStatusChargingList()
        {
            var list = new List<int>();
            list.Add(1);
            list.Add(2);
            list.Add(3);

            var update = new ChargingAcess().UpdateChargingList(list, true);
            Assert.IsTrue(update);
        }

        [TestMethod]
        public void CanGetListaCobrancaMassiva()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var teste = new ChargingAcess().GetListaCobrancaMassiva(10, 2018);
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine(elapsedMs);
            
        }

        [TestMethod]
        public void TestePagarme()
        {
            new PagarMeController().GeraCobrancaIntegrada(new Person
            {
                Id = 1,
                Charging = new Charging
                {
                    CommentBoleto = "teste",
                    Comment = "teste",
                    Ammount = "1000",
                    PaymentType = 1,
                    MesVingencia = "10",
                    AnoVingencia = "2018"
                }
            });
        }

        [TestMethod]
        public void TestePagarmeUpdate()
        {
            new TransactionAccess().GetQrCode();
        }

        [TestMethod]
        public void CanSaveStoreHistoryDocument()
        {
            
            var saveHistory = new ChargingAcess()
                .SaveChargingHistoryStore(new Person
                {
                    DocumentNumber = "90616693753",
                    Email = "marcio.franco@gmail.com",
                    Charging = new Charging
                    {
                        TransactionId = 101927,
                        Ammount = "100",
                        PaymentType = 1
                    }
                });

            Assert.IsTrue(saveHistory);
        }

        [TestMethod]
        public void CanSaveStoreHistoryId()
        {
            var saveHistory = new ChargingAcess()
                .SaveChargingHistoryStore(new Person
                {
                    Id = 1,
                    Charging = new Charging
                    {
                        TransactionId = 101927,
                        Ammount = "100",
                        PaymentType = 2,
                        BoletoId = 101927,
                    }
                });

            Assert.IsTrue(saveHistory);
        }

        [TestMethod]
        public void CanSaveCielo()
        {
            var saveHistory = new ChargingAcess()
                .InsertCieloCharging(new CieloPaymentModel {
                    Card = "uihwh89q98r32hf30h08f329h0",
                    PaymentMethod = 1,
                    CustomerIdERP = 1,
                    PaymentId = "6551651",
                    CustomerId = 2,
                    Amount = 100,
                    OrderId = "10",
                    PaymentGateway = "Cielo",
                    PaymentDate = DateTime.Now,
                    Address = new Adress
                    {
                        Cep = "28800000",
                        City = "Rioo",
                        StreetNumber = "100",
                        Country = "BR",
                        State = "RJ",
                        Street = "Rua do centro",
                        Neighborhood = "Centro"
                    }
                }); 

            Assert.IsTrue(saveHistory);
        }

        [TestMethod]
        public void CanGetMassChargingCustomers()
        {
            var teste = new ChargingAcess().GetMassChargingCustomers(8, 2019);
        }

        [TestMethod]
        public void CanSetupMassChargingCustomers()
        {
            var teste = new ChargingAcess().SetupAutomaticCharge(1, new Business.Commons.Entities.FoneClube.charging.MassChargingList());
        }

        [TestMethod]
        public void GetMassChargeLogCustomers()
        {
            var teste = new ChargingAcess().GetMassChargeLog(1);
        }

        [TestMethod]
        public void CanGetCards()
        {
            var debitCardHash = new CardUtils().PrepareCard(new CardFoneclube {
                Cvv = "820",
                ExpirationMonth = "11",
                ExpirationYear = "2019",
                Number = "4830420087436307",
                HolderName = "Marcio Guiamaraes Franco",
                Flag = "visa"
            });

            var teste = new CardUtils().EncryptFC(debitCardHash);
         
            var debitCard = new CardUtils().GetCard(teste);
        }

        [TestMethod]
        public void CanGetCardsRodrigo()
        {
            var debitCardHash = new CardUtils().PrepareCard(new CardFoneclube
            {
                Cvv = "350",
                ExpirationMonth = "11",
                ExpirationYear = "2024",
                Number = "4766084842803405",
                HolderName = "RODRIGO CARDOZO PINTO",
                Flag = "visa"
            });
            //QqxPS8daEMqB8l2ogACIQ738F+dQ4hs4J60nW1li0saonZgLZjWwac3j29VqmCT/htSt92toBcCCi1wxMyNmJy4sju+f2zGaKN+PeMFCy4E=
            var teste = new CardUtils().EncryptFC(debitCardHash);

            var debitCard = new CardUtils().GetCard(teste);
        }

        [TestMethod]
        public void CanDoDebitTransactionPaymentTransform()
        {
            var teste = new CieloAccess().EncryptCieloTransaction(new CieloDebitoTransaction
            {
                Ano = 2019,
                Mes = 9,
                CustomerId = 2,
                HistoryId = 1,
                Valor = 3990
            });

            var decrypt = new CieloAccess().DecryptCieloTransaction(teste);
        }

        [TestMethod]
        public void CanDoDebitTransactionPaymentTransformSecond()
        {
            var teste = new CieloAccess().EncryptCieloTransactionString(new CieloDebitoTransaction
            {
                Ano = 2019,
                Mes = 10,
                CustomerId = 5,
                HistoryId = 1533,
                Valor = 3990
            });

            var decrypt = new CieloAccess().DecryptCieloTransactionString(teste);
        }

        [TestMethod]
        public void CanDoDebitFirstLink()
        {
            var teste = new CieloAccess().GenerateFirstLinkDebito(new CieloDebitoTransaction
            {
                Ano = 2019,
                Mes = 10,
                CustomerId = 2,
                HistoryId = 1,
                Valor = 1950
            });

        }

        [TestMethod]
        public void CanDoDebitTransactionPayment()
        {
            var decrypt = new CieloAccess().GenerateDebitoCharge("MV8yMDE5XzEwXzE3NDdfMjAwMA==");
        }

        [TestMethod]
        public void CanCheckDebitoCard()
        {
            var decrypt = new CieloAccess().HasDebitCard(5);
        }

        [TestMethod]
        public void CanSaveTransaction()
        {
            //"50893320-adee-41ce-8893-7f66870a06e1"
            using (var ctx = new FoneClubeContext())
            {
                var transaction = new tblTransactionsCielo
                {
                    intGatewayId = CieloGatewayType.Id,
                    intHistoryId = 1,
                    intPersonId = 2,
                    txtPaymentId = "50893320-adee-41ce-8893-7f66870a06e1"
                };

                ctx.tblTransactionsCielo.Add(transaction);
                ctx.SaveChanges();
            }
        }

        [TestMethod]
        public void CanGetStatusBradesco()
        {
            var response = new ChargingAcess().GetStatusBoletoBradesco();
        }

        [TestMethod]
        public void CanGetStatus()
        {
            try
            {
                var url = ConfigurationManager.AppSettings["LINK_BRADESCO_BOLETO"];
                //var url = "https://nloja.foneclube.com.br/wp-admin/admin-ajax.php?action=loja5_woo_bradesco_api_boleto_cron";

                using (HttpClient client = new HttpClient())
                {
                    using (HttpResponseMessage response = client.GetAsync(url).Result)
                    {
                        using (HttpContent content = response.Content)
                        {
                            var result = content.ReadAsStringAsync().Result;

                            var separetor = new string[] { "<br>" };
                            var statusResults = result.Split(separetor, StringSplitOptions.None);

                            foreach (string statusResponse in statusResults)
                            {
                                if (statusResponse.Contains("pago"))
                                {
                                    var nicho = statusResponse.Split(' ');
                                    var idTransactionLoja = Convert.ToInt32(nicho[2]);
                                
                                    using (var ctx = new FoneClubeContext())
                                    {
                                        var charge = ctx.tblChargingHistory.FirstOrDefault(c => c.intIdCheckoutLoja == idTransactionLoja);
                                        charge.bitPago = true;
                                        ctx.SaveChanges();
                                    }
                                }    
                            }   
                        }
                    }
                }

                
            }
            catch (Exception e)
            {
                throw new HttpResponseException(
                            new Utils().GetErrorPostMessage(string.Format("Ocorreu um erro ao tentar coletar a lista")));
            }
        }

        [TestMethod]
        public void CanGetLastChargings()
        {
            new ChargingAcess().GetChargingsCustomers();
        }


        //auditoria compras
        [TestMethod]
        public void CanGetteste()
        {
            using (var ctx = new FoneClubeContext())
            {
                var emMaos = 1;
                var correios = 2;
                var frete = 1400;
                var histories = ctx.tblChargingHistory.Where(a => a.txtTokenTransaction != null && a.txtComment == "Checkout Loja Pagarme").ToList();
                var planos = ctx.tblPlansOptions.ToList();

                foreach (var history in histories)
                {
                    if(history.intIdFrete == emMaos)
                    {
                        frete = 0;
                    }

                    var total = Convert.ToInt32(history.txtAmmountPayment) - frete;
                    var totalVerificado = 0;
                    var planosSelecionados = history.txtTokenTransaction.Split(',');
                    foreach (var planoSelecionado in planosSelecionados)
                    {
                        var selecionado = Convert.ToInt32(planoSelecionado);
                        totalVerificado += planos.FirstOrDefault(a => a.intIdPlan == selecionado).intCost;
                    }

                    if (totalVerificado != total)
                    {
                        history.txtAmmountPayment = totalVerificado.ToString();
                        ctx.SaveChanges();
                        //Assert.Fail();
                    }
                }
            }
        }

        [TestMethod]
        public void ExecuteChargeSchedule()
        {
            
            var execute = new ChargingAcess().ExecuteCharges();
            
        }

        [TestMethod]
        public void DeleteChargeSchedule()
        {

            var execute = new ChargingAcess().DeleteScheduleCharging(1);

        }

        [TestMethod]
        public void MigrateHistory()
        {

            using (var ctx = new FoneClubeContext())
            {
                
                var charges = ctx.tblChargingHistory.Where(p => p.intIdCustomer == 2 && p.intId > 4642)
                    .ToList();

                foreach(var a in charges)
                {
                    var oc = new tblChargingScheduled
                    {
                        intIdCustomer = a.intIdCustomer,
                        intIdPaymentType = a.intIdPaymentType,
                        txtComment = a.txtComment,
                        dtePayment = a.dtePayment,
                        txtAmmountPayment = a.txtAmmountPayment,
                        intIdBoleto = a.intIdBoleto,
                        txtAcquireId = a.txtAcquireId,
                        dteCreate = a.dteCreate,
                        dteValidity = a.dteValidity,
                        intChargeStatusId = a.intChargeStatusId,
                        intIdTransaction = null,
                        txtCommentEmail = a.txtCommentEmail,
                        txtCommentBoleto = a.txtCommentBoleto,
                        pixCode = a.pixCode,
                        dteCharge = a.dteCharge,
                        dteChargingDate = a.dteChargingDate,
                        dteExecution = DateTime.Now,
                        bitExecuted = false
                    };

                    ctx.tblChargingScheduled.Add(oc);
                }

                ctx.SaveChanges();
                
            }


        }

    }
}
;
