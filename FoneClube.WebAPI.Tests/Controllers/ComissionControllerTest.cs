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
using System.Net;
using System.IO;

namespace FoneClube.WebAPI.Tests.Controllers
{
    [TestClass]
    public class ComissionControllerTest
    {

        [TestMethod]
        public void CanComissionCliente()
        {
            using (var ctx = new FoneClubeContext())
            {
                var matriculaPai = 1;
                var comissionAcess = new ComissionAccess();

                var ordens = ctx.tblComissionOrder.Where(c => c.intIdCustomerReceiver == matriculaPai).ToList();
                var liberadas = ordens.Where(c => c.bitComissionConceded == true).ToList();
                var pendentes = ordens.Where(c => c.bitComissionConceded == false).ToList();

                var teste = comissionAcess.GetComissoesCliente(matriculaPai);

                //var liberadasPorCliente = liberadas.Where(c => c.intIdCustomerGiver == 4187).ToList(); 
                //var pendentesPorCliente = liberadas.Where(c => c.intIdCustomerGiver == 4187).ToList();
                //var valorLiberadoPorCliente = liberadasPorCliente.Select(c => c.intAmount).Take(ordens.Count).Sum();
                //var valorPendentesPorCliente = liberadasPorCliente.Select(c => c.intAmount).Take(ordens.Count).Sum();
            }
        }

        [TestMethod]
        public void CanAmountComissionCliente()
        {
            using (var ctx = new FoneClubeContext())
            {
                var matriculaPai = 1;
                var comissionAcess = new ComissionAccess();
                var teste2 = new ComissionAccess().GetComissionAmmount(matriculaPai);

            }
        }

        [TestMethod]
        public void CanComissionFlowExecuted()
        {
            using (var ctx = new FoneClubeContext())
            {
                var matriculaPai = 1;
                var comissionAcess = new ComissionAccess();
                comissionAcess.SetupComissionOrders();


                //var ordensLiberadas = comissionAcess.PrepareCustomerComissionOrders(matriculaPai);
                //var comissoes = comissionAcess.GetComissoesCustomer(matriculaPai);

                //var ammount = comissoes.Select(c => c.Ammount)
                //            .ToList().Take(comissoes.Count).Sum();

                //var teste = comissionAcess.GetComissoesCliente(matriculaPai);

                //var releaseComissions = comissionAcess.ReleaseComissions(1);

            }
        }

        [TestMethod]
        public void CanAplitBenefit()
        {
            using (var ctx = new FoneClubeContext())
            {
                var comissionAcess = new ComissionAccess();
                var matricula = 1;
                var valor = 250;
                var resultBenefit = comissionAcess.ReleaseSplitedBenefit(matricula, valor);
                Assert.IsTrue(resultBenefit.Success);

            }
        }

        [TestMethod]
        public void CanAplitBenefitCompare()
        {
            using (var ctx = new FoneClubeContext())
            {
                var comissionAcess = new ComissionAccess();
                var matricula = 1;
                var valor = 250;
                var totalizadores = comissionAcess.GetTotalizadoresComissao(matricula).ValorTotalLiberadoParaPagarCliente;
                var resultBenefit = comissionAcess.ReleaseSplitedBenefit(matricula, valor);
                
                Assert.IsTrue(resultBenefit.Success);
                Assert.AreEqual(totalizadores - valor, resultBenefit.Value);
                //275610
            }
        }

        [TestMethod]
        public void CanSaveLog()
        {
            using (var ctx = new FoneClubeContext())
            {
                var comissionAcess = new ComissionAccess();
                var log = comissionAcess.SaveLogComission("250_1_[{'IdOrder':2707,'Amount':50.0,'tipo':0,'Baixa':true,'Reduzido':false},{'IdOrder':2714,'Amount':100.0,'tipo':0,'Baixa':true,'Reduzido':false},{'IdOrder':2719,'Amount':100.0,'tipo':0,'Baixa':true,'Reduzido':false}]", ctx);
                Assert.IsTrue(log);

            }
        }

