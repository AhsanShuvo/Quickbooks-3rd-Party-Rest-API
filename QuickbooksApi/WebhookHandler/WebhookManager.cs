using Newtonsoft.Json;
using QuickbooksApi.Interfaces;
using QuickbooksApi.ModelBuilder;
using QuickbooksApi.Models;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace QuickbooksApi.Webhookhandler 
{
    public class WebhookManager : IWebhookManager
    {
        private IUserRepository _user;
        private IApiDataProvider _provider;
        private IAccountRepository _accountRepo;
        private ICompanyRepository _companyRepo;
        private ICustomerRepository _customerRepo;
        private IEmployeeRepository _employeeRepo;
        private IInvoiceRepository _invoiceRepo;
        private IItemRepository _itemRepo;
        private IPaymentRepository _paymentRepo;
        private IVendorRepository _vendorRepo;
        private IJsonToModelBuilder _builder;

        public WebhookManager(
            IUserRepository user, IApiDataProvider provider, IAccountRepository accountRepo,
            ICompanyRepository companyRepo, ICustomerRepository customerRepo, IEmployeeRepository employeeRepo,
            IInvoiceRepository invoiceRepo, IItemRepository itemRepo, IPaymentRepository paymentRepo,
            IVendorRepository vendorRepo, IJsonToModelBuilder builder)
        {
            _user = user;
            _provider = provider;
            _accountRepo = accountRepo;
            _companyRepo = companyRepo;
            _customerRepo = customerRepo;
            _employeeRepo = employeeRepo;
            _invoiceRepo = invoiceRepo;
            _itemRepo = itemRepo;
            _paymentRepo = paymentRepo;
            _vendorRepo = vendorRepo;
            _builder = builder;
        }

        private string _hashKey = ConfigurationManager.AppSettings["intuit-signature"];
        private string _qboBaseUrl = ConfigurationManager.AppSettings["baseUrl"];

        public async Task Init(string payload, string signature)
        {
            var webhookModel = JsonConvert.DeserializeObject<WebhookModel>(payload);
            bool res = VerifySignature(signature, payload);
            if(res == true)
            {
                await HandleEventNotifications(webhookModel);
            }
        }

        private bool VerifySignature(string signature, string payload)
        {
            if (signature == null) return false;
            try
            {
                byte[] key = Encoding.ASCII.GetBytes(_hashKey);
                HMACSHA256 myhmacsha256 = new HMACSHA256(key);
                byte[] byteArray = Encoding.ASCII.GetBytes(payload);
                using (MemoryStream stream = new MemoryStream(byteArray))
                {
                    var encodeData = Convert.ToBase64String(myhmacsha256.ComputeHash(stream));
                    if (encodeData.Equals(signature)) return true;
                    else return false;
                }
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        private async Task HandleEventNotifications(WebhookModel model)
        {
            foreach(EventNotification item in model.EventNotifications)
            {
                var realmId = item.RealmId;
                await HandleIndividualEvent(realmId, item.DataChangeEvent);
            }
        }

        private async Task HandleIndividualEvent(string realmId, DatachangeEvent model)
        {
            foreach(Entity item in model.Entities)
            {
                await ProcessEvent(realmId, item);
            }
        }

        private async Task ProcessEvent(string realmId, Entity model)
        {
            UserInfo user = _user.GetUserInfo(realmId);
            var id = WebUtility.UrlEncode(model.Id);
            var entityType = WebUtility.UrlEncode(model.Name.ToLower());
            string uri = string.Format("{0}/v3/company/{1}/{2}/{3}?minorversion=55", _qboBaseUrl, realmId,entityType, id);
            var data = await _provider.Get(uri, user.AccessToken);

            if(model.Name.Equals(EntityType.Account.ToString()))
            {
                if(model.Operation.Equals(Operations.Delete.ToString()))
                {
                    _accountRepo.DeleteAccountInfo(model.Id);
                }
                else
                {
                    AccountInfo account = _builder.GetAccountModel(data);
                    _accountRepo.SaveAccountInfo(account);
                }
            }
            else if(model.Name.Equals(EntityType.Company.ToString()))
            {
                CompanyInfo company = _builder.GetCompanyModel(data);
                _companyRepo.SaveCompanyDetails(company);
            }
            else if(model.Name.Equals(EntityType.Customer.ToString()))
            {
                if (model.Operation.Equals(Operations.Delete.ToString()))
                {
                    _customerRepo.DeleteCustomer(model.Id);
                }
                else
                {
                    CustomerInfo customer = _builder.GetCustomerModel(data);
                    _customerRepo.SaveCustomerDetails(customer);
                }
            }
            else if(model.Name.Equals(EntityType.Payment.ToString()))
            {
                if (model.Operation.Equals(Operations.Delete.ToString()))
                {
                    _paymentRepo.DeletePayment(model.Id);
                }
                else
                {
                    PaymentInfo payment = _builder.GetPaymentModel(data);
                    _paymentRepo.SavePaymentInfo(payment);
                }
            }
            else if(model.Name.Equals(EntityType.Vendor.ToString()))
            {
                if (model.Operation.Equals(Operations.Delete.ToString()))
                {
                    _vendorRepo.DeleteVendor(model.Id);
                }
                else
                {
                    VendorInfo vendor = _builder.GetVendorModel(data);
                    _vendorRepo.SaveVendorInfo(vendor);
                }
            }
            else if(model.Name.Equals(EntityType.Item.ToString()))
            {
                if (model.Operation.Equals(Operations.Delete.ToString()))
                {
                    _itemRepo.DeleteItem(model.Id);
                }
                else
                {
                    ItemInfo item = _builder.GetItemModel(data);
                    if(item.Type.Equals("Category"))
                    {
                        _itemRepo.SaveCategoryInfo(item);
                    }
                    else
                    {
                        _itemRepo.SaveItemInfo(item);
                    } 
                }
            }
            else if(model.Name.Equals(EntityType.Employee.ToString()))
            {
                if (model.Operation.Equals(Operations.Delete))
                {
                    _employeeRepo.DeleteEmployee(model.Id);
                }
                else
                {
                    EmployeeInfo employee = _builder.GetEmployeeModel(data);
                    _employeeRepo.SaveEmployeeInfo(employee);
                }
            }
            else if (model.Name.Equals(EntityType.Invoice.ToString()))
            {
                if (model.Operation.Equals(Operations.Delete))
                {
                    _invoiceRepo.DeleteInvoiceInfo(model.Id);
                }
                else
                {
                    InvoiceInfo invoice = _builder.GetInvoice(data);
                    _invoiceRepo.SaveInvoiceInfo(invoice);
                }
            }
        }
    }
}