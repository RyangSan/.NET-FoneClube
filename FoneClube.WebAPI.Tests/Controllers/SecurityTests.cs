using FoneClube.DataAccess.security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoneClube.WebAPI.Tests.Controllers
{
    [TestClass]
    public class SecurityTests
    {
        [TestMethod]
        public void CanHashPassword()
        {
            var password = new Security().EncryptPassword("123qwe");
            Assert.AreEqual("777DEF05330D6E7A19F54B4F2392D9028762D401", password.Password);
        }
    }
}
