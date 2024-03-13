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
    public class VivoTest
    {
        [TestMethod]
        public void VivoBlockLineTest()
        {
            var vivo = new VivoAccess();
            vivo.SetLineStatus("10222355", false);

            var result = vivo.GetLineStatus("10222355");
            Assert.AreEqual("BLOQUEIO GESTOR", result);
        }

        [TestMethod]
        public void VivoUnblockLineTest()
        {
            var vivo = new VivoAccess();
            vivo.SetLineStatus("10222355", true);

            var result = vivo.GetLineStatus("10222355");

            Assert.AreEqual("ATIVADO", result);
        }

        [TestMethod]
        public void VivoListLinesTest()
        {
            var vivo = new VivoAccess();
            var result = vivo.ListNumbers("11405275");

            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void VivoLineStatusTest()
        {
            var vivo = new VivoAccess();
            vivo.SetLineStatus("10222355", true);

            var result = vivo.GetLineStatus("10222355");

            Assert.AreEqual("ATIVADO", result);
        }

        [TestMethod]
        public void GetStatus()
        {
            var result = new VivoAccess().GetLineStatus("21982008200");
            
        }
    }
}
