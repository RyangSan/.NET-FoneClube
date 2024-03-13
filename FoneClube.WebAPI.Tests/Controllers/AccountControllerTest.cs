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

namespace FoneClube.WebAPI.Tests.Controllers
{
    [TestClass]
    public class AccountControllerTest
    {
        [TestMethod]
        public void GetPlans()
        {
            var plans = new AccountController().GetPlans();
            Assert.IsTrue(plans.Count > 0);
        }

        [TestMethod]
        public void GetOperators()
        {
            var operators = new AccountController().GetOperators();
            Assert.IsTrue(operators.Count == 2);
        }

        [TestMethod]
        [Ignore]
        public void InsertImages()
        {
            var images = new List<string>();
            images.Add("0987654321");
            images.Add("1234567890");
            images.Add("qwertyuiop");
            var person = new Person { Id = 1, Images = images };
            //Assert.IsTrue(new ProfileAccess().SavePersonImages(person));
        }

        [TestMethod]
        [Ignore]
        public void InsertPlan()
        {
            var person = new Person { Id = 1, IdPlanOption = 1 };
            //Assert.IsTrue(new ProfileAccess().SavePersonPlan(person));
        }

        //todo inserção guid imagem do usuário
        //inserção plano do usuário

    }
}
