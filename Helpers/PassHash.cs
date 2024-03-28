using System.Security.Cryptography;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FinalProjectLoans.Helpers
{
    
    public class PassHash
    {
        

        static string GenerateHash(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] combinedBytes = Encoding.UTF8.GetBytes(password);
                return  Convert.ToBase64String(sha256.ComputeHash(combinedBytes));
            }
        }

        public static string HashPassword(string password)
        {
            return GenerateHash(password);
        }
    }
}
