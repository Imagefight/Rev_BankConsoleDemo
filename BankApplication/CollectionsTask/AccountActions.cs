namespace CollectionsTask;

using BankLib.AccountSystem;
using BankLib.Login;
using BankLib.Transactions;
using Microsoft.IdentityModel.Tokens;

using static CollectionsTask.Helpers.RequestInput;
using static CollectionsTask.Helpers.ListBuilder;
using static CollectionsTask.LoginManagement;

public static class AccountActions
{
    private static DB.SampleDbContext DB => Program.DB;

    public static void AccountMenu(LoginAccount login)
    {
        while(true)
        {
            Console.Clear();
            System.Console.WriteLine($"Welcome, {login.FirstName} {login.LastName}.");
            System.Console.WriteLine("Please Select one of the Following Options:");
            System.Console.WriteLine("[Esc] Return to Main Menu.");
            System.Console.WriteLine("[1] Check Account Details.");
            System.Console.WriteLine("[2] Get Transaction History.");
            System.Console.WriteLine("[3] Manage Checkbooks.");
            System.Console.WriteLine("[4] Withdraw Cash.");
            System.Console.WriteLine("[5] Deposit Cash.");
            System.Console.WriteLine("[6] Transfer to another account.");
            System.Console.WriteLine("[7] Apply for a New Account.");
            System.Console.WriteLine("[8] Change Password");
            System.Console.WriteLine("[9] Change Username");

            switch(Console.ReadKey().KeyChar - '0')
            {
                case ((int) ConsoleKey.Escape - '0'):
                    return;
                case 1:
                    Console.Clear();
                    ViewAccountDetails(login);
                    break;
                case 2:
                    Console.Clear();
                    GetTransactions(login);
                    break;
                case 3:
                    Console.Clear();
                    CheckbookMenu(login);
                    break;
                case 4:
                    Console.Clear();
                    WithdrawMenu(login);
                    break;
                case 5:
                    Console.Clear();
                    DepositMenu(login);
                    break;
                case 6:
                    Console.Clear();
                    TransferMenu(login);
                    break;
                case 7:
                    Console.Clear();
                    RegisterAccount(login);
                    break;
                case 8:
                    Console.Clear();
                    ChangePassword(login);
                    break;
                case 9:
                    Console.Clear();
                    ChangeUsername(login);
                    break;
                default:
                    break;
            }
        }
    }

    #region Account Management
    public static Account SelectAccount(
        LoginAccount login, 
        bool checking = true, 
        bool savings = true, 
        bool loan = true, 
        bool omitClosed = true
    )
    {
        var accounts = from account in DB.Accounts 
                       where account.AssociatedID == login.ID &&
                           ((checking && account.AccountType == Account.Type.Checking) ||
                            (savings  && account.AccountType == Account.Type.Savings) ||
                            (loan     && account.AccountType == Account.Type.Loan)) &&
                            (!omitClosed || account.IsActive)
                       select account;

        Account selectedAccount = ListSelectable(
            accounts,
            "Bank Account",
            account => $"Account {account.AccountNumber}: {account.AccountType}, ${account.CheckBalance()}");

        return selectedAccount;
    }

    public static void ViewAccountDetails(LoginAccount login)
    {
        Console.Clear();
        System.Console.WriteLine($"Account ID: {login.ID}");
        System.Console.WriteLine($"First Name: {login.FirstName}");
        System.Console.WriteLine($"Last Name: {login.LastName}");
        Console.WriteLine("---- ----- ----");
        Console.WriteLine("Press any key to continue.");
        Console.ReadKey();
        return;
    }


