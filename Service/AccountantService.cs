using FinalProjectLoans.Helpers;
using FinalProjectLoans.Model.Data;
using FinalProjectLoans.Model.Domain;

namespace FinalProjectLoans.Service
{
    public interface IAccountantService
    {
        Accountant LogIn(string userName, string passWord);
        List<Loan> GetAllLoans();
        User ChangeUserStatus(int id, bool isBlocked);
        Loan EditUserLoan(Loan loan);
        Loan DeleteUserLoan(int id);
    }
    public class AccountantService : IAccountantService
    {
        LoanContext _context = new LoanContext();
        private Accountant? GetUserByUsername(string userName)
        {
            return _context.Accountants.Where(e => e.UserName == userName).FirstOrDefault();
        }

        public Accountant LogIn(string userName, string passWord)
        {
            Accountant? accountantModel = GetUserByUsername(userName);
            if (accountantModel == null) return null;
            string hashedPass = PassHash.HashPassword(passWord);
            if (accountantModel.Password != hashedPass) return null;
            return accountantModel;

        }

        public List<Loan> GetAllLoans()
        {
            return _context.Loans.ToList();
        }

        public User ChangeUserStatus(int id, bool isBlocked)
        {
            var user = _context.Users.Where(e => e.Id == id).FirstOrDefault();
            if (user == null) return null;
            user.IsBlocked = isBlocked;
            _context.Update(user);
            _context.SaveChanges();
            return user;
        }

        public Loan EditUserLoan(Loan loan)
        {
            if (loan == null) return null;
            _context.Update(loan);
            _context.SaveChanges();
            return loan;
        }
        public Loan DeleteUserLoan(int id)
        {
            var loan = _context.Loans.Where(e => e.Id == id).FirstOrDefault();
            if (loan == null) return null;
            _context.Loans.Remove(loan);
            _context.SaveChanges();
            return loan;
        }



    }
}
