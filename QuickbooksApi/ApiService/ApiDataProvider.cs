using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace QuickbooksApi.ApiService
{
    public class ApiDataProvider
    {
        public async Task<string> Get(string uri, string token)
        {
            string responseAsString = string.Empty;
            try
            {
                using(HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                    client.DefaultRequestHeaders.Add("ContentType", "application/json");
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    var response = await client.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        responseAsString = await response.Content.ReadAsStringAsync();
                    }
                }
            }
            catch
            {

            }
            
            return responseAsString;
        }

        public async Task<string> Post(string uri, StringContent requestBody, string token)
        {
            string resposeAsString = string.Empty;

            try
            {
                using(HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                    client.DefaultRequestHeaders.Add("ContentType", "application/json");
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    HttpResponseMessage response = await client.PostAsync(uri, requestBody);
                    if (response.IsSuccessStatusCode)
                    {
                        resposeAsString = await response.Content.ReadAsStringAsync();

                    }
                }
            }
            catch
            {

            }
            
            return resposeAsString;
        }

        public async Task GetPDF(string uri, string token)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("ContentType", "application/octet-stream");
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    byte[] data = await client.GetByteArrayAsync(uri);
                    var gen = new Random();
                    var number = gen.Next();
                    var fileName = "D:\\MypdfFile" + number + ".pdf";
                    if (!File.Exists(fileName))
                    {
                        using (FileStream stream = File.Create(fileName))
                        {
                            stream.Write(data, 0, data.Length);
                        }
                    }
                }
            }
            catch
            {

            }
        }
    }
}