using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Collections;
using Business.Commons;
using Business.Commons.Entities;
using Febraban;
using FoneClube.DataAccess;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoneClube.BoletoSimples.Common;
using FoneClube.BoletoSimples;
using System.Net.Http;

namespace FoneClube.Tests.BoletoSimplesTests.UnitTests
{
    [TestClass]
    public class HttpRequestBuilderUnitTest
    {
        private HttpClientRequestBuilder Builder;

        [TestInitialize]
        public void InitTests()
        {
            var connection = new ClientConnection(@"https://sandbox.boletosimples.com.br/api", "v1",
                "3f88ca60e0c7dc5702e9057b889ce6e11c8f2ee85ad2bcaaeb0575f88e7ca163",
                "Meu e-Commerce (meuecommerce@example.com)");
            var client = new BoletoSimplesClient(connection);
            Builder = new HttpClientRequestBuilder(client);
        }

        [TestMethod]
        public async Task Build_a_complex_http_request_message_with_json_body_custom_header_and_query_string()
        {
            // Arrange and Act
            var buildedRequestMessage = Builder.To(new Uri("http://any-uri.io"), "any-resource")
                                               .WithMethod(HttpMethod.Post)
                                               .AndOptionalContent(new { AnyContent = "Any Content" })
                                               .AppendQuery(new Dictionary<string, string> { ["query"] = "any" })
                                               .AditionalHeaders(new Dictionary<string, string> { ["My-Header"] = "Any Value" })
                                               .Build();

            var content = await buildedRequestMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

            // Assert
            Assert.AreEqual(buildedRequestMessage.RequestUri, new Uri("http://any-uri.io/any-resource?query=any"));
            Assert.AreEqual(buildedRequestMessage.Method, HttpMethod.Post);
            Assert.IsTrue(content.Contains("Any Content"));
            Assert.IsTrue(buildedRequestMessage.Headers.Authorization != null);
            Assert.AreEqual(buildedRequestMessage.Headers.GetValues("My-Header").First(), "Any Value");
            
        }

         [TestMethod]
        public async Task Build_a_simple_http_request_message_without_body()
        {
            // Arrange and Act
            var buildedRequestMessage = Builder.To(new Uri("http://any-uri.io"), "any-resource")
                                               .WithMethod(HttpMethod.Get)
                                               .Build();
            // Assert
            Assert.AreEqual(buildedRequestMessage.RequestUri, new Uri("http://any-uri.io/any-resource"));
            Assert.AreEqual(buildedRequestMessage.Method, HttpMethod.Get);
            Assert.IsTrue(buildedRequestMessage.Content == null);
            Assert.IsTrue(buildedRequestMessage.Headers.Authorization != null);
            
        }
     

        private static async Task<Stream> GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            await writer.WriteLineAsync(s).ConfigureAwait(false);
            await writer.FlushAsync();
            stream.Position = 0;

            return stream;
        }
    }
}
