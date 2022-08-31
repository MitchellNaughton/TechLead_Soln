using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaxManWeb.Models
{
    public class SalaryReturnModel
    {
        public decimal dcGrossSal { get; set; }
        public decimal dcGrossTaxPd { get; set; }
        public decimal dcNetSal { get; set; }
        public decimal dcGrossMonthlySal { get; set; }
        public decimal dcGrossMonthlyTaxPd { get; set; }
        public decimal dcNetMonthlySal { get; set; } 
    }
}