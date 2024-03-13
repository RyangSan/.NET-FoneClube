using FoneClube.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoneClube.Tests
{
    [TestClass]
    public class ClaroTest
    {
        [TestMethod]
        public void ClaroGetLineDetailsTest()
        {
            var claro = new ClaroAccess();
            var line = claro.GetLineDetails("21966209299");


            var bloqueado = line.Profile.Contains("BLOQUEADO");
            
        }

        [TestMethod]
        public void ClaroUpdateLineTest()
        {
            var claro = new ClaroAccess();
            var result = claro.UpdateLine("21974503161", "1755119350", null, null, "0", null, null, null, "", null);

            Assert.AreEqual("Informa��es atualizadas com sucesso!", result);
        }

        [TestMethod]
        public void ClaroBlockLine()
        {
            var claro = new ClaroAccess();
            var result = claro.BlockLine("21975004581", "1755119350");

            Assert.AreEqual("Informa��es atualizadas com sucesso!", result);
        }

        [TestMethod]
        public void ClaroIsLineBlocked()
        {
            var claro = new ClaroAccess();
            var result = claro.IsLineBlocked("21974503161", "1755119350");

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void ClaroUnblockLine()
        {
            var claro = new ClaroAccess();
            var result = claro.UnblockLine("21975004581", "1755119350");

            Assert.AreEqual("Informa��es atualizadas com sucesso!", result);
        }

        [TestMethod]
        public void ClaroSMSBalanceConfigTest()
        {
            var claro = new ClaroAccess();
            var result = claro.SMSBalanceConfig(60, 75, 85, 95);

            Assert.AreEqual("Percentuais atualizados com sucesso!", result);
        }

        [TestMethod]
        public void ClaroLongDistanceConfigTest()
        {
            var claro = new ClaroAccess();
            var result = claro.LongDistanceConfig(21, true);

            Assert.AreEqual("Informa��es atualizadas com sucesso!", result);
        }

        [TestMethod]
        public void ClaroListLinesTest()
        {
            var claro = new ClaroAccess();
            var result = claro.ListLines();

            Assert.IsTrue(result.Count > 0);
        }
    }
}
