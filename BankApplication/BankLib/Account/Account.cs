using System.Globalization;
using System.Security.Cryptography;

namespace BankLib
{
    namespace AccountSystem
    {
        public abstract class Account
        {
            public string AccountNumber {get; set;}
            public int AssociatedID {get; set;}
            public double Balance {get; protected set;}
            public bool IsActive {get; set;}
            public Type AccountType {get; set;}
            public enum Type
            {
                Checking,
                Savings,
                Loan
            }

            public virtual double Deposit(double amount)
            {
                if (amount < 0) throw new ArgumentException("Deposit amount cannot be negative.");

                Balance += amount;
                return Balance;
            }

            public virtual double Withdrawl(double amount)
            {
                if (amount < 0) 
                    throw new ArgumentException("Withdrawl cannot be negative.");
                if (amount > Balance)
                    throw new ArgumentException("Withdrawl cannot exceed balance.");
                
                Balance -= amount;
                return Balance;
            }

            public virtual void OutputToConsole()
            {
                Console.WriteLine($"Account Type: {AccountType}");
                Console.WriteLine($"Account Balance: {Balance}");    
                Console.WriteLine($"Active: {IsActive}");    
            }

            public static string GenerateRandomAccountNumber()
            {
                // Generate a random 13-digit base: from 1000000000000 to 9999999999999
                ulong max = 9_999_999_999_999 - 100_000_000_000_0 + 1; // count of 13-digit numbers
                Span<byte> bytes = stackalloc byte[8];
                RandomNumberGenerator.Fill(bytes);

                ulong value = BitConverter.ToUInt64(bytes);
                ulong base13 = value % max + 1_000_000_000_000; // 13-digit base
                
                int checkDigit = LuhnCheckDigit(base13.ToString()); // Luhn check digit
                return $"{base13}{checkDigit}"; // 14 digits total: 13 random + 1 check
            }

            // Luhn algorithm check digit (used in credit card numbers)
            static int LuhnCheckDigit(string digits)
            {
                int sum = 0;
                bool doubleDigit = true; // start from rightmost position of final number
                for (int i = digits.Length - 1; i >= 0; i--)
                {
                    int d = digits[i] - '0';
                    if (doubleDigit)
                    {
                        d *= 2;
                        if (d > 9) d -= 9;
                    }
                    sum += d;
                    doubleDigit = !doubleDigit;
                }
                return (10 - (sum % 10)) % 10;
            }

            public double CheckBalance() => Balance;
        }
    }
}