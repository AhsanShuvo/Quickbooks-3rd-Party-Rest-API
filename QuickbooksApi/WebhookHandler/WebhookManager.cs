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
        private IApiModelToEntityModelBuilder _entityBuilder;

        public WebhookManager(
            IUserRepository user, IApiDataProvider provider, IAccountRepository accountRepo,
            ICompanyRepository companyRepo, ICustomerRepository customerRepo, IEmployeeRepository employeeRepo,
            IInvoiceRepository invoiceRepo, IItemRepository itemRepo, IPaymentRepository paymentRepo,
            IVendorRepository vendorRepo, IJsonToModelBuilder builder, IApiModelToEntityModelBuilder entityBuilder)
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
            _entityBuilder = entityBuilder;
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
                    AccountModel accountModel = _builder.GetAccountModel(data);
                    var accountEntityModel = _entityBuilder.GetAccountEntityModel(accountModel);
                    _accountRepo.SaveAccountInfo(accountEntityModel);
                }
            }
            else if(model.Name.Equals(EntityType.Company.ToString()))
            {
                CompanyModel companyModel = _builder.GetCompanyModel(data);
                var companyEntityModel = _entityBuilder.GetCompanyEntityModel(companyModel);
                _companyRepo.SaveCompanyDetails(companyEntityModel);
            }
            else if(model.Name.Equals(EntityType.Customer.ToString()))
            {
                if (model.Operation.Equals(Operations.Delete.ToString()))
                {
                    _customerRepo.DeleteCustomer(model.Id);
                }
                else
                {
                    var customerModel = _builder.GetCustomerModel(data);
                    var customerEntityModel = _entityBuilder.GetCustomerEntityModel(customerModel);
                    _customerRepo.SaveCustomerDetails(customerEntityModel);
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
                    var paymentModel = _builder.GetPaymentModel(data);
                    var paymentEntityModel = _entityBuilder.GetPaymentEntityModel(paymentModel);
                    _paymentRepo.SavePaymentInfo(paymentEntityModel);
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
                    var vendorModel = _builder.GetVendorModel(data);
                    var vendorEntityModel = _entityBuilder.GetVendorEntityModel(vendorModel);
                    _vendorRepo.SaveVendorInfo(vendorEntityModel);
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
                    var item = _builder.GetItemModel(data);
                    if(item.Type.Equals("Category"))
                    {
                        var categoryEntityModel = _entityBuilder.GetCategoryEntityModel(item);
                        _itemRepo.SaveCategoryInfo(categoryEntityModel);
                    }
                    else
                    {
                        var itemEntityModel = _entityBuilder.GetItemEntityModel(item);
                        _itemRepo.SaveItemInfo(itemEntityModel);
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
                    var employee = _builder.GetEmployeeModel(data);
                    var employeeEntityModel = _entityBuilder.GetEmployeeEntityModel(employee);
                    _employeeRepo.SaveEmployeeInfo(employeeEntityModel);
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
                    var invoice = _builder.GetInvoice(data);
                    //var invoiceEntityModel = _entityBuilder.GetInvoiceEntityModel(invoice);
                    //_invoiceRepo.SaveInvoiceInfo(invoiceentityModel);
                }
            }
        }
    }
}