namespace Api.Models
{
    public class Paycheck
    {
        public decimal GrossTotal { get; set; }
        public decimal NetTotal { get; set; }
        public List<Deduction> Deductions { get; set; }
    }
    public class Deduction
    {
        public decimal DeductionTotal { get; set; }
        public DeductionTypes DeductionType { get; set; }
    }
}
