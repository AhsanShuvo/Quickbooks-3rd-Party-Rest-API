using QuickbooksApi.Models;

namespace QuickbooksApi.Interfaces
{
    public interface IInvoiceRepository
    {
        void SaveInvoiceInfo(InvoiceInfo model);
        void DeleteInvoiceInfo(string id);
    }
}
