using System.Configuration;
using TaxManDL.Models;
using TaxManDL.bo;
using Xunit;

namespace TaxManTests.TaxManDL
{
    public class TaxManBoTest
    {
        [Fact]
        public void GetSGetSingleSelect_Test()
        {
            string sExpected = "select iUid from TMDB.dbo.tmCountry where "
                            + "TMDB.dbo.tmCountry.cCountry = testCountry";
            string sActual = TaxManBo.SelectSingleQuery("iUid", 
                                                      "TMDB.dbo.tmCountry", 
                                                      true, 
                                                      "TMDB.dbo.tmCountry.cCountry = testCountry");

            Assert.Equal(sExpected, sActual);
        }

        [Fact]
        public void CalculateSalaryReturn_Test1()
        {
            SalaryReturnModel oExpecteds = new SalaryReturnModel
            {
                dcGrossSal   = decimal.Parse(ConfigurationManager.AppSettings["Test.Gross.Salary.1"]),
                dcNetSal     = decimal.Parse(ConfigurationManager.AppSettings["Test.Net.Salary.1"]),
                dcGrossTaxPd = decimal.Parse(ConfigurationManager.AppSettings["Test.Gross.Tax.1"])
            };
            SalaryReturnModel oInputs = new SalaryReturnModel
            {
                dcGrossSal = oExpecteds.dcGrossSal
            };
            TaxValuesModel oTaxVal = new TaxValuesModel
            {
                dcTaxInc1  = decimal.Parse(ConfigurationManager.AppSettings["Test.Inc.1"]),
                dcTaxInc2  = decimal.Parse(ConfigurationManager.AppSettings["Test.Inc.2"]),
                dcTaxInc3  = decimal.Parse(ConfigurationManager.AppSettings["Test.Inc.3"]),
                dcTaxRate1 = decimal.Parse(ConfigurationManager.AppSettings["Test.Rate.1"]),
                dcTaxRate2 = decimal.Parse(ConfigurationManager.AppSettings["Test.Rate.2"]),
                dcTaxRate3 = decimal.Parse(ConfigurationManager.AppSettings["Test.Rate.3"])
            };

            SalaryReturnModel oActuals = TaxManBo.CalculateSalaryReturn(oTaxVal, oInputs);

            Assert.Equal(oExpecteds.dcGrossSal, oActuals.dcGrossSal);
            Assert.Equal(oExpecteds.dcNetSal, oActuals.dcNetSal);
            Assert.Equal(oExpecteds.dcGrossTaxPd, oActuals.dcGrossTaxPd);
        }

        [Fact]
        public void CalculateSalaryReturn_Test2()
        {
            SalaryReturnModel oExpecteds = new SalaryReturnModel
            {
                dcGrossSal   = decimal.Parse(ConfigurationManager.AppSettings["Test.Gross.Salary.2"]),
                dcNetSal     = decimal.Parse(ConfigurationManager.AppSettings["Test.Net.Salary.2"]),
                dcGrossTaxPd = decimal.Parse(ConfigurationManager.AppSettings["Test.Gross.Tax.2"])
            };
            SalaryReturnModel oInputs = new SalaryReturnModel
            {
                dcGrossSal = oExpecteds.dcGrossSal
            };
            TaxValuesModel oTaxVal = new TaxValuesModel
            {
                dcTaxInc1  = decimal.Parse(ConfigurationManager.AppSettings["Test.Inc.1"]),
                dcTaxInc2  = decimal.Parse(ConfigurationManager.AppSettings["Test.Inc.2"]),
                dcTaxInc3  = decimal.Parse(ConfigurationManager.AppSettings["Test.Inc.3"]),
                dcTaxRate1 = decimal.Parse(ConfigurationManager.AppSettings["Test.Rate.1"]),
                dcTaxRate2 = decimal.Parse(ConfigurationManager.AppSettings["Test.Rate.2"]),
                dcTaxRate3 = decimal.Parse(ConfigurationManager.AppSettings["Test.Rate.3"])
            };

            SalaryReturnModel oActuals = TaxManBo.CalculateSalaryReturn(oTaxVal, oInputs);

            Assert.Equal(oExpecteds.dcGrossSal, oActuals.dcGrossSal);
            Assert.Equal(oExpecteds.dcNetSal, oActuals.dcNetSal);
            Assert.Equal(oExpecteds.dcGrossTaxPd, oActuals.dcGrossTaxPd);
        }
    }
}
