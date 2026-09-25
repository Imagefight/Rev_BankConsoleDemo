namespace BankLib
{
    namespace AccountSystem
    {
        public class CheckingAccount : Account
        {
            public bool IsOverdraftEnabled {get; set;} = false;
            public double OverdraftLimit {get; set;} = 30000;
            
            // Parameterless for EF Core
            public CheckingAccount() => AccountNumber = GenerateRandomAccountNumber();

            public CheckingAccount(double balance, int assocID, bool overdraft = false, double overdraftLimit = 30000)
            {
                Balance = balance;
                IsActive = true;
                IsOverdraftEnabled = overdraft;
                OverdraftLimit = overdraftLimit;
                AccountType = Type.Checking;
                AssociatedID = assocID;
                
                AccountNumber = GenerateRandomAccountNumber();
            }

            public override double Withdrawl(double amount)
            {
                if (amount > Balance)
                {
                    if (!IsOverdraftEnabled) 
                        throw new ArgumentException("Overdraft is disabled.");
                    else if (IsOverdraftEnabled && Balance - amount < -OverdraftLimit)
                        throw new ArgumentOutOfRangeException($"You may not exceed the overdraft limit of ${OverdraftLimit}");
                    else
                        Balance -= amount;
                }

                return base.Withdrawl(amount);
            }
        
            public override void OutputToConsole()
            {
                base.OutputToConsole();

                if (IsOverdraftEnabled)
                    Console.WriteLine($"Overdraft Limit: {OverdraftLimit}");
            }
        }
    }
}