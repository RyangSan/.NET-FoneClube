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

namespace FoneClube.WebAPI.Tests.Controllers
{
    [TestClass]
    public class PhoneAccessTest
    {
        [TestMethod]
        public void GetEvents()
        {
            var events = new PhoneAccess().GetEventos();
            Assert.IsTrue(events.Count > 0);
        }

        [TestMethod]
        [Ignore]
        public void CanDesassociarLinha()
        {
            var phones = new List<Phone>();
            phones.Add(new Phone { DDD = "21", Number = "991302405", EventTypeId = Convert.ToInt32(Phone.PhoneEvents.DesassociarLinha) });
            //var events = new PhoneAccess().InsertPhonePropertyHistory(new Person
            //{
            //    Id = 1,
            //    Phones = phones
            //});

            //Assert.IsTrue(events);

        }

        [TestMethod]
        public void CanUpdateLinha()
        {
            var phone = new Phone { Id = 1222, IdPlanOption = 17 };
            var update = new PhoneAccess().UpdatePhonePlan(phone);

        }

        [TestMethod]
        public void CanInsertService()
        {
            var servicos = new List<PhoneService>();
            servicos.Add(new PhoneService { Id = 1 });

            var insert = new PhoneAccess().InsertPhoneService(new Phone {
                Id = 1398,
                Servicos = servicos
            });

            var update = new PhoneAccess().InsertPhoneService(new Phone
            {
                Id = 1398,
                Servicos = servicos
            });

            var deactive = new PhoneAccess().InsertDeactivePhoneService(new Phone
            {
                Id = 1398,
                Servicos = servicos
            });
        }

        [TestMethod]
        public void CanGetPhoneServices()
        {
            var phone = new PhoneAccess().GetPhoneServices(new Phone
            {
                Id = 1398
            });
        }

        [TestMethod]
        public void CanGetAllPlans()
        {
            var phone = new PhoneAccess().GetAllPlansOptions();
            Assert.IsTrue(phone.Count > 0);
        }

        [TestMethod]
        public void CanGetAllServices()
        {
            var services = new PhoneAccess().GetAllPhoneServices();
            Assert.IsTrue(services.Count > 0);
        }

        [TestMethod]
        [Ignore]
        public void CanInsertPlan()
        {
            var plan = new PhoneAccess().InsertNewPlan(new Plan {
                IdOperator = 1,
                Description = "Teste",
                Active = false,
                Cost = 3000
            });
            Assert.IsTrue(plan);
        }

        [TestMethod]
        [Ignore]
        public void CanInsertServiceFoneclube()
        {
            var service = new PhoneAccess().InsertNewService(new PhoneService
            {
                Descricao = "Teste",
                Assinatura = true,
                ExtraOption = true,
                AmountFoneclube = 1000,
                AmountOperadora = 900,
                Editavel = false
            });
            Assert.IsTrue(service);
        }

        [TestMethod]
        //[Ignore]
        public void CanInsertGenericPhoneFlag()
        {
            var phones = new List<Phone>();
            var flags = new List<Flag>();

            flags.Add(new Flag
            {
                IdType = 1,
                InteractionDescription = "Descrição feita pelo operador usuário no client",
                PendingInteraction = true
            });

            
            var service = new PhoneAccess().InsertPhoneFlag(new Phone
            {
                Id = 8,
                Flags = flags
            });
            Assert.IsTrue(service);
            
            
        }

        [TestMethod]
        public void GetFlagsTypesPhones()
        {
            var types = new PhoneAccess().GetGenericFlagsTypes(true);
        }

        [TestMethod]
        public void GetFlagsTypesAll()
        {
            var types = new PhoneAccess().GetGenericFlagsTypes(false);
        }

        [TestMethod]
        [Ignore]
        public void CanInsertGenericFlag()
        {
            var insertFlag = new PhoneAccess().InsertGenericFlag(new Business.Commons.Entities.FoneClube.flag.GenericFlag {
                IdFlagType = 2,
                Description = "Flag inserida mas concluída",
                IdPhone = 8,
                PendingInteraction = false
            });
            Assert.IsTrue(insertFlag.FlagSuccess);

        }

        [TestMethod]
        //[Ignore]
        public void CanInsertGenericFlagEmail()
        {
            var insertFlag = new PhoneAccess().InsertGenericFlag(new Business.Commons.Entities.FoneClube.flag.GenericFlag
            {
                IdFlagType = 1,
                Description = "Flag inserida mas concluída",
                IdPhone = 8,
                PendingInteraction = false,
                PlanId = 16
            });
            Assert.IsTrue(insertFlag.EmailSuccess);
            Assert.IsTrue(insertFlag.FlagSuccess);

        }

        [TestMethod]
        public void CanGetGenericPhoneFlag()
        {
            var phones = new List<Phone>();
            phones.Add(new Phone
            {
                Id = 7
            });

            using (var ctx = new FoneClubeContext())
            {
                var phonesList = new PhoneAccess().GetGenericPhoneFlags(phones, ctx);
            }

        }

        [TestMethod]
        public void GetAvailablePhoneNumbers()
        {
            var phonesList = new PhoneAccess().GetAvailablePhoneNumbers("21994646991");
        }

        [TestMethod]
        public void GetAllPhonesNumbers()
        {
            var phonesList = new PhoneAccess().GetAllPhonesNumbers();
        }

        [TestMethod]
        public void GetAllCustomerPhones()
        {
            var phonesList = new PhoneAccess().GetAllCustomerPhones(1);
        }

        [TestMethod]
        public void GetPersonPropertyHistory()
        {
            var phonesList = new PhoneAccess().GetPersonPropertyHistory(new Person { Id = 34});
        
        }
        [TestMethod]
        public void GetAllGenericFlags()
        {
            var phonesList = new PhoneAccess().GetAllGenericFlags(5);
        }

        [TestMethod]
        public void GetLinhasFoneclubeEstoque()
        {
            var teste = new PhoneAccess().GetLinhasFoneclubeEstoque();
        }


    }
}
