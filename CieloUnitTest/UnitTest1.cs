using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CieloLib.Boleto;
using CieloLib.Domain;
using CieloLib;
using System.Text;
using Newtonsoft.Json;
using CieloLib.Eft;
using CieloLib.Debit;
using CieloLib.Debit.Domain;
using System.Linq;
using CieloLib.Credit;

namespace CieloUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        ILogger logger = new Logger();

        public UnitTest1()
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            };
        }

        [TestMethod]
        public void DoBoletoPayment()
        {
            bool useSandbox = true;
            string provider = useSandbox ? "Simulado" : "Bradesco2";
            string merchantId = useSandbox ? "b27f29ac-64b8-407d-9297-c9828cc838dd" : "";
            string securityKey = useSandbox ? "CJJTEDHWIKYQECLIIBFTFXWEPVUSTZTNBLIQEQUV" : "";
            string cpf = "90616666666";

            var settings = new BoletoPaymentSettings() {
                AdditionalFee = 0,
                AdditionalFeePercentage = false,
                DaysBeforeLinkExpiration = 10,
                MerchantId = new Guid(merchantId),
                PaymentInstruction = "for activation of your line this bank slip must be paid",
                PaymentType = "Boleto",
                Provider = provider,
                ReturnUrl = "https://loja.foneclube.com.br/CieloBoleto/Verify",
                SecurityKey = securityKey,
                UseSandbox = useSandbox
            };

            PaymentRequest paymentRequest = new PaymentRequest {
                CustomerId = 917,
                OrderGuid = Guid.NewGuid(),
                OrderTotal = 280
            };

            Customer customer = new Customer {
                Address = new CustomerAddress {
                    City = "New York",
                    Country = "United States",
                    State = "Ne",
                    Number  = "10021",
                    ZipCode = "10021",
                    District = "21 West 52nd Street",
                    Street = "NA"
                },
                Identity = cpf,
                Name = "Babawale S"
            };

            BoletoProcessor bp = new BoletoProcessor(settings);
            var paymentResponse = bp.ProcessPayment(paymentRequest, customer);

            StringBuilder sb = new StringBuilder();
            sb.Append("Settings:\t").AppendLine(JsonConvert.SerializeObject(settings));
            sb.Append("PaymentRequest:\t").AppendLine(JsonConvert.SerializeObject(paymentRequest));
            sb.Append("Customer:\t").AppendLine(JsonConvert.SerializeObject(customer));
            sb.Append("PaymentResponse:\t").AppendLine(JsonConvert.SerializeObject(paymentResponse)).AppendLine("\n");
            logger.Info(sb.ToString());

            Assert.AreEqual(paymentResponse.Status, BoletoProcessor.TS_PENDING);
        }


        [TestMethod]
        public void DoEftPayment()
        {
            bool useSandbox = true;
            string provider = useSandbox ? "Simulado" : "Bradesco";
            string merchantId = useSandbox ? "b27f29ac-64b8-407d-9297-c9828cc838dd" : "";
            string securityKey = useSandbox ? "CJJTEDHWIKYQECLIIBFTFXWEPVUSTZTNBLIQEQUV" : "";
            string cpf = "90616666666";

            var settings = new EftPaymentSettings()
            {
                AdditionalFee = 0,
                AdditionalFeePercentage = false,
                MerchantId = new Guid(merchantId),
                PaymentType = "EletronicTransfer",
                Installments = 1,
                Provider = provider,
                ReturnUrl = "https://loja.foneclube.com.br/CieloEft/Verify",
                SecurityKey = securityKey,
                UseSandbox = useSandbox
            };

            PaymentRequest paymentRequest = new PaymentRequest
            {
                CustomerId = 917,
                OrderGuid = Guid.NewGuid(),
                OrderTotal = 280
            };

            Customer customer = new Customer
            {
                Address = new CustomerAddress
                {
                    City = "New York",
                    Country = "United States",
                    State = "Ne",
                    Number = "10021",
                    ZipCode = "10021",
                    District = "21 West 52nd Street",
                    Street = "NA"
                },
                Identity = cpf,
                Name = "Babawale S"
            };

            var processor = new EftProcessor(settings, logger);
            var paymentResponse = processor.ProcessPayment(paymentRequest, customer);

            StringBuilder sb = new StringBuilder();
            sb.Append("Settings:\t").AppendLine(JsonConvert.SerializeObject(settings));
            sb.Append("PaymentRequest:\t").AppendLine(JsonConvert.SerializeObject(paymentRequest));
            sb.Append("Customer:\t").AppendLine(JsonConvert.SerializeObject(customer));
            sb.Append("PaymentResponse:\t").AppendLine(JsonConvert.SerializeObject(paymentResponse)).AppendLine("\n");
            logger.Info(sb.ToString());

            Assert.AreEqual(paymentResponse.Status, EftProcessor.TS_PENDING);
        }


        #region Debitcard Test Methods
        [TestMethod]
        public void DoDebitPayment()
        {
            bool useSandbox = false;
            string provider = useSandbox ? "Simulado" : "Bradesco";
            string merchantId = useSandbox ? "b27f29ac-64b8-407d-9297-c9828cc838dd" : "99879411-cc78-4e23-875a-95c91639260b";
            string securityKey = useSandbox ? "CJJTEDHWIKYQECLIIBFTFXWEPVUSTZTNBLIQEQUV" : "B5fz29Vtfj3MuJ4KQGwSLDiEFiVPmeSZ6bMG1QQw";
            string cpf = "90616693753";

            var settings = new DebitPaymentSettings()
            {
                AdditionalFee = 0,
                AdditionalFeePercentage = false,
                MerchantId = new Guid(merchantId),
                PaymentType = "DebitCard",
                Installments = 1,
                Provider = provider,
                ReturnUrl = "https://loja.foneclube.com.br/CieloDebit/Verify",
                SecurityKey = securityKey,
                UseSandbox = useSandbox,
                AuthenticateTransaction = true
            };

            PaymentRequest paymentRequest = new PaymentRequest
            {
                OrderGuid = Guid.NewGuid(),
                //OrderTotal = 10.05m,
                OrderTotal = Convert.ToInt32(Decimal.Parse((10.05).ToString())),
                CreditCardCvv2 = "820",
                CreditCardExpireMonth = 11,
                CreditCardExpireYear = 2019,
                CreditCardName = "Marcio Guiamaraes Franco",
                CreditCardNumber = "4830420087436307",
                CreditCardType = "visa"    //visa, Master, Discover, Amex, Elo, Aura, JCB, Diners, Hicard

            };

            Customer customer = new Customer
            {
                Address = new CustomerAddress
                {
                    City = "Rio de Janeiro",
                    Country = "Brasil",
                    State = "RJ",
                    Number = "10021",
                    ZipCode = "22631910",
                    District = "Rio de Janeiro",
                    Street = "Avenida das Américas"
                },
                Identity = cpf,
                Name = "Marcio Guiamaraes Franco"
            };

            var processor = new DebitProcessor(settings);
            var paymentResponse = processor.ProcessPayment(paymentRequest, customer);

            StringBuilder sb = new StringBuilder();
            sb.Append("Settings:\t").AppendLine(JsonConvert.SerializeObject(settings));
            sb.Append("PaymentRequest:\t").AppendLine(JsonConvert.SerializeObject(paymentRequest));
            sb.Append("Customer:\t").AppendLine(JsonConvert.SerializeObject(customer));
            sb.Append("PaymentResponse:\t").AppendLine(JsonConvert.SerializeObject(paymentResponse));

            sb.Append("###QueryUrl###:\t").AppendLine(paymentResponse.Response.Payment.Links.FirstOrDefault()?.Href);
            sb.Append("###PaymentId###:\t").AppendLine(paymentResponse.Response.Payment.PaymentId.ToString());

            if (paymentResponse.Status == DebitProcessor.TS_PENDING && !string.IsNullOrWhiteSpace(paymentResponse.Response.Payment.AuthenticationUrl))
            {
                //Authentication required
                sb.Append("Status:\t").AppendLine($"Authentication Required @ {paymentResponse.Response.Payment.AuthenticationUrl}");
                logger.Info(sb.AppendLine("\n").ToString());
            }
            else if (paymentResponse.Status == DebitProcessor.TS_PAID)
            {
                //Transaction Completed
                sb.Append("###Status###:\t").AppendLine($"Transaction Completed, Operation Successful");
                logger.Info(sb.AppendLine("\n").ToString());
            }
            else
            {
                logger.Info(sb.AppendLine("\n").ToString());
                Assert.Fail();
            }
        }


        [TestMethod]
        public void DoDebitPaymentVerification()
        {
            bool useSandbox = true;
            string provider = useSandbox ? "Simulado" : "Bradesco";
            string merchantId = useSandbox ? "b27f29ac-64b8-407d-9297-c9828cc838dd" : "";
            string securityKey = useSandbox ? "CJJTEDHWIKYQECLIIBFTFXWEPVUSTZTNBLIQEQUV" : "";

            var settings = new DebitPaymentSettings()
            {
                AdditionalFee = 0,
                AdditionalFeePercentage = false,
                MerchantId = new Guid(merchantId),
                PaymentType = "DebitCard",
                Installments = 1,
                Provider = provider,
                ReturnUrl = "https://loja.foneclube.com.br/CieloDebit/Verify",
                SecurityKey = securityKey,
                UseSandbox = useSandbox
            };

            //get queryUrl after executing DoDebitPayment, from PaymentResponse.Response.Payment.Links[0]
            var queryUrl = "https://apiquerysandbox.cieloecommerce.cielo.com.br/1/sales/f0488457-fec1-488c-9607-14f43817f0a8"; //PaymentResponse.Response.Payment.Links[0]

            var processor = new DebitProcessor(settings);
            var response = processor.VerifyPayment<DebitResponse>(queryUrl);

            var msg = "not confirmed";
            if (response?.Payment.Status == 2)
            {
                msg = "Payment confirmed and finalised";
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("Settings:\t").AppendLine(JsonConvert.SerializeObject(settings));
            sb.Append("queryUrl:\t").AppendLine(queryUrl);
            sb.Append("PaymentResponse:\t").AppendLine(JsonConvert.SerializeObject(response)).AppendLine("\n");
            logger.Info(sb.ToString());

            Assert.AreEqual(2, response?.Payment.Status);  //2 = Payment confirmed and finalised
        }
        #endregion



        #region Creditcard Test Methods

        [TestMethod]
        public void DoCreditPayment()
        {
            bool useSandbox = true;
            string provider = useSandbox ? "Simulado" : "Bradesco";
            string merchantId = useSandbox ? "b27f29ac-64b8-407d-9297-c9828cc838dd" : "";
            string securityKey = useSandbox ? "CJJTEDHWIKYQECLIIBFTFXWEPVUSTZTNBLIQEQUV" : "";
            string cpf = "90616666666";

            var settings = new CreditPaymentSettings()
            {
                AdditionalFee = 0,
                AdditionalFeePercentage = false,
                MerchantId = new Guid(merchantId),
                PaymentType = "CreditCard",
                Installments = 3,
                Provider = provider,
                ReturnUrl = "https://loja.foneclube.com.br/CieloCredit/Verify",
                SecurityKey = securityKey,
                UseSandbox = useSandbox
            };

            PaymentRequest paymentRequest = new PaymentRequest
            {
                CustomerId = 917,
                OrderGuid = Guid.NewGuid(),
                OrderTotal = 280,

                CreditCardCvv2 = "744",
                CreditCardExpireMonth = 09,
                CreditCardExpireYear = 2023,
                CreditCardName = "Babawale S",
                CreditCardNumber = "5555555555554444",
                CreditCardType = "Master",   //visa, Master, Discover, Amex, Elo, Aura, JCB, Diners, Hicard
                SoftDescriptor = "cielo gateway",
                SaveCard = true

            };

            var saveCardCustomerInfo = new CieloLib.Credit.Domain.Customer
            {
                Email = "babawale@phoneclube.com",
                FirstName = "Babawale",
                LastName = "S",
                Phone = "09033675431"
            };

            Customer customer = new Customer
            {
                Address = new CustomerAddress
                {
                    City = "New York",
                    Country = "United States",
                    State = "Ne",
                    Number = "10021",
                    ZipCode = "10021",
                    District = "21 West 52nd Street",
                    Street = "NA"
                },
                Identity = cpf,

                Name = "Babawale S",
                Status = "New"
        };



            var processor = new CreditProcessor(settings);
            var paymentResponse = processor.ProcessPayment(paymentRequest, customer, null, saveCardCustomerInfo);



            StringBuilder sb = new StringBuilder();
            sb.Append("Settings:\t").AppendLine(JsonConvert.SerializeObject(settings));
            sb.Append("PaymentRequest:\t").AppendLine(JsonConvert.SerializeObject(paymentRequest));
            sb.Append("Customer:\t").AppendLine(JsonConvert.SerializeObject(customer));
            sb.Append("PaymentResponse:\t").AppendLine(JsonConvert.SerializeObject(paymentResponse));

            sb.Append("###QueryUrl###:\t").AppendLine(paymentResponse.Response.Payment.Links.FirstOrDefault()?.Href);
            sb.Append("###PaymentId###:\t").AppendLine(paymentResponse.Response.Payment.PaymentId.ToString());

            if (paymentResponse.Status == CreditProcessor.TS_PENDING &&
                !string.IsNullOrWhiteSpace(paymentResponse.Response.Payment.AuthenticationUrl))
            {
                //Authentication required
                sb.Append("Status:\t").AppendLine($"Authentication Required @ {paymentResponse.Response.Payment.AuthenticationUrl}");
                logger.Info(sb.AppendLine("\n").ToString());
            }
            else if (paymentResponse.Status == CreditProcessor.TS_AUTHORIZED)
            {
                //Transaction Completed
                sb.Append("###Status###:\t").AppendLine($"Transaction Completed, Operation Successful");
                logger.Info(sb.AppendLine("\n").ToString());
            }
            else
            {
                logger.Info(sb.AppendLine("\n").ToString());
                Assert.Fail();
            }
        }

        [TestMethod]
        public void DoCreditCapture()
        {
            bool useSandbox = true;
            string provider = useSandbox ? "Simulado" : "Bradesco";
            string merchantId = useSandbox ? "b27f29ac-64b8-407d-9297-c9828cc838dd" : "";
            string securityKey = useSandbox ? "CJJTEDHWIKYQECLIIBFTFXWEPVUSTZTNBLIQEQUV" : "";

            var settings = new CreditPaymentSettings()
            {
                AdditionalFee = 0,
                AdditionalFeePercentage = false,
                MerchantId = new Guid(merchantId),
                PaymentType = "CreditCard",
                Installments = 1,
                Provider = provider,
                ReturnUrl = "https://loja.foneclube.com.br/CieloCredit/Verify",
                SecurityKey = securityKey,
                UseSandbox = useSandbox
            };

            string paymentId = "1ed8cc09-fe7d-4dae-8699-a59b8c034eb5";
            var processor = new CreditProcessor(settings);
            var paymentResponse = processor.ProcessCapture(paymentId);


            StringBuilder sb = new StringBuilder();
            sb.Append("Settings:\t").AppendLine(JsonConvert.SerializeObject(settings));
            sb.Append("PaymentId:\t").AppendLine(paymentId);
            sb.Append("PaymentResponse:\t").AppendLine(JsonConvert.SerializeObject(paymentResponse));

            sb.Append("###QueryUrl###:\t").AppendLine(paymentResponse.Response?.Links?.FirstOrDefault()?.Href);

            //Transaction Completed
            sb.Append("###Status###:\t").AppendLine(paymentResponse.Status);
            logger.Info(sb.AppendLine("\n").ToString());


            if (paymentResponse.Response.ReturnCode != 6) Assert.Fail();
        }

        [TestMethod]
        public void DoCreditPaymentWithSavedCard()
        {
            bool useSandbox = true;
            string provider = useSandbox ? "Simulado" : "Bradesco";
            string merchantId = useSandbox ? "b27f29ac-64b8-407d-9297-c9828cc838dd" : "";
            string securityKey = useSandbox ? "CJJTEDHWIKYQECLIIBFTFXWEPVUSTZTNBLIQEQUV" : "";
            string cpf = "90616666666";

            var settings = new CreditPaymentSettings()
            {
                AdditionalFee = 0,
                AdditionalFeePercentage = false,
                MerchantId = new Guid(merchantId),
                PaymentType = "CreditCard",
                Installments = 1,
                Provider = provider,
                ReturnUrl = "https://loja.foneclube.com.br/CieloCredit/Verify",
                SecurityKey = securityKey,
                UseSandbox = useSandbox
            };

            PaymentRequest paymentRequest = new PaymentRequest
            {
                CustomerId = 917,
                OrderGuid = Guid.NewGuid(),
                OrderTotal = 280
            };

            var storedCard = new CieloLib.Credit.Domain.StoredCard
            {
                CardBrand = "visa",
                CardToken = "4ce27b45-197c-487f-900c-21b15053d577",
                CreditCardCvv2 = "744"
            };

            Customer customer = new Customer
            {
                Address = new CustomerAddress
                {
                    City = "New York",
                    Country = "United States",
                    State = "Ne",
                    Number = "10021",
                    ZipCode = "10021",
                    District = "21 West 52nd Street",
                    Street = "NA"
                },
                Identity = cpf,
                Name = "Babawale S"
            };

            var processor = new CreditProcessor(settings);
            var paymentResponse = processor.ProcessPayment(paymentRequest, customer, storedCard);

            StringBuilder sb = new StringBuilder();
            sb.Append("Settings:\t").AppendLine(JsonConvert.SerializeObject(settings));
            sb.Append("PaymentRequest:\t").AppendLine(JsonConvert.SerializeObject(paymentRequest));
            sb.Append("Customer:\t").AppendLine(JsonConvert.SerializeObject(customer));
            sb.Append("PaymentResponse:\t").AppendLine(JsonConvert.SerializeObject(paymentResponse));

            sb.Append("###QueryUrl###:\t").AppendLine(paymentResponse.Response.Payment.Links.FirstOrDefault()?.Href);
            sb.Append("###PaymentId###:\t").AppendLine(paymentResponse.Response.Payment.PaymentId.ToString());

            if (paymentResponse.Status == CreditProcessor.TS_PENDING &&
                !string.IsNullOrWhiteSpace(paymentResponse.Response.Payment.AuthenticationUrl))
            {
                //Authentication required
                sb.Append("Status:\t").AppendLine($"Authentication Required @ {paymentResponse.Response.Payment.AuthenticationUrl}");
                logger.Info(sb.AppendLine("\n").ToString());
            }
            else if (paymentResponse.Status == CreditProcessor.TS_AUTHORIZED)
            {
                //Transaction Completed
                sb.Append("###Status###:\t").AppendLine($"Transaction Completed, Operation Successful");
                logger.Info(sb.AppendLine("\n").ToString());
            }
            else
            {
                logger.Info(sb.AppendLine("\n").ToString());
                Assert.Fail();
            }
        }


        [TestMethod]
        public void DoCreditPaymentVerification()
        {
            bool useSandbox = true;
            string provider = useSandbox ? "Simulado" : "Bradesco";
            string merchantId = useSandbox ? "b27f29ac-64b8-407d-9297-c9828cc838dd" : "";
            string securityKey = useSandbox ? "CJJTEDHWIKYQECLIIBFTFXWEPVUSTZTNBLIQEQUV" : "";

            var settings = new CreditPaymentSettings()
            {
                AdditionalFee = 0,
                AdditionalFeePercentage = false,
                MerchantId = new Guid(merchantId),
                PaymentType = "CreditCard",
                Installments = 1,
                Provider = provider,
                ReturnUrl = "https://loja.foneclube.com.br/CieloCredit/Verify",
                SecurityKey = securityKey,
                UseSandbox = useSandbox
            };



            //get queryUrl after executing DoCreditPayment, from PaymentResponse.Response.Payment.Links[0]
            var queryUrl = "https://apiquerysandbox.cieloecommerce.cielo.com.br/1/sales/f0488457-fec1-488c-9607-14f43817f0a8"; //PaymentResponse.Response.Payment.Links[0]




            var processor = new CreditProcessor(settings);
            var response = processor.VerifyPayment<CreditResponse>(queryUrl);

            var msg = "not confirmed";
            if (response?.Payment.Status == 2)
            {
                msg = "Payment confirmed and finalised";
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("Settings:\t").AppendLine(JsonConvert.SerializeObject(settings));
            sb.Append("queryUrl:\t").AppendLine(queryUrl);
            sb.Append("PaymentResponse:\t").AppendLine(JsonConvert.SerializeObject(response)).AppendLine("\n");
            logger.Info(sb.ToString());

            Assert.AreEqual(2, response?.Payment.Status);  //2 = Payment confirmed and finalised
        }
        #endregion
    }
}
