namespace BankLib.Transactions
{
    public class Transaction
    {
        public int ID {get; set;}
        public string AccountNumber {get; set;}
        public double Amount {get; set;}
        public DateTime Date {get; set;}

        public Transaction(){} //Parameterless constructor for EF Core

        public Transaction(string accountNumber, double amount)
        {
            AccountNumber = accountNumber;
            Amount = amount;
            Date = DateTime.Now;
        }

        public virtual void OutputToConsole()
        {
            System.Console.WriteLine("- --  -- --");
            Console.WriteLine($"Account Number: {AccountNumber}");
            Console.WriteLine($"Amount: {Amount}");
            Console.WriteLine($"Date: {Date}");
        }
    }

    public class TransferTransaction : Transaction
    {
        public string ToAccountNumber {get; set;}

        public TransferTransaction(){} //Parameterless constructor for EF Core

        public TransferTransaction(string fromAccountNumber, string toAccountNumber, double amount)
            : base(fromAccountNumber, amount)
        {
            AccountNumber = fromAccountNumber;
            ToAccountNumber = toAccountNumber;
        }

        public override void OutputToConsole()
        {
            base.OutputToConsole();
            Console.WriteLine($"To Account Number: {ToAccountNumber}");
        }
    }
}