    private static void DepositMenu(LoginAccount login)
    {
        Account selectedAccount = SelectAccount(login);
        Transaction transaction;
        if (selectedAccount == null) return;
        
        double? deposit;
        bool success = false;

        Console.WriteLine($"Your current balance is: {selectedAccount.CheckBalance()}");
        do
        {
            deposit = RequestDouble("How much would you like to deposit? ");
            if (deposit == null) return;
        }
        while (deposit.Value < 1);
        
        do
        {
            try
            {
                transaction = new Transaction(selectedAccount.AccountNumber, deposit.Value);
                double oldBalance, newBalance;
                
                oldBalance = selectedAccount.CheckBalance();

                if (selectedAccount.AccountType == Account.Type.Loan)
                    if (deposit.Value > selectedAccount.CheckBalance())
                            selectedAccount.Deposit(selectedAccount.CheckBalance());
                else
                    selectedAccount.Deposit(deposit.Value);
                
                newBalance = selectedAccount.CheckBalance();
                
                System.Console.WriteLine($"Your old balance: {oldBalance}");
                System.Console.WriteLine($"Your new balance: {newBalance}");
                System.Console.WriteLine($"Deposit Amount: {deposit}");
                System.Console.WriteLine("-- --- --- ---- --- --- --");
                System.Console.WriteLine("Press any key to continue.");
                
                DB.Accounts.Update(selectedAccount);
                DB.Transactions.Add(transaction);
                DB.SaveChanges();
                success = true;

                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.Clear();
                Console.WriteLine($"{e.Message} Please try again.");
            }
        } while (!success);
    }

    private static void WithdrawMenu(LoginAccount login)
    {
        Account selectedAccount = SelectAccount(login, loan: false);
        if (selectedAccount == null) return;
        
        double? withdrawl;
        bool success = false;
        Transaction transaction;

        Console.WriteLine($"Your current balance is: {selectedAccount.CheckBalance()}");
        
        do
        {
            withdrawl = RequestDouble("How much would you like to withdraw from your account? ");
            if (withdrawl == null) return;
        }
        while (withdrawl < 1);

        do
        {
            try
            {
                double oldBalance = selectedAccount.CheckBalance();
                selectedAccount.Withdrawl(withdrawl.Value);
                
                transaction = new Transaction(selectedAccount.AccountNumber, -withdrawl.Value);
                DB.Transactions.Add(transaction);

                double newBalance = selectedAccount.CheckBalance();
                
                DB.Accounts.Update(selectedAccount);
                DB.SaveChanges();

                System.Console.WriteLine($"Your old balance: {oldBalance}");
                System.Console.WriteLine($"Your new balance: {newBalance}");
                System.Console.WriteLine($"Withdrawl Amount: {withdrawl}");
                System.Console.WriteLine("-- --- --- ---- --- --- --");
                System.Console.WriteLine("Press any key to continue.");
                Console.ReadKey();
                success = true;
            }
            catch (Exception e)
            {
                Console.Clear();
                Console.WriteLine($"{e.Message} Please try again.");
            }
        } while (!success);
        
    }

    private static void RegisterAccount(LoginAccount login)
    {
        while (true)
        {
            Console.Clear();
            System.Console.WriteLine("In order to apply for an account, please select one of the following options:");
            System.Console.WriteLine("0. Return.");
            System.Console.WriteLine("1. Apply for a Checking Account.");
            System.Console.WriteLine("2. Apply for a Savings Account.");
            System.Console.WriteLine("3. Apply for a Loan.");

            switch(Console.ReadKey().KeyChar - '0')
            {
                case 0:
                    return;
                case 1:
                    Console.Clear();
                    RegisterChecking(login);
                    break;
                case 2:
                    Console.Clear();
                    RegisterSavings(login);
                    break;
                case 3:
                    Console.Clear();
                    ApplyLoan(login);
                    break;
                default:
                    break;
            }
        }
    }

    private static void RegisterChecking(LoginAccount login)
    {
        CheckingAccount checking;
        Console.WriteLine("You're currently applying for a Checking Account.");
        Console.WriteLine("Please provide the following information.");
        double? amount, overdraftLimit;
        bool? overdraft;
        
        amount = RequestDouble("Starting Account Balance: ");
        if (amount == null) return;

        checking = new(
            amount.Value,
            login.ID
        );

        overdraft = RequestBoolean("Would you like to apply Overdraft Limits? Please input true or false: ");
        if (overdraft == null) return;

        checking.IsOverdraftEnabled = overdraft.Value;

        if(checking.IsOverdraftEnabled)
        {
            overdraftLimit = RequestDouble("Please specify the limit allowed for overdraft: ");
            if (overdraftLimit == null) return; 

            checking.OverdraftLimit = overdraftLimit.Value;
        }
        
        DB.Accounts.Add(checking);
        DB.SaveChanges();

        Console.WriteLine($"Thank you, your information is processed. Your account number is: {checking.AccountNumber}");
        Console.WriteLine("Please press any key to continue.");
        
        Console.ReadKey();
    }

