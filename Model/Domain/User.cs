namespace FinalProjectLoans.Model.Domain
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public int Age { get; set; }
        public int Salary { get; set; }
        public bool IsBlocked { get; set; }
        public const string role = "User";
    }
}
