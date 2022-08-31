using System.Web.Mvc;

namespace TaxManAPI.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Oops()
        {
            return View();
        }
    }
}
