using Newtonsoft.Json;
using QuickbooksApi.Interfaces;
using QuickbooksApi.Models;

namespace QuickbooksApi.ModelBuilder
{
    public class JsonToModelBuilder : IJsonToModelBuilder
    {
        public CustomerModel GetCustomerModel(string json)
        {
            CustomerApiModel customerModel = JsonConvert.DeserializeObject<CustomerApiModel>(json);
            return customerModel.Customer;
        }

        public AccountModel GetAccountModel(string json)
        {
            AccountApiModel acctModel = JsonConvert.DeserializeObject<AccountApiModel>(json);
            return acctModel.Account;
        }

        public EmployeeModel GetEmployeeModel(string json)
        {
            EmployeeApiModel employeeModel = JsonConvert.DeserializeObject<EmployeeApiModel>(json);
            return employeeModel.Employee;
        }

        public CompanyModel GetCompanyModel(string json)
        {
            CompanyApiModel companyModel = JsonConvert.DeserializeObject<CompanyApiModel>(json);
            return companyModel.CompanyInfo;
        }

        public VendorModel GetVendorModel(string json)
        {
            VendorApiModel vendorModel = JsonConvert.DeserializeObject<VendorApiModel>(json);
            return vendorModel.Vendor;
        }

        public ItemModel GetItemModel(string json)
        {
            ItemApiModel itemModel = JsonConvert.DeserializeObject<ItemApiModel>(json);
            return itemModel.Item;
        }

        public PaymentModel GetPaymentModel(string json)
        {
            PaymentApiModel paymentModel = JsonConvert.DeserializeObject<PaymentApiModel>(json);
            return paymentModel.Payment;
        }

        public InvoiceModel GetInvoice(string json)
        {
            InvoiceApiModel invoiceModel = JsonConvert.DeserializeObject<InvoiceApiModel>(json);
            return invoiceModel.Invoice;
        }
    }
}