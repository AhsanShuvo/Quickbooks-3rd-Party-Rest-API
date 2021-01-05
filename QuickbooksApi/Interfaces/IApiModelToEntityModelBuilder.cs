using QuickbooksApi.Models;

namespace QuickbooksApi.Interfaces
{
    public interface IApiModelToEntityModelBuilder
    {
        AccountInfo GetAccountEntityModel(AccountModel model);
        CompanyInfo GetCompanyEntityModel(CompanyModel model);
        CustomerInfo GetCustomerEntityModel(CustomerModel model);
        EmployeeInfo GetEmployeeEntityModel(EmployeeModel model);
        ItemInfo GetItemEntityModel(ItemModel model);
        ItemInfo GetCategoryEntityModel(ItemModel model);
        PaymentInfo GetPaymentEntityModel(PaymentModel model);
        VendorInfo GetVendorEntityModel(VendorModel model);
    }
}
