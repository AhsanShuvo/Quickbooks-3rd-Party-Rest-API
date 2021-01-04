using System.Threading.Tasks;

namespace QuickbooksApi.Interfaces
{
    public interface IWebhookManager
    {
        Task Init(string payload, string signature);
    }
}
