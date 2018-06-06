using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RestSharp;
using Newtonsoft.Json;
using System.Security.Cryptography;

namespace TesteApiBodytechCompany
{
    class Program
    {
        static void Main(string[] args)
        {
            RestClient client;
            RestRequest request;
            UserToken userToken;

            string user="admin";
            string password = "admin";

            // Generate user key to Request Token
            Encoding encoding = UTF8Encoding.UTF8;
            byte[] textAsBytes = encoding.GetBytes(user + ":" + password);
            string keyToRequestToken = Convert.ToBase64String(textAsBytes);



            // Request token by user key
            {
                client = new RestClient("http://ec2-52-2-62-115.compute-1.amazonaws.com:5000/api/v1/auth/");
                request = new RestRequest(Method.GET);
                request.AddHeader("Postman-Token", "5d85c5d7-4973-48f5-98a7-c16e397f7b2e");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Authorization", "Basic " + keyToRequestToken);
                IRestResponse response = client.Execute(request);

                string content = response.Content;
                content = content.Substring(1, content.Length - 1 - 1 - 1);
                content = content.Replace(@"""data"": ", "");
                userToken = JsonConvert.DeserializeObject<UserToken>(content);
            }


            // Request customers
            {
                client = new RestClient("http://ec2-52-2-62-115.compute-1.amazonaws.com:5000/api/v1/customers/tmk/BT/mgm/excluded/2018-03-01");
                request = new RestRequest(Method.GET);
                request.AddHeader("Postman-Token", "8a75efff-bb39-439f-a5a2-9bed786e67c8");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("x-access-token", userToken.token);
                IRestResponse response = client.Execute(request);


                // Write result
                Console.Write(response.Content);
            }
        }
    }

    class UserToken
    {
        public string token { get; set; }
    }

}
