namespace FinalProjectLoans.Model.Domain
{
    public class Accountant
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public const string role = "Accountant";
    }
}
