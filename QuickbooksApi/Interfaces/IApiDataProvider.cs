using System.Net.Http;
using System.Threading.Tasks;

namespace QuickbooksApi.Interfaces
{
    public interface IApiDataProvider
    {
        Task<string> Get(string uri, string token);
        Task<string> Post(string uri, StringContent requestBody, string token);
        Task GetPDF(string uri, string token);
    }
}
