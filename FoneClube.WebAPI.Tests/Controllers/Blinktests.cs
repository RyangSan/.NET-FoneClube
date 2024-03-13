using FoneClube.DataAccess.blink;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoneClube.WebAPI.Tests.Controllers
{
    [TestClass]
    public class Blinktests
    {
        [TestMethod]
        public void CanUseBlinkService()
        {
            var blinkManager = new BlinkIntegrationManager();
            
            var token = blinkManager.GetAuthenticationToken();

        }

        [TestMethod]
        public void CanUseBlinkLink()
        {
            var blinkManager = new BlinkIntegrationManager();
            var teste = blinkManager.CreateLinkIndication("http://cadastro.foneclube.com.br/NTA4Nw", "terceiro-teste");
        }

        
    }
}
