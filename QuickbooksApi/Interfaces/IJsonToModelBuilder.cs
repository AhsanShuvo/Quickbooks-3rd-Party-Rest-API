using QuickbooksWeb.Models;

namespace QuickbooksWeb.Interfaces
{
    public interface IJsonToModelBuilder
    {
        CustomerModel GetCustomerModel(string json);
        AccountModel GetAccountModel(string json);
        EmployeeModel GetEmployeeModel(string json);
        CompanyModel GetCompanyModel(string json);
        VendorModel GetVendorModel(string json);
        ItemModel GetItemModel(string json);
        PaymentModel GetPaymentModel(string json);
        InvoiceModel GetInvoice(string json);
    }
}
