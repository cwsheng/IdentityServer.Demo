using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;

namespace ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");
            //客户端模式
            ClientCredentials_Test();
            //用户名密码
            //ResourceOwnerPassword_Test();

            Console.WriteLine();
        }


        /// <summary>
        /// 客户端认证模式
        /// </summary>
        private static void ClientCredentials_Test()
        {
            Console.WriteLine("ClientCredentials_Test------------------->");
            var client = new HttpClient();
            var disco = client.GetDiscoveryDocumentAsync("http://localhost:5600/").Result;
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return;
            }
            // request token
            var tokenResponse = client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "credentials_client",
                ClientSecret = "secret",
                Scope = "goods"
            }).Result;

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            Console.WriteLine("Token结果：" + tokenResponse.Json);
            // call api
            var apiClient = new HttpClient();
            apiClient.SetBearerToken(tokenResponse.AccessToken);

            var response = apiClient.GetAsync("http://localhost:5601/identity").Result;
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine("API调用结果：" + content);
            }
        }

        /// <summary>
        /// 用户名密码模式
        /// </summary>
        public static void ResourceOwnerPassword_Test()
        {
            Console.WriteLine("ResourceOwnerPassword_Test------------------->");
            // request token
            var client = new HttpClient();
            var disco = client.GetDiscoveryDocumentAsync("http://localhost:5600/").Result;
            var tokenResponse = client.RequestPasswordTokenAsync(new PasswordTokenRequest()
            {
                Address = disco.TokenEndpoint,
                ClientId = "password_client",
                ClientSecret = "secret",
                UserName = "cba",
                Password = "cba",
                Scope = "order goods",
            }).Result;

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }
            Console.WriteLine(tokenResponse.Json);
            // call api
            var apiClient = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);
            var response = apiClient.GetAsync("http://localhost:5601/identity").Result;
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(content);
            }
        }

    }
}
