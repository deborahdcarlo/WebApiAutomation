using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Serialization.Json;

namespace WebApiAutomation
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var client = new RestClient("http://localhost:3000/");

            var request = new RestRequest("auth/login", Method.POST);

            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(new
            {
                email = "bruno@email.com",
                password = "bruno"
            });

            var response = client.Execute(request);

            JObject obs = JObject.Parse(response.Content);
            var result = obs["access_token"];

            var request2 = new RestRequest("products/{productid}", Method.GET);
            request2.AddHeader("Authorization", "Bearer " + result.ToString());
            request2.AddUrlSegment("productid", 3);

            var response2 = client.Execute(request2);

            //var deserialize = new JsonDeserializer();
            //var output = deserialize.Deserialize<Dictionary<string, string>>(response);
            //var result = output["name"];


            JObject obs2 = JObject.Parse(response2.Content);
            var result2 = obs2["name"];
        }
    }
}
