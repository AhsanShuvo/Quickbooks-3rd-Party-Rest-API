using QuickbooksWeb.Interfaces;
using QuickbooksWeb.Models;
using QuickbooksDAL;

namespace QuickbooksWeb.ModelBuilder
{
    public class ApiModelToEntityModelBuilder : IApiModelToEntityModelBuilder
    {
        public AccountInfo GetAccountEntityModel(AccountModel model)
        {
            AccountInfo account = new AccountInfo()
            {
                Id = model.Id,
                Name = model.Name,
                AccountType = model.AccountType,
                Classification = model.Classification,
                CurrentBalance = model.CurrentBalance,
                SyncToken = model.SyncToken
            };
            return account;
        }

        public CompanyInfo GetCompanyEntityModel(CompanyModel model)
        {
            CompanyInfo company = new CompanyInfo()
            {
                Id = model.Id,
                CompanyName = model.CompanyName,
                CompanyStartDate = model.CompanyStartDate,
                SyncToken = model.SyncToken
            };
            return company;
        }

        public CustomerInfo GetCustomerEntityModel(CustomerModel model)
        {
            CustomerInfo customer = new CustomerInfo()
            {
                Id = model.Id,
                DisplayName = model.DisplayName,
                CompanyName = model.CompanyName,
                Balance = model.Balance,
                SyncToken = model.SyncToken,
                Active = model.Active
            };
            return customer;
        }

        public EmployeeInfo GetEmployeeEntityModel(EmployeeModel model)
        {
            EmployeeInfo employee = new EmployeeInfo()
            {
                Id = model.Id,
                GivenName = model.GivenName,
                FamilyName = model.FamilyName,
                DisplayName = model.DisplayName,
                Active = model.Active,
                SyncToken = model.SyncToken
            };
            return employee;
        }

        public ItemInfo GetItemEntityModel(ItemModel model)
        {
            ItemInfo item = new ItemInfo()
            {
                Id = model.Id,
                Name = model.Name,
                Type = model.Type,
                UnitPrice = model.UnitPrice,
                PurchaseCost = model.PurchaseCost,
                QtyOnHand = model.QtyOnHand,
                Active = model.Active,
                SyncToken = model.SyncToken
            };
            return item;
        }

        public ItemInfo GetCategoryEntityModel(ItemModel model)
        {
            ItemInfo category = new ItemInfo()
            {
                Id = model.Id,
                Name = model.Name,
                Type = model.Type,
                Active = model.Active,
                SyncToken = model.SyncToken
            };
            return category;
        }

        public PaymentInfo GetPaymentEntityModel(PaymentModel model)
        {
            PaymentInfo payment = new PaymentInfo()
            {
                Id = model.Id,
                TotalAmt = model.TotalAmt,
                SyncToken = model.SyncToken,
                CustomerRef = model.CustomerRef.value
            };
            return payment;
        }

        public VendorInfo GetVendorEntityModel(VendorModel model)
        {
            VendorInfo vendor = new VendorInfo()
            {
                Id = model.Id,
                DisplayName = model.DisplayName,
                CompanyName = model.CompanyName,
                Balance = model.Balance,
                SyncToken = model.SyncToken,
                Active = model.Active
            };
            return vendor;
        }
    }
}