        [TestMethod]
        public void CanTestInconsistenciaPaternidade()
        {
            using (var ctx = new FoneClubeContext())
            {
                var matriculaPai = 4085;
                var comissionAcess = new ComissionAccess();
                //var temInconsistencia = comissionAcess.InconsistenciaPaternidade(matriculaPai);

                var info = comissionAcess.GetClientesListadosPaternidadeInfo();
                //var ordensLiberadas = comissionAcess.PrepareCustomerComissionOrders(matriculaPai);
                //var comissoes = comissionAcess.GetComissoesCustomer(matriculaPai);

                //var ammount = comissoes.Select(c => c.Ammount)
                //            .ToList().Take(comissoes.Count).Sum();

                //var teste = comissionAcess.GetComissoesCliente(matriculaPai);

                //var releaseComissions = comissionAcess.ReleaseComissions(1);

            }
        }

        [TestMethod]
        public void CanGetHierarquia()
        {
            using (var ctx = new FoneClubeContext())
            {
                var matriculaPai = 1;
                var comissionAcess = new ComissionAccess();
                var comissoes = comissionAcess.GetComissoesCliente(matriculaPai);
                var hierarquia = comissionAcess.GetHierarquiaCliente(matriculaPai);

            }
        }

        [TestMethod]
        public void CanAddLog()
        {
            var teste = "{'Amount':5298,'DaysLimit':0,'BoletoInstructions':null,'Nome':'Rodrigo Cardozo','Email':'rodrigocardozop@gmail.com','DocumentNumber':'10667103767','Street':'Rua Enio Augusto De Mello','StreetNumber':'170','Neighborhood':'Cidade Nova','Zipcode':'28800000','Ddd':'11','Number':'23456789','CardHolderName':'MARCIO G FRANCO','CardNumber':'5491591007676748','CardExpirationDate':'1120','CardCvv':'630'}";

            var request = WebRequest.Create("http://homol-api.p2badpmtjj.us-east-2.elasticbeanstalk.com/api/pagarme/cartao");
            request.Method = "POST";
            var byteArray = Encoding.UTF8.GetBytes(teste);
            request.ContentType = "application/json";
            request.ContentLength = byteArray.Length;

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(teste);
            }

