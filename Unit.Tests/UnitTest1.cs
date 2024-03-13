using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FoneClube.WebAPI.Controllers;

namespace Unit.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var teste = new PagarMeController().GetTransactions();
        }
    }
}
