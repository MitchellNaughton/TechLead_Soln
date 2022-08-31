using System.Web.Mvc;
using TaxManDL.Models;
using TaxManDL.bo;

namespace TaxManAPI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public ActionResult New()
        {
            ViewBag.Title = "New tax set up";
            TaxValuesModel initial = new TaxValuesModel()
            {
                dcTaxInc1 = 0m,
                dcTaxInc2 = 0m,
                dcTaxInc3 = 0m,
                dcTaxRate1 = 0m,
                dcTaxRate2 = 0m,
                dcTaxRate3 = 0m
            };

            return View(initial);
        }

        [HttpPost]
        public ActionResult New(TaxValuesModel ipoTaxVal, string ipsCountry)
        {
            ViewBag.Title = "New set up created";

            TaxManBo.CreateTaxSetUp(ipoTaxVal, ipsCountry);
            TaxValuesModel initial = new TaxValuesModel()
            {
                dcTaxInc1 = 0m,
                dcTaxInc2 = 0m,
                dcTaxInc3 = 0m,
                dcTaxRate1 = 0m,
                dcTaxRate2 = 0m,
                dcTaxRate3 = 0m
            };

            return View(initial);
        }
    }
}
