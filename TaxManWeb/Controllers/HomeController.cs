using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Net.Http;
using System.Configuration;
using TaxManWeb.Models;

namespace TaxManWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(decimal ipiSalary)
        {
            
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["APILocation"]);
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("", ipiSalary.ToString())
                });
                var responseTask = client.PostAsync("Tax", content);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    var readJob = result.Content.ReadAsAsync<SalaryReturnModel>();
                    readJob.Wait();
                    SalaryReturnModel returnVals = readJob.Result;

                    returnVals.dcGrossMonthlySal = returnVals.dcGrossSal / 12;
                    returnVals.dcGrossMonthlyTaxPd = returnVals.dcGrossTaxPd / 12;
                    returnVals.dcNetMonthlySal = returnVals.dcNetSal / 12;

                    return View(returnVals);
                } else
                {
                    ModelState.AddModelError(string.Empty, "Error: DAC004");
                    return View();
                }
            }
        }
    }
}