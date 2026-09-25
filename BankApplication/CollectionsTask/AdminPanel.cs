namespace CollectionsTask;
using BankLib.AccountSystem;
using BankLib.Login;

using static CollectionsTask.AccountActions;
using static CollectionsTask.LoginManagement; 
using static CollectionsTask.Helpers.ListBuilder;
using static CollectionsTask.Helpers.RequestInput;

public class AdminPanel
{
    private static DB.SampleDbContext DB => Program.DB;
    public static void AdminLogin()
    {
        string  username,
                password;
        LoginAccount match;
        Console.WriteLine("Welcome back.");
        Console.WriteLine("To log in, please provide the following information.");
        
        do
        {  
            username = RequestText("Username: ", false, 32, 8);
            if (username == null) return;

            password = RequestText("Password: ", true, 32, 8);
            if (password == null) return;

            string usernameHash = LoginAccount.Hash(username);
            string passwordHash = LoginAccount.Hash(password);

            var acc = from login in DB.Logins
                    where login.Username == usernameHash &&
                        login.Password == passwordHash &&
                        login.IsAdmin == true
                    select login;
            
            match = acc.FirstOrDefault();

            if (match == default)
            {
                Console.WriteLine("We couldn't find that account. If you'd like to return to the main menu, please press escape. Otherwise, press any other key to try again.");
                
                if (Console.ReadKey().Key == ConsoleKey.Escape) 
                    return;
            }
            
        } while (match == default);

        AdminMenu(match);
    }

    private static void AdminMenu(LoginAccount admin)
    {
        do
        {
            Console.Clear();
            Console.WriteLine("Welcome Back.");
            System.Console.WriteLine("Please Select one of the Following Options:");
            System.Console.WriteLine("[Esc] Exit.");
            System.Console.WriteLine("[1] Create New Admin Account.");
            System.Console.WriteLine("[2] Manage Admin Accounts.");
            System.Console.WriteLine("[3] Manage User Accounts.");
            System.Console.WriteLine("[4] Change Username.");
            System.Console.WriteLine("[5] Change Password.");

            switch (Console.ReadKey().KeyChar - '0')
            {
                case ((int) ConsoleKey.Escape - '0'):
                    return;
                case 1:
                    Console.Clear();
                    CreateAdminAccount();
                    break;
                case 2:
                    Console.Clear();
                    AdminManagement(AdminDirectory());
                    break;
                case 3:
                    Console.Clear();
                    LoginManagement(LoginDirectory());
                    break;
                case 4:
                    Console.Clear();
                    ChangeUsername(admin);
                    break;
                case 5:
                    Console.Clear();
                    ChangePassword(admin);
                    break;
                default:
                    break;
            }
        }
        while(true);
    }

    private static void CreateAdminAccount()
    {
        LoginAccount login = SignUp();
        if (login == null) return;
        
        login.Elevate();

        DB.Update(login);
        DB.SaveChanges();
        Console.WriteLine("Admin account created successfully. Press any key to continue.");
        Console.ReadKey();
    }

    private static void AdminManagement(LoginAccount login)
    {
        do 
        {
            Console.Clear();
            System.Console.WriteLine("Please Select one of the Following Options:");
            System.Console.WriteLine("[Esc] Exit.");
            System.Console.WriteLine("[1] Reset Account");
            System.Console.WriteLine("[2] Delete Account");

            switch (Console.ReadKey().KeyChar - '0')
            {
                case ((int) ConsoleKey.Escape - '0'):
                    return;
                case 1:
                    Console.Clear();
                    ResetAccount(login);
                    break;
                case 2:
                    Console.Clear();
                    DeleteAccount(login);
                    break;
                default:
                    break;
            }
        } while (true);
    }

