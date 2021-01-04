using QuickbooksApi.Models;

namespace QuickbooksApi.Interfaces
{
    public interface IJsonToModelBuilder
    {
        CustomerInfo GetCustomerModel(string json);
        AccountInfo GetAccountModel(string json);
        EmployeeInfo GetEmployeeModel(string json);
        CompanyInfo GetCompanyModel(string json);
        VendorInfo GetVendorModel(string json);
        ItemInfo GetItemModel(string json);
        PaymentInfo GetPaymentModel(string json);
        InvoiceInfo GetInvoice(string json);
    }
}
