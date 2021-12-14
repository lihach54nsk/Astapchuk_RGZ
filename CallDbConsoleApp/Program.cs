using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CallDbConsoleApp
{
    class Program
    {
        private const string Url = "https://localhost:44383/Cache";
        private static readonly HttpClient httpClient = new HttpClient();

        private static void Main(string[] args)
        {
            MakeCacheRequest().Wait();
        }

        private static async Task MakeCacheRequest()
        {
            int i = 0;

            while (true)
            {
                var content = new FormUrlEncodedContent(new Dictionary<string, string>());

                var postResponse = await httpClient.PostAsync(Url + $"?data=data{i}", content);

                var key = await postResponse.Content.ReadAsStringAsync();
                i = Convert.ToInt32(key);
                Console.WriteLine($"Value data{i} has written with key {key}");

                Thread.Sleep(5000);

                var getResponse = await httpClient.GetAsync(Url + $"?id={i}");

                Console.WriteLine($"Data that are written by key {i} is {await getResponse.Content.ReadAsStringAsync()}");

                i++;
                Thread.Sleep(5000);
            }
        }
    }
}
