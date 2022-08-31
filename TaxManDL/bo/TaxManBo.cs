using System;
using TaxManDL.Models;
using TaxManDL.da;

namespace TaxManDL.bo
{
    public class TaxManBo
    {
        public static string SelectSingleQuery(string sFields, string sTable, bool bCriteria, string sClause)
        {
            string sQuery = $@"select {sFields} from {sTable}";

            if (bCriteria)
            {
                sQuery += $@" where {sClause}";
            }

            return sQuery;
        }

        public static string InsertSingleQuery(string sFields, string sTable, string sData)
        {
            string sQuery = $@"insert into {sTable} ({sFields})
                             values ({sData});";

            return sQuery;
        }

        public static SalaryReturnModel GetSalaryReturn(decimal dcSalary, string sCountry)
        {

            if (String.IsNullOrEmpty(sCountry))
            {
                sCountry = DataAccess.GetAppSetting("Default.Country");
            }
            string sQuery = SelectSingleQuery("iUid", "TMDB.dbo.tmCountry", true, $@"TMDB.dbo.tmCountry.cCountry = '{sCountry}'");
            int iCountryId = DataAccess.GetCountryId(sQuery);
            sQuery = SelectSingleQuery("flTaxInc1, flTaxInc2, flTaxInc3, flTaxRate1, flTaxRate2, flTaxRate3", "TMDB.dbo.tmTax", true, $@"TMDB.dbo.tmTax.iCountryid = {iCountryId}");
            
            TaxValuesModel otaxVal = DataAccess.GetTaxObject(sQuery);
            SalaryReturnModel oReturn = new SalaryReturnModel
            {
                dcGrossSal = dcSalary
            };

            oReturn = CalculateSalaryReturn(otaxVal, oReturn);

            return oReturn;
        }

        public static SalaryReturnModel CalculateSalaryReturn(TaxValuesModel ipoTaxVal, SalaryReturnModel ipoSalRet)
        {
            SalaryReturnModel oReturn = ipoSalRet;
            TaxValuesModel TaxVal     = ipoTaxVal;
            decimal dcTaxable1 = 0;
            decimal dcTaxable2 = 0;
            decimal dcTaxable3 = 0;

            if (TaxVal.dcTaxInc1 >= oReturn.dcGrossSal)
            {

                dcTaxable1 = oReturn.dcGrossSal;

            }
            else if (TaxVal.dcTaxInc1 < oReturn.dcGrossSal && TaxVal.dcTaxInc2 >= oReturn.dcGrossSal)
            {
                dcTaxable1   = TaxVal.dcTaxInc1;
                dcTaxable2   = oReturn.dcGrossSal - TaxVal.dcTaxInc1;
            } else if (TaxVal.dcTaxInc2 < oReturn.dcGrossSal)
            {
                dcTaxable1   = TaxVal.dcTaxInc1;
                dcTaxable2   = TaxVal.dcTaxInc2 - TaxVal.dcTaxInc1;
                dcTaxable3   = oReturn.dcGrossSal - TaxVal.dcTaxInc2;
            }

            oReturn.dcGrossTaxPd = (dcTaxable1 * TaxVal.dcTaxRate1)
                                 + (dcTaxable2 * TaxVal.dcTaxRate2)
                                 + (dcTaxable3 * TaxVal.dcTaxRate3);
            oReturn.dcNetSal = oReturn.dcGrossSal - oReturn.dcGrossTaxPd;

            return oReturn;
        }

        public static bool CreateTaxSetUp(TaxValuesModel ipoTaxVal, string ipsCountry)
        {
            bool lReturn = false;
            string sQuery = SelectSingleQuery("iUid", "TMDB.dbo.tmCountry", true, $@"TMDB.dbo.tmCountry.cCountry = '{ipsCountry}'");
            int iCountryId = DataAccess.GetCountryId(sQuery);

            if (iCountryId == 0)
            {
                sQuery = InsertSingleQuery("cCountry", "TMDB.dbo.tmCountry", $"'{ipsCountry}'");
                DataAccess.InsertSingleRecord(sQuery);
                sQuery = SelectSingleQuery("iUid", "TMDB.dbo.tmCountry", true, $@"TMDB.dbo.tmCountry.cCountry = '{ipsCountry}'");
                iCountryId = DataAccess.GetCountryId(sQuery);
            }

            ipoTaxVal.iCountryId = iCountryId;
            /* I thought about transctioning this and the country addition
             * but countries only get added if they don't exist, plus these
             * are only single inserts so if they error then they aren't added 
             * there will likely be some front-end validation as well as server
             * side validation... */
            sQuery = InsertSingleQuery("iCountryId, flTaxInc1, flTaxInc2, flTaxInc3, flTaxRate1, flTaxRate2, flTaxRate3",
                                       "TMDB.dbo.tmTax",
                                       $"{ipoTaxVal.iCountryId}, {(float)ipoTaxVal.dcTaxInc1}, {(float)ipoTaxVal.dcTaxInc2}, {(float)ipoTaxVal.dcTaxInc3}, {(float)ipoTaxVal.dcTaxRate1}, {(float)ipoTaxVal.dcTaxRate2}, {(float)ipoTaxVal.dcTaxRate3}");
            DataAccess.InsertSingleRecord(sQuery);

            return lReturn;
        }
    }
}