    private static void RegisterSavings(LoginAccount login)
    {
        SavingsAccount savings;
        double? amount;

        Console.WriteLine("Thank you for choosing Fortbuscus. You're currently applying for a Savings Account.");
        Console.WriteLine("Please provide the following information.");
        
        amount = RequestDouble("Starting Account Balance: ");
        if (amount == null) return;

        savings = new(
            amount.Value,
            login.ID
        );
        
        DB.Accounts.Add(savings);
        DB.SaveChanges();

        Console.WriteLine($"Thank you, your information is processed. Your account number is: {savings.AccountNumber}");
        Console.WriteLine("Please press any key to continue.");
        Console.ReadKey();
    }

    private static void ApplyLoan(LoginAccount login)
    {
        LoanAccount loan;
        double? amount;
        Console.WriteLine("Thank you for choosing Fortbuscus. You're currently applying for a Loan.");
        Console.WriteLine("Please provide the following information.");
        
        amount = RequestDouble("Loan Amount: ");
        if (amount == null) return;

        loan = new(
            amount.Value,
            login.ID
        );
        
        DB.Accounts.Add(loan);
        DB.SaveChanges();

        Console.WriteLine($"Thank you, your information is processed. Your account number is: {loan.AccountNumber}");
        Console.WriteLine("Please press any key to continue.");
        Console.ReadKey();
    }

    #endregion Account Management

    #region Checkbooks
    public static void CheckbookMenu(LoginAccount login)
    {
        Account account = SelectAccount(login, savings: false, loan: false);
        if (account == null) return;
        do
        {
            Console.Clear();
            System.Console.WriteLine("Please Select one of the Following Options:");
            System.Console.WriteLine("[Esc] Exit.");
            System.Console.WriteLine("[1] View Checkbooks");
            System.Console.WriteLine("[2] Request New Checkbook");

            switch (Console.ReadKey().KeyChar - '0')
            {
                case ((int) ConsoleKey.Escape - '0'):
                    return;
                case 1:
                    Console.Clear();
                    ViewCheckbooks(account);
                    break;
                case 2:
                    Console.Clear();
                    RequestCheckbook(account);
                    break;
                default:
                    break;
            }
        }while(true);
    }

    public static void ViewCheckbooks(Account account)
    {
        var checkbooks = DB.Checkbooks
            .Where(checkbook => checkbook.AccountNumber == account.AccountNumber);
        
        Checkbook selectedCheckbook = ListSelectable<Checkbook>(checkbooks, "Checkbook", checkbook => $"Checkbook ID: {checkbook.ID} | Active: {checkbook.Active}");
        if (selectedCheckbook == null) return;
    }

    private static void RequestCheckbook(Account account)
    {
        if (account.AccountType != Account.Type.Checking)
        {
            System.Console.WriteLine("Checkbooks are only available for Checking Accounts. Press any key to return.");
            Console.ReadKey();
            return;
        }

        Checkbook checkbook = new(account.AccountNumber);
        
        DB.Checkbooks.Add(checkbook);
        DB.SaveChanges();

        System.Console.WriteLine($"Your checkbook has been requested. Your checkbook ID is: {checkbook.ID}");
        System.Console.WriteLine("Please press any key to continue.");
        Console.ReadKey();
    }

    #endregion Checkbooks

