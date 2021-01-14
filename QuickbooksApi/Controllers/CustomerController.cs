using QuickbooksAPI.Interfaces;
using QuickbooksCommon.Logger;
using QuickbooksDAL;
using QuickbooksDAL.Interfaces;
using Newtonsoft.Json;
using QuickbooksWeb.Interfaces;
using QuickbooksWeb.Models;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using System.Web.UI.WebControls;
using Quickbooks.Report.ReportProvider;
using Quickbooks.Report.Interfaces;

namespace QuickbooksWeb.Controllers
{
    public class CustomerController : BaseController
    {
        private ICustomerRepository _repository;
        private ICustomerBalanceReportProvider _rprtProvider;

        public CustomerController(
            ICustomerRepository repository, IApiDataProvider provider,
            IJsonToModelBuilder builder, IApiModelToEntityModelBuilder entityBuilder,
            ICustomerBalanceReportProvider rprtProvider): base(provider, builder, entityBuilder)
        {
            _repository = repository;
            _rprtProvider = rprtProvider;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateCustomer()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateCustomer(CustomerModel model)
        {
            Logger.WriteDebug("Creating new customer");
            var requestBody = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var customerObject = await HandlePostRequest(requestBody, EntityType.Customer.ToString().ToLower());
            CustomerModel customerModel = _builder.GetCustomerModel(customerObject);
            CustomerInfo customerEntityModel = _entityBuilder.GetCustomerEntityModel(customerModel);
            _repository.SaveCustomerDetails(customerEntityModel);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> CustomerDetails()
        {
            var customerObject = await HandleGetRequest("68", EntityType.Customer.ToString().ToLower());
            var customerModel = _builder.GetCustomerModel(customerObject);
            var customerEntityModel = _entityBuilder.GetCustomerEntityModel(customerModel);
            _repository.SaveCustomerDetails(customerEntityModel);
            return View(customerModel);
        }

        public ActionResult UpdateCustomer()
        {
            var model = _repository.GetCustomerInfo("2");
            return View(model);
        }

        public async Task<ActionResult> UpdateCustomer(CustomerInfo model)
        {
            var requestBody = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var customerObj = await HandlePostRequest(requestBody, EntityType.Customer.ToString().ToLower());
            var customerModel = _builder.GetCustomerModel(customerObj);
            var customerEntityModel = _entityBuilder.GetCustomerEntityModel(customerModel);
            _repository.SaveCustomerDetails(customerEntityModel);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> CustomerReport()
        {
            var realmId = GetRealmId();
            var accessToken = GetAccessToken();
            var id = Url.Encode("1");
            var entity = Url.Encode("CustomerBalance");
            var empData = await _rprtProvider.GetCustomerBalance(_qboBaseUrl, entity, realmId, accessToken, id);
            
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;
            reportViewer.Width = Unit.Percentage(100);
            reportViewer.Height = Unit.Percentage(100);

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"bin\Reports\CustomerBalance.rdlc";
            ReportDataSource data = new ReportDataSource("DataSet1", empData);
            reportViewer.LocalReport.DataSources.Add(data);
            ViewBag.ReportViewer = reportViewer;
            return View();
        }
    }
}