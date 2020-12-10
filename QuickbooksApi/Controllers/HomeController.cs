using Newtonsoft.Json;
using QuickbooksApi.Models;
using QuickbooksApi.Repository;
using System.Web.Mvc;

namespace QuickbooksApi.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

    }
}   