    #region Transfers
    public static void TransferMenu(LoginAccount login)
    {
        Account transferFrom = SelectAccount(login, loan:false);
        if (transferFrom == null) return;
        bool confirm = false;

        do
        {
            string? recipientAccNo = RequestText("Please provide the Account Number of the recipient: ", false, 32, 1);
            bool? confirmAccount;
            if (recipientAccNo == null) 
                return; 
            
            Account recipient = DB.Accounts.Where(acc => acc.AccountNumber == recipientAccNo).FirstOrDefault();

            if (recipient != null)
            {
                string recipientName = DB.Logins
                    .Where(login => login.ID == recipient.AssociatedID)
                    .Select(login => $"{login.FirstName} {login.LastName}")
                    .FirstOrDefault();
                
                confirmAccount = RequestBoolean($"Recipient: {recipientName}. Is this correct? ");
                if (confirmAccount == null) 
                    return;
                else if (confirmAccount.Value)
                {
                    Console.Clear();
                    FinalizeTransfer(transferFrom, recipient);
                    confirm = true;
                }
                else
                {
                    Console.Clear();
                }
            }
            
            else
            {
                Console.Clear();
                Console.WriteLine("Account not found. Please try again.");
            }

        } while (!confirm);
    }

    private static void FinalizeTransfer(Account from, Account to)
    {
        TransferTransaction transaction;
        double? amount;
        bool success = false;

        Console.WriteLine("Please Write -1 to return to the previous menu.");
        Console.WriteLine($"Your current balance is: {from.CheckBalance()}");
        amount = RequestDouble("Please provide the transfer amount: ");
        if (amount == null) return;

        do
        {
            if (amount > from.CheckBalance())
            {
                Console.Clear();
                System.Console.WriteLine("Insufficient funds, please try again.");
            }
            // Everything works
            else
            {
                
                string recepientName = DB.Logins
                    .Where(login => login.ID == to.AssociatedID)
                    .Select(login => $"{login.FirstName} {login.LastName}")
                    .FirstOrDefault();
                
                System.Console.WriteLine($"You're sending: {amount.Value}");
                System.Console.WriteLine($"To: {recepientName}: {to.AccountNumber}");
                System.Console.WriteLine($"From your account: {from.AccountNumber}");

                if (RequestBoolean("Is this correct? ").Value)
                {
                    double oldBalance = from.CheckBalance();
                    from.Withdrawl(amount.Value);
                    to.Deposit(amount.Value);
                    double newBalance = from.CheckBalance();

                    transaction = new TransferTransaction(from.AccountNumber, to.AccountNumber, amount.Value);
                    DB.TransferTransactions.Add(transaction);
                    DB.Accounts.Update(from);
                    DB.Accounts.Update(to);
                    DB.SaveChanges();
                    
                    System.Console.WriteLine($"Your old balance: {oldBalance}");
                    System.Console.WriteLine($"Your new balance: {newBalance}");
                    System.Console.WriteLine($"Amount Sent: {amount}");
                    System.Console.WriteLine("-- --- --- ---- --- --- --");
                    System.Console.WriteLine("Press any key to continue.");
                    Console.ReadKey();

                    return;
                }
                else
                {
                    Console.Clear();
                    System.Console.WriteLine("Transaction Aborted.");
                    System.Console.WriteLine("-- --- --- ---- --- --- --");                    
                    System.Console.WriteLine("Press any key to continue.");
                    Console.ReadKey();
                    success = true;
                }
            }
        } while (!success);
    }
   #endregion Transfers

    #region Transactions
    public static void GetTransactions(LoginAccount login)
    {
        Account selectedAccount = SelectAccount(login);
        if (selectedAccount == null) return;

        var transactions = DB.Transactions
            .Where(transaction => transaction.AccountNumber == selectedAccount.AccountNumber)
            .ToList();

        var transfersToMe = DB.TransferTransactions
            .Where(transfer => transfer.ToAccountNumber == selectedAccount.AccountNumber)
            .ToList();

        var allTransactions = transactions
            .Concat(transfersToMe)
            .OrderByDescending(transaction => transaction.Date)
            .ToList();
        
        Console.Clear();
        
        if (allTransactions.IsNullOrEmpty())
        {
            System.Console.WriteLine("No transactions found. Press any key to return.");
            System.Console.WriteLine("- -- --- -- --- -- --------- --- -- --- - --- -");
            Console.ReadKey();
            return;
        }

        foreach (var transaction in allTransactions)
            transaction.OutputToConsole();

        System.Console.WriteLine("- --  -- --");
        Console.ReadKey();
        return;
    }
    #endregion Transactions
}