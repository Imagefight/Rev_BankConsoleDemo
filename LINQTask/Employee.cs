
namespace TheThing
{    
    public class Employee (string name, double salary, int id, int deptID, bool isPermanent = true)
    {
        public string Name {get; set;} = name;
        public int EmployeeID {get; set;} = id;
        public int DepartmentID {get; set;} = deptID;
        public double Salary {get; set;} = salary;
        public bool IsPermanent {get; set;} = isPermanent;
    }
}