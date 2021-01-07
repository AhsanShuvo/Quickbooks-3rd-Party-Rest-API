namespace QuickbooksDAL.Interfaces
{
    public interface IInvoiceRepository
    {
        void SaveInvoiceInfo(InvoiceInfo model);
        void DeleteInvoiceInfo(string id);
    }
}
