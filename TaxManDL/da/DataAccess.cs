using System;
using Dapper;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using TaxManDL.Models;

namespace TaxManDL.da
{
    class DataAccess
    {
        public static string GetConnectionString(string connectionName = "TMDB")
        {
            return ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
        }

        public static string GetAppSetting(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        public static bool InsertSingleRecord(string ipsQuery)
        {
            using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
            {
                 int iRowNo   = cnn.Execute(ipsQuery);
                 bool lReturn = iRowNo > 0;
                 return lReturn;
            }
        }

        public static TaxValuesModel GetTaxObject(string query)
        {
            using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
            {
                SqlTaxModel vals = cnn.QuerySingle<SqlTaxModel>(query);

                TaxValuesModel oReturn = new TaxValuesModel
                {
                    dcTaxInc1  = (decimal)vals.flTaxInc1,
                    dcTaxInc2  = (decimal)vals.flTaxInc2,
                    dcTaxInc3  = (decimal)vals.flTaxInc3,
                    dcTaxRate1 = (decimal)vals.flTaxRate1,
                    dcTaxRate2 = (decimal)vals.flTaxRate2,
                    dcTaxRate3 = (decimal)vals.flTaxRate3
                };

                return oReturn;
            }
        }

        public static int GetCountryId(string query)
        {
            using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
            {
                int iReturn;
                Nullable<int> iResponse = cnn.QuerySingleOrDefault<int>(query);
                
                if (iResponse.HasValue)
                {
                    iReturn = iResponse.Value;
                } else
                {
                    
                    iReturn = 0;
                }

                return iReturn;
            }
        }
    }
}
