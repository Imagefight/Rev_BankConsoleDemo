namespace CollectionsTask;
using BankLib.Login;
using CollectionsTask.DB;

using static CollectionsTask.Helpers.RequestInput;
using static CollectionsTask.AccountActions;

public class LoginManagement
{
    public static SampleDbContext DB => Program.DB;
    public static LoginAccount SignUp()
    {
        Console.WriteLine("Thank you for choosing Fortbuscus.");
        Console.WriteLine("To create a new account, please provide the following information.");
        string  username,
                password,
                firstName,
                lastName;
        bool    usernameTaken;

        firstName = RequestText("FirstName: ", false, 32, 2);
        if (firstName == null) return null;
        
        lastName = RequestText ("LastName: ", false, 32, 2);
        if (lastName == null) return null;

        do
        {
            username = RequestText("Username: ", false, 32, 8);
            if (username == null) return null;

            usernameTaken = UsernameTaken(username);
            
            if (usernameTaken)
                System.Console.WriteLine("We apologize, but your username is taken, please try again with a new username.");
            
        } 
        while (usernameTaken);

        password = RequestText("Password: ", true, 32, 8);
        if (password == null) return null;

        LoginAccount login = new
        (
            firstName,
            lastName,
            username,
            password
        );
        
        DB.Add(login);
        DB.SaveChanges();

        Console.WriteLine($"Your information is processed.");
        Console.WriteLine("Please press any key to continue.");
        
        Console.ReadKey();
        return login;
    }

    public static void Login()
    {
        string  username,
                password;
        LoginAccount match;
        Console.WriteLine("Welcome back.");
        Console.WriteLine("To log in, please provide the following information.");
        //Console.WriteLine("Input -1 to return to the previous menu.");
        
        do
        {  
            username = RequestText("Username: ", false, 32, 8);
            if (username == null) return;

            password = RequestText("Password: ", true, 32, 8);
            if (password == null) return;

            string usernameHash = LoginAccount.Hash(username);
            string passwordHash = LoginAccount.Hash(password);

            var acc =   from    login in DB.Logins
                        where   login.Username == usernameHash &&
                                login.Password == passwordHash &&
                                login.IsAdmin == false
                        select  login;
            
            match = acc.FirstOrDefault();

            if (match == default)
            {
                Console.WriteLine("We couldn't find that account. If you'd like to return to the main menu, please press escape. Otherwise, press any other key to try again.");
                
                if (Console.ReadKey().Key == ConsoleKey.Escape) 
                    return;
            }
            
        } while (match == default);

        AccountMenu(match);
    }

    public static void ChangeUsername(LoginAccount login)
    {
        if (ValidatePassword(login) == false) return;

        do 
        {
            string newUsername = RequestText("Please input your new username: ", false, 32, 8);
            if (newUsername == null) return;

            else if (UsernameTaken(newUsername))
            {
                System.Console.WriteLine("Username is already taken. Please try again.");
                continue;
            }

            login.ChangeUsername(newUsername);
            DB.Logins.Update(login);
            DB.SaveChanges();

            System.Console.WriteLine("Your username has been changed successfully. Press any key to continue.");
            Console.ReadKey();
            return;
        } while (true);
    }

    public static void ChangePassword(LoginAccount login)
    {
        if (ValidatePassword(login) == false) return;

        do
        {
            string newPassword = RequestText("Please input your new password: ", true, 32, 8);
            if (newPassword == null) return;

            string confirmPassword = RequestText("Please confirm your new password: ", true, 32, 8);
            if (confirmPassword == null) return;

            if (newPassword != confirmPassword)
                System.Console.WriteLine("Passwords do not match. Please try again.");
            else
            {
                login.ChangePassword(newPassword);
                DB.Logins.Update(login);
                DB.SaveChanges();

                System.Console.WriteLine("Your password has been changed successfully. Press any key to continue.");
                Console.ReadKey();
                return;
            }
        } while (true);
    }

    // "Reset" in this case IM ASSUMING means resetting their password for them
    public static void ResetAccount(LoginAccount login)
    {
        string newPassword = RequestText("Please input the new password: ", true, 32, 8);
        if (newPassword == null) return;

        login.ChangePassword(newPassword);
        DB.Logins.Update(login);
        DB.SaveChanges();

        System.Console.WriteLine("The account has been reset successfully. Press any key to continue.");
        Console.ReadKey();
    }
    #region Helper Functions
    private static bool UsernameTaken(string username)
    {
        string usernameHash = LoginAccount.Hash(username);
        return DB.Logins.Any(login => login.Username == usernameHash);
    }

    private static bool ValidatePassword(LoginAccount login)
    {
        bool match = false;
        string password;
        do
        {
            password = RequestText("Please input your current password to continue: ", true, 32, 8);
            if (password == null) return false;

            match = login.ValidatePassword(password);
        } while (!match);

        return match;
    }
    #endregion Helper Functions
}