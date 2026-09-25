using CollectionsTask.DB;
using static CollectionsTask.AdminPanel;
using static CollectionsTask.LoginManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

public class Program
{
    public static readonly SampleDbContext DB = SetupDB();
    

    public static void Main(string[] args)
    {
        Console.Clear();
        AdminOrUser();
    }

    private static SampleDbContext SetupDB()
    {
        // Configure appsettings.json loading
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
        
        // Create DbContext with connection string from config
        var optionsBuilder = new DbContextOptionsBuilder<SampleDbContext>();
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

        return new SampleDbContext(optionsBuilder.Options);
    }

    private static void AdminOrUser()
    {
        do
        {
            System.Console.WriteLine("Please Select one of the Following Options:");
            System.Console.WriteLine("[Esc] Exit.");
            System.Console.WriteLine("[1] I am a customer.");
            System.Console.WriteLine("[2] I am an administrator.");

            switch(Console.ReadKey().KeyChar - '0')
            {
                case ((int) ConsoleKey.Escape - '0'):
                    return;
                case 1:
                    Console.Clear();
                    MainMenu();
                    break;
                case 2:
                    Console.Clear();
                    AdminLogin();
                    break;
                default:
                    break;
            }

        } while (true);
    }

    private static void MainMenu()
    {
        while (true)
        {
            Console.Clear();
            System.Console.WriteLine("Welcome to the National Bank of Fortbuscus.");
            System.Console.WriteLine("Please Select one of the Following Options:");
            System.Console.WriteLine("[Esc] Exit.");
            System.Console.WriteLine("[1] Log In.");
            System.Console.WriteLine("[2] Sign Up.");

            switch(Console.ReadKey().KeyChar - '0')
            {
                case ((int) ConsoleKey.Escape - '0'):
                    return;
                case 1:
                    Console.Clear();
                    Login();
                    break;
                case 2:
                    Console.Clear();
                    SignUp();
                    break;
                default:
                    break;
            }
        }
    }

}