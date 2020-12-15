using Newtonsoft.Json;
using QuickbooksApi.ApiService;
using QuickbooksApi.ModelBuilder;
using QuickbooksApi.Models;
using QuickbooksApi.Repository;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace QuickbooksApi.Webhookhandler
{
    public class WebhookManager
    {
        private string _hashKey = ConfigurationManager.AppSettings["intuit-signature"];
        private string _qboBaseUrl = ConfigurationManager.AppSettings["baseUrl"];
        private UserRepository _user = new UserRepository();
        private ApiDataProvider _provider = new ApiDataProvider();
        private JsonToModelBuilder _builder = new JsonToModelBuilder();

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
                var _repo = new AccountRepository();
                if(model.Operation.Equals(Operations.Delete.ToString()))
                {
                    _repo.DeleteAccountInfo(model.Id);
                }
                else
                {
                    AccountInfo account = _builder.GetAccountModel(data);
                    _repo.SaveAccountInfo(account);
                }
            }
            else if(model.Name.Equals(EntityType.Company.ToString()))
            {
                var _repo = new CompanyRepository();
                CompanyInfo company = _builder.GetCompanyModel(data);
                _repo.SaveCompanyDetails(company);
            }
            else if(model.Name.Equals(EntityType.Customer.ToString()))
            {
                var _repo = new CustomerRepository();
                if (model.Operation.Equals(Operations.Delete.ToString()))
                {
                    _repo.DeleteCustomer(model.Id);
                }
                else
                {
                    CustomerInfo customer = _builder.GetCustomerModel(data);
                    _repo.SaveCustomerDetails(customer);
                }
            }
            else if(model.Name.Equals(EntityType.Payment.ToString()))
            {
                var _repo = new PaymentRepository();
                if (model.Operation.Equals(Operations.Delete.ToString()))
                {
                    _repo.DeletePayment(model.Id);
                }
                else
                {
                    PaymentInfo payment = _builder.GetPaymentModel(data);
                    _repo.SavePaymentInfo(payment);
                }
            }
            else if(model.Name.Equals(EntityType.Vendor.ToString()))
            {
                var _repo = new VendorRepository();
                if (model.Operation.Equals(Operations.Delete.ToString()))
                {
                    _repo.DeleteVendor(model.Id);
                }
                else
                {
                    VendorInfo vendor = _builder.GetVendorModel(data);
                    _repo.SaveVendorInfo(vendor);
                }
            }
            else if(model.Name.Equals(EntityType.Item.ToString())) // Need to handle Item or category manually
            {
                var _repo = new ItemRepository();
                if (model.Operation.Equals(Operations.Delete.ToString()))
                {
                    _repo.DeleteItem(model.Id);
                }
                else
                {
                    ItemInfo item = _builder.GetItemModel(data);
                    _repo.SaveItemInfo(item);
                }
            }
            else if(model.Name.Equals(EntityType.Employee.ToString()))
            {
                var _repo = new EmployeeRepository();
                if (model.Operation.Equals(Operations.Delete))
                {
                    _repo.DeleteEmployee(model.Id);
                }
                else
                {
                    EmployeeInfo employee = _builder.GetEmployeeModel(data);
                    _repo.SaveEmployeeInfo(employee);
                }
            }
            else if (model.Name.Equals(EntityType.Invoice.ToString()))
            {
                var _repo = new InvoiceRepository();
                if (model.Operation.Equals(Operations.Delete))
                {
                    _repo.DeleteInvoiceInfo(model.Id);
                }
                else
                {
                    InvoiceInfo invoice = _builder.GetInvoice(data);
                    _repo.SaveInvoiceInfo(invoice);
                }
            }
        }
    }
}