            var httpResponse = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var responseText = streamReader.ReadToEnd();
            }
            //var comissions = new List<ComissaoOrdem>();
            //var statusDispatched = true;
            //AddComissionLog(comissions, statusDispatched);

        }

        [TestMethod]
        public void CanAddComission()
        {
            var comissionBonus = new ComissionAccess()
                .InsertBonusComission(new Person { Id = 1 });
            Assert.IsTrue(comissionBonus);
        }

        [TestMethod]
        public void CanGetHierarquiaPagante()
        {
            using (var ctx = new FoneClubeContext())
            {
                //var claro = ctx.CobFullClaro_Extract(3, 2018).ToList();
                //var vivo = ctx.CobFullVivo_Extract(3, 2018).ToList();
                var matricula = 1;
                var filhosNaoPagantes = ctx.GetFilhosHierarquiaNaoPagante(matricula).ToList()
                .Select(p => new Person
                {
                    Id = p.intIdSon.Value,
                    IdPagarme = p.intIdPagarme,
                    DocumentNumber = p.txtDocumentNumber,
                    Name = p.txtName,
                    Email = p.txtEmail
                }).ToList();

                var filhosPagantes =
                ctx.GetFilhosHierarquiaPagante(matricula).ToList()
                .Select(p => new Person {
                    Id = p.intIdSon.Value,
                    IdPagarme = p.intIdPagarme.Value,
                    DocumentNumber = p.txtDocumentNumber,
                    Name = p.txtName,
                    Email = p.txtEmail
                }).ToList();

            }
        }

        public void CanGetBonus()
        {
            using (var ctx = new FoneClubeContext())
            {
                var bonus = (from c in ctx.tblChargingHistory
                             join t in ctx.tblComissionOrder on c.intIdTransaction equals t.intIdTransaction
                             join b in ctx.tblBonusOptions on t.intIdBonus equals b.intIdBonus
                             where t.bitComissionConceded == false
                             && t.intIdCustomerReceiver == 1
                             && t.intIdBonus != null
                             select new ComissaoOrdem
                             {
                                 Id = t.intIdComissionOrder,
                                 IdRecebedor = c.intIdCustomer.Value,
                                 //TransactionId = t.intIdTransaction,
                                 //Level = t.intIdComission.Value,
                                 Ammount = b.intAmount.Value,
                                 Concedida = t.bitComissionConceded
                             }).ToList();
            }
        }

        [TestMethod]
        public void CanGetteste()
        {
            using (var ctx = new FoneClubeContext())
            {
                //            bitComissionConceded: false
                //dteConceded: null
                //dteCreated: { 5 / 28 / 2018 9:05:58 PM}
                //            dteValidity: null
                //intIdAgent: 1
                //intIdBonus: null
                //intIdComission: 1
                //intIdComissionOrder: 0
                //intIdCustomerReceiver: 1
                //intIdTransaction: 3608783
                //tblBonusOptions: null
                //tblComissionTokens: Count = 0
                //tblCommisionLevels: null
                //tblPersons: null

                var orders = new List<tblComissionOrder>();

                orders.Add(new tblComissionOrder
                {
                    bitComissionConceded = false,
                    dteCreated = DateTime.Now,
                    intIdTransaction = 3608783,
                    intIdAgent = 1,
                    intIdComission = 1,
                    intIdCustomerReceiver = 1
                });


                ctx.tblComissionOrder.AddRange(orders);
                ctx.SaveChanges();


            }
        }

        [TestMethod]
        public void CanGetComissions()
        {
            var history = new ComissionController().GetCustomerhierarchyDocument("90616693753");
            Assert.IsNotNull(history);
        }

        [TestMethod]
        public void CanSetupBonus()
        {
            //colocar esse update no processo

            var history = new ComissionAccess().SetupBonus();
            Assert.IsNotNull(history);
        }

        [TestMethod]
        public void CanGetBonusCustomer()
        {
            var matriculaNick = 4307;
            var history = new ComissionAccess().GetCustomerBonus(matriculaNick);
            Assert.IsNotNull(history);
        }

        [TestMethod]
        public void CanGetBonusCustomerCompare()
        {
            var matriculaNick = 4307;
            var bonusNick = new ComissionAccess().GetCustomerBonus(matriculaNick);
            //var bonusNickAmount = new ComissionAccess().GetCustomerBonusAmount(matriculaNick);

            var bonusMarcio = new ComissionAccess().GetCustomerBonus(1);
            //var bonusMarcioAmount = new ComissionAccess().GetCustomerBonusAmount(1);

            //Assert.AreEqual(bonusNick, bonusNickAmount);
            //Assert.AreEqual(bonusMarcio, bonusMarcioAmount);
        }

        [TestMethod]
        public void CanGetBonusLog()
        {
            var bonusLog = new ComissionAccess().GetBonusLog();
        }

        [TestMethod]
        public void GetHistoryBonusOrder()
        {
            var bonusLog = new ComissionAccess().GetHistoryBonusOrder(100);
        }

        

        [TestMethod]
        [Ignore]
        public void CanDispatchBonusCustomer()
        {
            var dispatch = new ComissionAccess().SetDispatchBonus(1);
            Assert.IsTrue(dispatch);
        }

        [TestMethod]
        //[Ignore]
        public void CanDispatchComissionCustomer()
        {
            var dispatch = new ComissionAccess().ReleaseComissions(2);
            Assert.IsTrue(dispatch);
        }

        [TestMethod]
        public void CanRestoreTransaction()
        {
            var dispatch = new TransactionAccess().RestoreTransactions();
            Assert.IsTrue(dispatch);
        }

        [TestMethod]
        public void CanGetTotalizadoresComissao()
        {
            var totalizadores = new ComissionAccess().GetTotalizadoresComissao(4307);
            Assert.IsNotNull(totalizadores);
        }

        [TestMethod]
        public void CanUpdateBonusAmount()
        {
            var bonusNick = new ComissionAccess().UpdateBonusAmount();
        }

        [TestMethod]
        public void GetDetalhesComissao()
        {
            var detalhes = new ComissionAccess().GetDetalhesComissao(4307);
        }

        [TestMethod]
        public void GetCustomerAmigosQuatroFilhos()
        {
            new ComissionAccess().GetCustomerAmigosQuatroFilhos();
        }

        [TestMethod]
        public void GetSetupDesconto()
        {
            new ComissionAccess().SetupDesconto();
        }

        //[TestMethod]
        //public void GetPromoCodeOwnerCustom()
        //{
        //    var customer = new ComissionAccess().GetPromoCodeOwner("CARDOZO");
        //}

        //[TestMethod]
        //public void GetPromoCodeOwnerBase()
        //{
        //    var customer = new ComissionAccess().GetPromoCodeOwner("Mg==");
        //}

        //[TestMethod]
        //public void GetPromoCodeOwnerInvexistente()
        //{
        //    var customer = new ComissionAccess().GetPromoCodeOwner("AAAA");
        //}

    }
}
