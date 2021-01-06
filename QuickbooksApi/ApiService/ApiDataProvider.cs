using QuickbooksApi.Helper;
using QuickbooksApi.Interfaces;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace QuickbooksApi.ApiService
{
    public class ApiDataProvider : IApiDataProvider
    {
        public async Task<string> Get(string uri, string token)
        {
            string responseAsString = string.Empty;
            try
            {
                Logger.WriteDebug("Get api request to quickbooks server to retrieve data.");
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                    client.DefaultRequestHeaders.Add("ContentType", "application/json");
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    var response = await client.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        Logger.WriteDebug("Api request is successfull");
                        responseAsString = await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        Logger.WriteDebug("Api request failed. Status code: " + response.StatusCode + "");
                        throw new HttpRequestException($"Response status does not indicate success: {(int)response.StatusCode} ({response.StatusCode}).");
                    }
                }
            }
            catch(Exception e)
            {
                Logger.WriteError(e, "Failed to connect to quickbooks server.");
                throw e;
            }
            
            return responseAsString;
        }

        public async Task<string> Post(string uri, StringContent requestBody, string token)
        {
            string resposeAsString = string.Empty;

            try
            {
                Logger.WriteDebug("Post api request to quickbooks server to modify data.");
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                    client.DefaultRequestHeaders.Add("ContentType", "application/json");
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    HttpResponseMessage response = await client.PostAsync(uri, requestBody);
                    if (response.IsSuccessStatusCode)
                    {
                        Logger.WriteDebug("Api request is successfull");
                        resposeAsString = await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        Logger.WriteDebug("Api request failed. Status code: " + response.StatusCode + "");
                        throw new HttpRequestException($"Response status does not indicate success: {(int)response.StatusCode} ({response.StatusCode}).");
                    }
                }
            }
            catch(Exception e)
            {
                Logger.WriteError(e, "Failed to connect to quickbooks server.");
                throw e;
            }
            
            return resposeAsString;
        }

        public async Task GetPDF(string uri, string token)
        {
            try
            {
                Logger.WriteDebug("Api request to download pdf file.");
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
            catch(Exception e)
            {
                Logger.WriteError(e, "Failed to download pdf file.");
                throw e;
            }
        }
    }
}