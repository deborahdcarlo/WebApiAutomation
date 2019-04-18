using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Serialization.Json;

namespace WebApiAutomation
{
    [TestClass]
    public class UnitTest1
    {
        private static JToken token;
        private static RestClient client;

        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            client = new RestClient("http://localhost:3000/");

            var request = new RestRequest("auth/login", Method.POST);

            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(new
            {
                email = "bruno@email.com",
                password = "bruno"
            });

            var response = client.Execute(request);
            JObject obs = JObject.Parse(response.Content);
            token = obs["access_token"];
            
        }

        [TestMethod]
        public void TestMethod1()
        {                                              
            var request = new RestRequest("locations/{locationid}", Method.GET);

            var authentication = new JwtAuthenticator(token.ToString());
            authentication.Authenticate(client, request);

            //request2.AddHeader("Authorization", "Bearer " + result.ToString());
            request.AddUrlSegment("locationid", 2);

            var response = client.Execute(request);

            //var deserialize = new JsonDeserializer();
            //var output = deserialize.Deserialize<Dictionary<string, string>>(response);
            //var result = output["name"];

            JObject obj = JObject.Parse(response.Content);
            var result = obj["name"];

            Assert.IsTrue(result.ToString() == "Location002");
        }

        [TestMethod]
        public void TestMethod2()
        {
            var request = new RestRequest("products/{productid}", Method.GET);

            var authentication = new JwtAuthenticator(token.ToString());
            authentication.Authenticate(client, request);

            //request2.AddHeader("Authorization", "Bearer " + result.ToString());
            request.AddUrlSegment("productid", 3);

            var response = client.Execute(request);

            //var deserialize = new JsonDeserializer();
            //var output = deserialize.Deserialize<Dictionary<string, string>>(response);
            //var result = output["name"];

            JObject obj = JObject.Parse(response.Content);
            var result = obj["name"];

            Assert.IsTrue(result.ToString() == "Product003");
        }
    }
}