    private static void LoginManagement(LoginAccount login)
    {
        do 
        {
            Console.Clear();
            System.Console.WriteLine("Please Select one of the Following Options:");
            System.Console.WriteLine("[Esc] Exit.");
            System.Console.WriteLine("[1] View Account Details");
            System.Console.WriteLine("[2] View Transactions");
            System.Console.WriteLine("[3] Manage Checkbooks");
            System.Console.WriteLine("[4] Reset Account");
            System.Console.WriteLine("[5] Delete Account");

            switch (Console.ReadKey().KeyChar - '0')
            {
                case ((int) ConsoleKey.Escape - '0'):
                    return;
                case 1:
                    ViewAccountDetails(login);
                    break;
                case 2:
                    GetTransactions(login);
                    break;
                case 3:
                    Console.Clear();
                    ManageCheckbooks(login);
                    break;
                case 4:
                    ResetAccount(login);
                    break;
                case 5:
                    DeleteAccount(login);
                    break;
                default:
                    break;
            }
        } while (true);
    }

    private static void ManageCheckbooks(LoginAccount login)
    {
        Account selectedAccount = SelectAccount(login);
        if (selectedAccount == null) return;

        var checkbooks = DB.Checkbooks
            .Where(checkbook => checkbook.AccountNumber == selectedAccount.AccountNumber);

        Checkbook selectedCheckbook = ListSelectable(checkbooks, "Checkbook", checkbook => $"Checkbook ID: {checkbook.ID} | Active: {checkbook.Active}");

        if (selectedCheckbook == null) return;

        do 
        {
        Console.Clear();
        System.Console.WriteLine("Please Select one of the Following Options:");
        System.Console.WriteLine("[Esc] Exit.");
        System.Console.WriteLine("[1] View Checkbook Status");
        System.Console.WriteLine("[2] Activate Checkbook");
        System.Console.WriteLine("[3] Decomission Checkbook");

        
            switch (Console.ReadKey().KeyChar - '0')
            {
                case ((int) ConsoleKey.Escape - '0'):
                    return;
                case 1:
                    ViewCheckbooks(selectedAccount);
                    break;
                case 2:
                    ActivateCheckbook(selectedCheckbook);
                    break;
                case 3:
                    DecomissionCheckbook(selectedCheckbook);
                    break;
                    
                default:
                    break;
            }
        } while (true);
    }


    private static void DeleteAccount(LoginAccount login)
    {
        Console.Clear();
        bool input = RequestBoolean($"Are you sure you want to delete the account for {login.FirstName} {login.LastName}? This action cannot be undone.").Value;
        if (input == null) return;

        if (input)
        {
            DB.Logins.Remove(login);
            DB.SaveChanges();
            System.Console.WriteLine("Account deleted successfully. Press any key to continue.");
            Console.ReadKey();
        }

        else
        {
            System.Console.WriteLine("Account deletion cancelled. Press any key to continue.");
            Console.ReadKey();
        }
    }

    private static LoginAccount LoginDirectory()
    {
        var accounts =  from account in DB.Logins 
                        where account.IsAdmin == false 
                        select account;
        
        LoginAccount selectedAccount = ListSelectable(accounts, "Login Account", 
            account => $"{account.FirstName} {account.LastName} (ID: {account.ID})");

        return selectedAccount;
    }

    private static LoginAccount AdminDirectory()
    {
        var accounts =  from account in DB.Logins 
                        where account.IsAdmin == true 
                        select account;
        LoginAccount selectedAccount = ListSelectable(accounts, "Admin Account", 
            account => $"{account.FirstName} {account.LastName} (ID: {account.ID})");

        return selectedAccount;
    }

    private static void ActivateCheckbook(Checkbook checkbook)
    {
        checkbook.Activate();
        DB.Checkbooks.Update(checkbook);
        DB.SaveChanges();

        System.Console.WriteLine($"Checkbook ID: {checkbook.ID} has been approved.");
        System.Console.WriteLine("Please press any key to continue.");
        Console.ReadKey();
    }

    private static void DecomissionCheckbook(Checkbook checkbook)
    {
        checkbook.Decomission();
        DB.Checkbooks.Update(checkbook);
        DB.SaveChanges();

        System.Console.WriteLine($"Checkbook ID: {checkbook.ID} has been decomissioned.");
        System.Console.WriteLine("Please press any key to continue.");
        Console.ReadKey();
    }
}