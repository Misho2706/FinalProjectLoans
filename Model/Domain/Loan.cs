namespace FinalProjectLoans.Model.Domain
{
    public class Loan
    {
        public int Id { get; set; }
        public int UserId {  get; set; }
        public string LoanType { get; set; }
        public int Amount { get; set; }
        public string Currency { get; set; }
        public int LoanPeriod { get; set; }
        public bool Status { get; set; }
    }
}
