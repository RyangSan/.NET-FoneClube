using FoneClube.BoletoSimples;
using FoneClube.BoletoSimples.Common;
using FoneClube.BoletoSimples.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace FoneClube.Tests.BoletoSimplesTests.IntegratedTests
{
    public class IntegratedTestBase
    {
        protected readonly BoletoSimplesClient Client;
       

        public IntegratedTestBase()
        {
            var customClient = new HttpClient();

            Client = new BoletoSimplesClient(customClient, new ClientConnection());

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver { NamingStrategy = new SnakeCaseNamingStrategy() },
                Converters = new List<JsonConverter> { new BrazilianCurrencyJsonConverter() }
            };
        }
    }
    
}
