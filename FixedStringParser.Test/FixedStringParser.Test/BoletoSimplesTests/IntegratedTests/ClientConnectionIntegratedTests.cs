using FoneClube.BoletoSimples;
using FoneClube.BoletoSimples.APIs.Users.Models;
using FoneClube.BoletoSimples.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using FoneClube.DataAccess;
using FoneClube.Business.Commons.Entities;
using FoneClube.Business.Commons.Entities.FoneClube;

namespace FoneClube.Tests.BoletoSimplesTests.IntegratedTests
{
    [TestClass]
    public class ClientConnectionIntegratedTests
    {
        [TestMethod]
        public async Task Establish_connection_by_access_token_with_sucess()
        {

            // Arrange
            var client = new BoletoSimplesClient();

            // Act
            var response = await client.Auth.GetUserInfoAsync().ConfigureAwait(false);
            var successResponse = await response.GetSuccessResponseAsync().ConfigureAwait(false);
            client.Dispose();

            // Assert
            Assert.IsTrue(response.IsSuccess);
            Assert.IsInstanceOfType(successResponse, typeof(UserInfo));
        }

        [TestMethod]
        public async Task When_establish_connection_with_invalid_token_should_return_unauthorize_code()
        {

            // Arrange
            var invalidConnection = new ClientConnection(ConfigurationManager.AppSettings["boletosimple-api-url"],
                                                         ConfigurationManager.AppSettings["boletosimple-api-version"],
                                                         Guid.NewGuid().ToString(),
                                                         ConfigurationManager.AppSettings["boletosimple-useragent"]);
       

            var client = new BoletoSimplesClient(new HttpClient(), invalidConnection);

            // Act
            var response = await client.Auth.GetUserInfoAsync().ConfigureAwait(false);
            var successResponse = await response.GetSuccessResponseAsync().ConfigureAwait(false);
            client.Dispose();

            // Assert
            Assert.IsFalse(response.IsSuccess);
            Assert.AreEqual(response.ErrorResponse.StatusCode, System.Net.HttpStatusCode.Unauthorized);
        }


        [TestMethod]
        public async Task CanCreateBilletDirect()
        {
            var addresses = new List<Adress>();
            addresses.Add(new Adress {
                Street = "Rua da Glória",
                Neighborhood = "Rua da Glória",
                State = "RJ",
                City = "Rio de Janeiro",
                Cep = "22420006",
                Complement = "Centro",
                StreetNumber = "100"
            });

            var person = new Person {
                DocumentNumber = "16214852500",
                Name = "Joao silva",
                Email = "rodrigocardozop@gmail.com",
                Adresses = addresses
            };
            var charging = new Charging { Ammount = "200", ChargingComment = "Cobrado a partir da loja." };
            var create = await new ChargingAcess().CreateBankBilletDirect(person, charging);
        }



        }
}
