using Intuit.Ipp.OAuth2PlatformClient;
using QuickbooksApi.Helper;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace QuickbooksApi.Controllers
{
    public class AppController : Controller
    {
        public static string clientId = ConfigurationManager.AppSettings["clientId"];
        public static string clientsecret = ConfigurationManager.AppSettings["clientsecret"];
        public static string redirectUrl = ConfigurationManager.AppSettings["redirectUrl"];
        public static string environment = ConfigurationManager.AppSettings["appEnvironment"];

        public static OAuth2Client auth2Client = new OAuth2Client(clientId, clientsecret, redirectUrl, environment);

        public ActionResult Index()
        {
            Logger.WriteDebug("Starting application");
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            Session.Clear();
            Session.Abandon();
            Request.GetOwinContext().Authentication.SignOut("Cookies");
            return View();
        }

        public ActionResult InitiateAuth(string submitButton)
        {
            switch (submitButton)
            {
                case "Connect to QuickBooks":
                    List<OidcScopes> scopes = new List<OidcScopes>();
                    scopes.Add(OidcScopes.Accounting);
                    string authorizeUrl = auth2Client.GetAuthorizationURL(scopes);
                    return Redirect(authorizeUrl);
                default:
                    return (View());
            }
        }

        public ActionResult Error()
        {
            Logger.WriteDebug("Something wrong happened. Failed to start application.");
            return View("Error");
        }
    }
}