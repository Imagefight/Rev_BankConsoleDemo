using System.Security.Cryptography;
using System.Text;
using BankLib.AccountSystem;

namespace BankLib
{
    namespace Login
    {
        public class LoginAccount
        {
            public string FirstName {get; set;}
            public string LastName {get; set;}
            public int ID {get; set;}
            
            // Admin Accounts are strictly used for management and
            // cannot have their own accounts.
            public bool IsAdmin {get; private set;} = false;

            public string Username 
            {
                get; 
                private set => field = Hash(value);
            }
            
            public string Password
            {
                get; 
                private set => field = Hash(value);
            }

            public LoginAccount(){} //Parameterless constructor for EF Core
            
            public LoginAccount(string firstName, string lastName, string username, string password)
            {
                FirstName = firstName;
                LastName = lastName;
                Username = username;
                Password = password;
            }

            public void Elevate() => IsAdmin = true;

            public static string Hash(string input)
            {
                byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
                StringBuilder builder = new();
                
                foreach (byte b in bytes)
                    builder.Append(b.ToString("x2")); // Hexadecimal string
                
                return builder.ToString();
            }

            public void ChangePassword(string input)    => Password = input;
            public void ChangeUsername(string input)    => Username = input;
            public bool ValidatePassword(string input)  => Hash(input) == Password;
            public bool ValidateUsername(string input)  => Hash(input) == Username;
        }
    }
}