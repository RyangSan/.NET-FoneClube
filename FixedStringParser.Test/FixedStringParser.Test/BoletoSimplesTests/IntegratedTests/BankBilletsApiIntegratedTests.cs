using FoneClube.BoletoSimples.APIs.BankBillets.Models;
using FoneClube.BoletoSimples.APIs.BankBillets.RequestMessages;
using FoneClube.BoletoSimples.Common;
using FoneClube.Tests.BoletoSimplesTests.IntegratedTests.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Net;
using System.Threading.Tasks;

namespace FoneClube.Tests.BoletoSimplesTests.IntegratedTests
{
    [TestClass]
    public class BankBilletsApiIntegratedTests : IntegratedTestBase
    {
        private BankBillet Content { get; set; }

      

        [TestInitialize]
        public void InitTests()
        {
            Content = JsonConvert.DeserializeObject<BankBillet>(JsonConstants.BankBillet);
        }


        [TestMethod]
        public async Task Create_a_valid_bank_billet()
        {
            // Arrange
            ApiResponse<BankBillet> response;
            BankBillet successResponse;
         

            // Act
            response = await Client.BankBillets.PostAsync(Content).ConfigureAwait(false);
            successResponse = await response.GetSuccessResponseAsync().ConfigureAwait(false);

            // Assert
            Assert.IsTrue(response.IsSuccess);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.Created);
            Assert.IsInstanceOfType(successResponse, typeof(BankBillet));


        }

        [TestMethod]
        public async Task Get_a_bank_billet_with_success()
        {
            // Arrange
            var createResponse = await Client.BankBillets.PostAsync(Content).ConfigureAwait(false);
            var successCreateResponse = await createResponse.GetSuccessResponseAsync().ConfigureAwait(false);
            ApiResponse<BankBillet> response;
            BankBillet successResponse;

            // Act
            response = await Client.BankBillets.GetAsync(successCreateResponse.Id).ConfigureAwait(false);
            successResponse = await response.GetSuccessResponseAsync().ConfigureAwait(false);

            // Assert
            Assert.IsTrue(response.IsSuccess);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            Assert.IsInstanceOfType(successResponse, typeof(BankBillet));
        }


        [TestMethod]
        public async Task Cancel_a_bank_billet_with_success()
        {
            // Arrange
            Content.BeneficiaryName = "Cancel-Beneficiary-Name";
            var createResponse = await Client.BankBillets.PostAsync(Content).ConfigureAwait(false);
            var successCreateResponse = await createResponse.GetSuccessResponseAsync().ConfigureAwait(false);

    
            // Act
            var cancelResponse = await Client.BankBillets.CancelAsync(successCreateResponse.Id).ConfigureAwait(false);
            var afterCancelResponse = await Client.BankBillets.GetAsync(successCreateResponse.Id).ConfigureAwait(false);
            var afterCancelSuccessResponse = await afterCancelResponse.GetSuccessResponseAsync().ConfigureAwait(false);


            // Assert
            Assert.IsTrue(cancelResponse.IsSuccessStatusCode);
            Assert.AreEqual(cancelResponse.StatusCode, HttpStatusCode.NoContent);
            Assert.AreNotEqual(successCreateResponse.Status, afterCancelSuccessResponse.Status);
        }

        [TestMethod]
        public async Task Duplicate_a_bank_billet_paged_with_success()
        {
            // Arrange
            Content.BeneficiaryName = "Cancel-Beneficiary-Name";
            var createResponse = await Client.BankBillets.PostAsync(Content).ConfigureAwait(false);
            var successCreateResponse = await createResponse.GetSuccessResponseAsync().ConfigureAwait(false);

            ApiResponse<BankBillet> response;
            BankBillet successResponse;

            response = await Client.BankBillets.GetAsync(successCreateResponse.Id).ConfigureAwait(false);
            successResponse = await response.GetSuccessResponseAsync().ConfigureAwait(false);
            
            // Act
            var duplicateResponse = await Client.BankBillets.DuplicateAsync(successResponse.Id, new Duplicate { Amount = successResponse.Amount + 100 }).ConfigureAwait(false);
            var afterDuplicateSuccessResponse = await duplicateResponse.GetSuccessResponseAsync().ConfigureAwait(false);

            // Assert
            Assert.IsTrue(duplicateResponse.IsSuccess);
            Assert.AreEqual(duplicateResponse.StatusCode, HttpStatusCode.Created);
            Assert.IsTrue(successResponse.Amount < afterDuplicateSuccessResponse.Amount);

        }



    }
}
