using FinalProjectLoans.Helpers;
using FinalProjectLoans.Model.Data;
using FinalProjectLoans.Model.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FinalProjectLoans.Service
{
    
    public interface IUserService
    {
        User LogIn(string userName, string passWord);
        User RegisterUser(User newUser);
        Loan CreateLoan(User user, Loan newLoan);
        Loan EditLoan(User user, Loan loan);
        Loan DeleteLoan(User user,Loan loan);
        List<Loan> GetAllLoans(User user);
    }

    public class UserService : IUserService
    {
        LoanContext _context = new LoanContext();
        private User ? GetUserByUsername (string userName)
        {
            return _context.Users.Where(e => e.UserName == userName).FirstOrDefault();
        }
        
        public User LogIn(string userName, string passWord)
        {
            User ? userModel = GetUserByUsername(userName);
            if (userModel == null)  return null;
            string hashedPass = PassHash.HashPassword(passWord);
            if (userModel.PassWord != hashedPass) return null;
            return userModel;

        }
        private bool CheckUsername(string username)
        {
            if (_context.Users.Where(e => e.UserName == username).FirstOrDefault() != null)
            {
                return false;
            }
            return true;
        }
        public User RegisterUser(User newUser)
        {
            if (CheckUsername(newUser.UserName) == false) return null;
            newUser.IsBlocked = false;
            newUser.PassWord = PassHash.HashPassword(newUser.PassWord);
            _context.Users.Add(newUser);
            _context.SaveChanges();
            return newUser;
        }

        public Loan CreateLoan(User user, Loan newLoan)
        {
            if (user.IsBlocked ) return null;
            newLoan.UserId = user.Id;
            _context.Loans.Add(newLoan);
            _context.SaveChanges();
            return newLoan;
            

        }
        public Loan EditLoan(User user, Loan loan)
        {
            if (user.IsBlocked || loan.Status == true) return null;

            _context.Loans.Update(loan);
            _context.SaveChanges();
            return loan;
        }
        public Loan DeleteLoan(User user,Loan loan)
        {
            if (user.IsBlocked || loan.Status == true) return null;

            _context.Loans.Remove(loan);
            _context.SaveChanges();
            return loan;
        }
        public List<Loan> GetAllLoans(User user)
        {
            return _context.Loans.Where(e => e.UserId == user.Id).ToList();
        }

    }
}
