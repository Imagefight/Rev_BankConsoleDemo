namespace BankLib
{    
    namespace AccountSystem
    {
        public class SavingsAccount : Account
        {
            private double maxWithdrawl {get; set;} = 5000;

            // Parameterless constructor for EF Core
            public SavingsAccount() => AccountNumber = GenerateRandomAccountNumber();
            public SavingsAccount(double balance, int assocID)
            {
                Balance = balance;
                IsActive = true;
                AccountType = Type.Savings;
                AssociatedID = assocID;
                AccountNumber = GenerateRandomAccountNumber();
            }

            public override double Withdrawl(double amount)
            {
                if (amount > maxWithdrawl) 
                    throw new ArgumentException($"Withdrawal amount must not exceed {maxWithdrawl}.");
                else
                    return base.Withdrawl(amount);
            }
        }
    }
}