using Newtonsoft.Json;
using QuickbooksApi.Models;
using System.Collections.Generic;

namespace QuickbooksApi.ModelBuilder
{
    public class JsonToModelBuilder
    {
        public CustomerInfo GetCustomerModel(string modelJson)
        {
           
            dynamic obj = JsonConvert.DeserializeObject(modelJson);
            dynamic model = obj.Customer;
            CustomerInfo customer = new CustomerInfo()
            {
                Id = model.Id,
                DisplayName = model.DisplayName,
                CompanyName = model.CompanyName,
                Balance = model.Balance,
                Active = model.Active,
                SyncToken = model.SyncToken
            };
            return customer;
        }

        public AccountInfo GetAccountModel(string modelJson)
        {
            dynamic obj = JsonConvert.DeserializeObject(modelJson);
            dynamic model = obj.Account;
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

        public EmployeeInfo GetEmployeeModel(string modelJson)
        {
            dynamic obj = JsonConvert.DeserializeObject(modelJson);
            dynamic model = obj.Employee;
            EmployeeInfo employee = new EmployeeInfo()
            {
                Id = model.Id,
                GivenName = model.GivenName,
                DisplayName = model.DisplayName,
                Active = model.Active,
                SyncToken = model.SyncToken
            };
                return employee;
        }

        public CompanyInfo GetCompanyModel(string modelJson)
        {
            dynamic obj = JsonConvert.DeserializeObject(modelJson);
            dynamic model = obj.CompanyInfo;
            CompanyInfo company = new CompanyInfo()
            {
                Id = model.Id,
                CompanyName = model.CompanyName,
                CompanyStartDate = model.CompanyStartDate,
                SyncToken = model.SyncToken
            };
            return company;
        }

        public VendorInfo GetVendorModel(string modelJson)
        {
            dynamic obj = JsonConvert.DeserializeObject(modelJson);
            dynamic model = obj.Vendor;
            VendorInfo vendor = new VendorInfo()
            {
                Id = model.Id,
                DisplayName = model.DisplayName,
                CompanyName = model.CompanyName,
                Active = model.Active,
                Balance = model.Balance,
                SyncToken = model.SyncToken
            };
            return vendor;
        }

        public ItemInfo GetItemModel(string modelJson)
        {
            dynamic obj = JsonConvert.DeserializeObject(modelJson);
            dynamic model = obj.Item;

            ItemInfo item = new ItemInfo()
            {
                Id = model.Id,
                Name = model.Name,
                Type = model.Type,
                Active = model.Active,
                SyncToken = model.SyncToken,
                QtyOnHand = model.Type == "Service" ? 0 : model.QtyOnHand,
                UnitPrice = model.UnitPrice,
                PurchaseCost = model.PurchaseCost
            };
            return item;
        }

        public CategoryInfo GetCategoryModel(string modelJson)
        {
            dynamic obj = JsonConvert.DeserializeObject(modelJson);
            dynamic model = obj.Item;

            CategoryInfo category = new CategoryInfo()
            {
                Id = model.Id,
                Name = model.Name,
                Type = model.Type,
                Active = model.Active,
                SyncToken = model.SyncToken
            };
            return category;

        }

        public PaymentInfo GetPaymentModel(string modelJson)
        {
            dynamic obj = JsonConvert.DeserializeObject(modelJson);
            dynamic model = obj.Payment;
            dynamic customer = model.CustomerRef;

            CustomerReference customerRef = new CustomerReference()
            {
                name = customer.name,
                value = customer.value
            };

            PaymentInfo payment = new PaymentInfo()
            {
                Id = model.Id,
                TotalAmt = model.TotalAmt,
                SyncToken = model.SyncToken,
                CustomerRef = customerRef
            };

            return payment;
        }

        public InvoiceInfo GetInvoice(string modelJson)
        {
            dynamic obj = JsonConvert.DeserializeObject(modelJson);
            dynamic model = obj.Invoice;
            List<LineRef> Lines = new List<LineRef>();
            var salesItems = model.Line;
            foreach(var item in salesItems)
            {
                LineRef line = new LineRef()
                {
                    Description = item.Description,
                    DetailType = item.DetailType,
                    Qty = item.Qty,
                    UnitPrice = item.UnitPrice,
                    Amount = item.Amount
                };
                Lines.Add(line);
            }

            CustomerReference customerRef = new CustomerReference()
            {
                value = model.CustomerRef.value
            };

            InvoiceInfo invoice = new InvoiceInfo()
            {
                Id = model.Id,
                TotalAmt = model.TotalAmt,
                SyncToken = model.SyncToken,
                TxnDate = model.TxnDate,
                CustomerRef = customerRef,
                Line = Lines
            };
            return invoice;
        }
    }
}