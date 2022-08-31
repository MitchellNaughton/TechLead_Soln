using System.Web.Http;
using TaxManDL.Models;
using TaxManDL.bo;

namespace TaxManAPI.Controllers
{
    public class TaxController : ApiController
    {
        // POST: api/Tax
        public SalaryReturnModel Post([FromBody] string ipsSalary)
        {
            decimal iSalary = decimal.Parse(ipsSalary); 
            SalaryReturnModel oReturn = TaxManBo.GetSalaryReturn(iSalary, "");
            return oReturn;
        }
    }
}
