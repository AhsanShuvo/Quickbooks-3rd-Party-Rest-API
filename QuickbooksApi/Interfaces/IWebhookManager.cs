using System.Threading.Tasks;

namespace QuickbooksWeb.Interfaces
{
    public interface IWebhookManager
    {
        Task Init(string payload, string signature);
    }
}
