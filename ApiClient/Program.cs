﻿using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApiClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Task.Run(async () =>
            {
                // discover endpoints from metadata
                var disco = await DiscoveryClient.GetAsync("http://localhost:5000");

                // request token
                // var tokenClient = new TokenClient(disco.TokenEndpoint, "client", "secret");
                // var tokenResponse = await tokenClient.RequestClientCredentialsAsync("api1");

                // 资源所有者密码
                // request token
                var tokenClient = new TokenClient(disco.TokenEndpoint, "ro.client", "secret");
                var tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync("524246918@qq.com", "Admin12345.", "api1");

                if (tokenResponse.IsError)
                {
                    Console.WriteLine(tokenResponse.Error);
                    return;
                }

                Console.WriteLine(tokenResponse.Json);

                // call api
                var client = new HttpClient();
                client.SetBearerToken(tokenResponse.AccessToken);

                var response = await client.GetAsync("http://localhost:5001/identity");
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine(response.StatusCode);
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(JArray.Parse(content));
                }
            }).Wait();
        }
    }
}
