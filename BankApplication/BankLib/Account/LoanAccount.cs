namespace BankLib
{    
    namespace AccountSystem
    {
        public class LoanAccount : Account
        {

            // Parameterless for EF Core
            public LoanAccount() => AccountNumber = GenerateRandomAccountNumber();

            public LoanAccount(double balance, int assocID)
            {
                Balance = balance;
                IsActive = false;
                AccountType = Type.Loan;
                AssociatedID = assocID;
                
                AccountNumber = GenerateRandomAccountNumber();
            }

            public override double Withdrawl(double amount)
            {
                throw new Exception("Withdrawls are not permitted under this type of account.");
            }

            public override double Deposit(double amount)
            {
                if (amount < 0) 
                    throw new ArgumentException("Deposit amount cannot be negative.");
                if (Balance - amount < 0)
                    throw new ArgumentException($"The given deposit: {amount} is over the amount to pay off your loan: {Balance}");
                Balance -= amount;

                if (Balance <= 0) 
                    Terminate();
                
                return Balance;
            }

            private void Terminate()
            {
                IsActive = false;
            }
        }
    }
}