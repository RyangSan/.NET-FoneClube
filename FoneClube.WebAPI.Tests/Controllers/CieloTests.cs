using FoneClube.Business.Commons.Entities.Cielo;
using FoneClube.DataAccess;
using HttpService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FoneClube.WebAPI.Tests.Controllers
{
    [TestClass]
    public class CieloTests
    {
        [TestMethod]
        [Ignore]
        public void GetTransactionCieloCredito()
        {

            //documentação
            //https://developercielo.github.io/manual/cielo-ecommerce
            var resultTransaction = new CieloAccess().GetTransaction("ec318074-1c98-49a7-8a2e-7f17b1e38797");
            Assert.IsTrue(!string.IsNullOrEmpty(resultTransaction.Payment.ReceivedDate));
        }

        [TestMethod]
        public void GetTransactionCieloDebito()
        {
            var resultTransaction = new CieloAccess().GetTransaction("3083359a-da59-488b-a49b-59373d2d90f5");
            Assert.IsTrue(!string.IsNullOrEmpty(resultTransaction.Payment.ReceivedDate));
        }

        [TestMethod]
        public void GetTransactionCieloDebitoUpdateStatus()
        {
            var resultTransaction = new CieloAccess().UpdateStatusTransactionsCielo();
        }

        [TestMethod]
        public void GetRestoreHistoryTransactionCielo()
        {
            var resultTransaction = new CieloAccess().GetRestoreHistoryTransactionCielo(1795);
        }

        [TestMethod]
        public void GetCieloTransactionStatus()
        {
            using (var ctx = new FoneClubeContext())
            {
                var listaTransactions = ctx.tblTransactionsCielo.ToList();
                var transactionId = listaTransactions.FirstOrDefault().txtPaymentId;
                var resultTransaction = new CieloAccess().GetTransaction(transactionId);

                var status = Enum.Parse(typeof(Transaction.Tipo), resultTransaction.Payment.Status.ToString());
            }
        }

        

        [TestMethod]
        public void GetTransactionCieloBoleto()
        {
            var resultTransaction = new CieloAccess().GetTransaction("4082a62e-08ec-4f31-9345-de1ef3c2c520");
            Assert.IsTrue(!string.IsNullOrEmpty(resultTransaction.Payment.ReceivedDate));
        }

        [TestMethod]
        //[Ignore]
        public void RestoreCieloTransactionData()
        {
            var restore = new CieloAccess().RestoreCieloTransactionData();
        }

        [TestMethod]
        public void UpdateCieloTransactionData()
        {
            var restore = new CieloAccess().UpdateTransactionsCielo(new FoneClubeContext());
        }

        [TestMethod]
        public void ReleaseBenefitsCielo()
        {
            var releaseBenefit = new CieloAccess().ReleaseBenefitsCielo();
        }

        [TestMethod]
        [Ignore]
        public void TestInsert()
        {
            var orders = new List<CieloOrder>();

            orders.Add(new CieloOrder
            {
                Transaction = new CieloAccess().GetTransaction("4082a62e-08ec-4f31-9345-de1ef3c2c520"),
                CustomerId = 1,
                OrderId = string.Empty
            });

            orders.Add(new CieloOrder
            {
                Transaction = new CieloAccess().GetTransaction("b18ba61f-4bf7-4f32-9f22-30077d30cab9"),
                CustomerId = 1,
                OrderId = string.Empty
            });

            orders.Add(new CieloOrder
            {
                Transaction = new CieloAccess().GetTransaction("3083359a-da59-488b-a49b-59373d2d90f5"),
                CustomerId = 1,
                OrderId = string.Empty
            });

            new CieloAccess().SaveTransactionsCielo(orders, new FoneClubeContext());

            //seguir para liberacao nas que forem aptas, na anulacao e na baixa
        }

        
    }
}
