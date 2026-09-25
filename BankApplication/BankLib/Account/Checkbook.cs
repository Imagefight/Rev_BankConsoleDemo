namespace BankLib.AccountSystem
{   
    public class Checkbook
    {
        public int ID {get; private set;}
        public string AccountNumber {get; private set;}
        public bool Active {get; private set;}
        
        public Checkbook(){} // Parameterless for EF Core

        public Checkbook(string accountNumber)
        {
            AccountNumber = accountNumber;
            Active = false;
        }

        public void Activate() => Active = true;
        public void Decomission() => Active = false;
    }
}