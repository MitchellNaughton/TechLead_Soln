
namespace TaxManDL.Models
{
    public class TaxValuesModel
    {
        public int iCountryId { get; set; }
        public decimal dcTaxInc1 { get; set; }
        public decimal dcTaxInc2 { get; set; }
        public decimal dcTaxInc3 { get; set; }
        public decimal dcTaxRate1 { get; set; }
        public decimal dcTaxRate2 { get; set; }
        public decimal dcTaxRate3 { get; set; }
    }

    public class SqlTaxModel
    {
        public float flTaxInc1 { get; set; }
        public float flTaxInc2 { get; set; }
        public float flTaxInc3 { get; set; }
        public float flTaxRate1 { get; set; }
        public float flTaxRate2 { get; set; }
        public float flTaxRate3 { get; set; }
    }
}