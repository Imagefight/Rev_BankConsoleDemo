using System.Linq;
using System.Runtime.CompilerServices;
using TheThing;
using System.Text.RegularExpressions;
using System.ComponentModel.Design;
using Microsoft.VisualBasic;
public class Program
{
    private static List<Employee> Employees = [];
    public static void Main (string[] args)
    {
        InitEmployees();
        GroupEmployeesByDepartment();
    }

    private static void EstimateSalaries ()
    {
        var emp =   from employee in Employees
                    select new
                    {
                        name = employee.Name,
                        salary = employee.Salary,
                        salaryMonthly = Math.Round(employee.Salary / 12, 2),
                        bonus = Math.Round((employee.Salary / 12) * 2, 2)
                    };
        foreach(var employee in emp)
        {
            Console.WriteLine($"Name: {employee.name} | Salary: {employee.salary} | Monthly Salary: {employee.salaryMonthly} | Bonus: {employee.bonus}");
        }
    }

    private static void GroupEmployeesByDepartment()
    {
        var employmentSummary = Employees.GroupBy(employee => employee.DepartmentID);

        foreach (var item in employmentSummary)
        {
            int deptID = item.Key;
            
            System.Console.WriteLine("- -- - -- - -- -");
            System.Console.WriteLine($"Department: {deptID}");
            System.Console.WriteLine("- -- - -- - -- -");

            foreach (var f in item)
                Console.WriteLine($"Name: {f.Name}, Salary: {f.Salary}");
        }
    }

    private static IEnumerable<Employee> GetEmployeesFromDepartment(int departmentID)
    {
        var emp =   from employee in Employees
                    where employee.DepartmentID == departmentID
                    select employee;

        System.Console.WriteLine("-- - -- - -- - -- - -- - -- - -- - -- -");
        System.Console.WriteLine($"Department ID: {departmentID}");
        System.Console.WriteLine("-- - -- - -- - -- - -- - -- - -- - -- -");
        
        foreach (Employee employee in emp)
            Console.WriteLine($"Name: {employee.Name}, Department: {employee.DepartmentID}");
        
        return emp;
    }

    private static IEnumerable<Employee> GetEmployeesBySalaries(double threshold, bool above)
    {
        var emp =   from employee in Employees
                    where above ? employee.Salary > threshold : employee.Salary < threshold
                    select employee;

        System.Console.WriteLine("-- - -- - -- - -- - -- - -- - -- - -- -");
        System.Console.WriteLine($"{(above ? "Above" : "Below")} {threshold}");
        System.Console.WriteLine("-- - -- - -- - -- - -- - -- - -- - -- -");
        
        foreach (Employee employee in emp)
            Console.WriteLine($"Name: {employee.Name}, Salary: {employee.Salary}");
        
        return emp;
    }

    private static IEnumerable<Employee> GetAllEmployees()
    {
        var emp =   from employee in Employees
                    where employee.DepartmentID == 100
                    select employee;
        return emp;
    }

    private static void InitEmployees()
    {
        Employees.Add(
            new
            (
                "John Souls", 
                250000d,
                100 + Employees.Count,
                102
            )
        );

        Employees.Add(
            new
            (
                "Richard Aram", 
                25000d,
                100 + Employees.Count,
                102,
                false
            )
        );

        Employees.Add(
            new
            (
                "Jane Doe",
                90000d,
                100 + Employees.Count,
                100
            )
        );

        Employees.Add(
            new
            (
                "Robert Copper",
                90000d,
                100 + Employees.Count,
                100
            )
        );

        Employees.Add(
            new
            (
                "Squidward Tentacles",
                25000d,
                100 + Employees.Count,
                105,
                false
            )
        );

        Employees.Add(
            new
            (
                "Tomoya Okazaki", 
                95000d,
                100 + Employees.Count,
                102
            )
        );

        Employees.Add(
            new
            (
                "Mary Yumekui", 
                500000d,
                100 + Employees.Count,
                100
            )
        );

        Employees.Add(
            new
            (
                "John Halo",
                900000d,
                100 + Employees.Count,
                100
            )
        );

        Employees.Add(
            new
            (
                "John MacTavish",
                120000d,
                100 + Employees.Count,
                100
            )
        );

        Employees.Add(
            new
            (
                "Peter Griffin",
                95000d,
                100 + Employees.Count,
                105
            )
        );

        Employees.Add(
            new
            (
                "Gordon Freeman", 
                250000d,
                100 + Employees.Count,
                100
            )
        );

        Employees.Add(
            new
            (
                "Ronald Monhiyan", 
                25000d,
                100 + Employees.Count,
                102,
                false
            )
        );

        Employees.Add(
            new(
                "Sikhs Evans",
                90000d,
                100 + Employees.Count,
                100,
                false
            )
        );

        Employees.Add(
            new
            (
                "Lorem Ipsum",
                90000d,
                100 + Employees.Count,
                100
            )
        );

        Employees.Add(
            new
            (
                "Ipissimus Missimus",
                25000d,
                100 + Employees.Count,
                105,
                false
            )
        );

        Employees.Add(
            new
            (
                "Adam West", 
                250000d,
                100 + Employees.Count,
                102
            )
        );

        Employees.Add(
            new
            (
                "Brocktree Brockhall", 
                250000d,
                100 + Employees.Count,
                100,
                false
            )
        );

        Employees.Add(
            new
            (
                "Wario Wario",
                125000d,
                100 + Employees.Count,
                100
            )
        );

        Employees.Add(
            new
            (
                "Marisa Kirisame",
                250000d,
                100 + Employees.Count,
                100
            )
        );

        Employees.Add(
            new
            (
                "Reimu Hakurei",
                250000d,
                100 + Employees.Count,
                100
            )
        );

    }

    
    private static double GetMinimumSalary() => (from employee in Employees select employee.Salary).Min();
    private static int GetTotalDepartmentEmployees(int departmentID) => GetEmployeesFromDepartment(departmentID).Count();

}
