using SoftUni.Data;
using SoftUni.Models;
using System;
using System.Linq;
using System.Text;

namespace SoftUni
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            SoftUniContext context = new SoftUniContext();
            //Console.WriteLine(GetEmployeesFullInformation(context));
            //Console.WriteLine(GetEmployeesWithSalaryOver50000(context));
            //Console.WriteLine(GetEmployeesFromResearchAndDevelopment(context));
            //Console.WriteLine(AddNewAddressToEmployee(context
            //Console.WriteLine(GetEmployeesInPeriod(context));
            //Console.WriteLine(GetAddressesByTown(context));
            //Console.WriteLine(GetEmployee147(context));
            //Console.WriteLine(GetDepartmentsWithMoreThan5Employees(context));
            //Console.WriteLine(GetLatestProjects(context));
            Console.WriteLine(GetEmployeesByFirstNameStartingWithSa(context));
        }
        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            StringBuilder text = new StringBuilder();
            foreach (var employee in context.Employees.OrderBy(e => e.EmployeeId))
            {
                text.AppendLine($"{employee.FirstName} {employee.LastName} {employee.MiddleName} {employee.JobTitle} {employee.Salary:F2}");
            }
            return text.ToString().TrimEnd();
        }
        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            StringBuilder text = new StringBuilder();
            foreach (var employee in context.Employees.Where(x => x.Salary > 50000).OrderBy(e => e.FirstName))
            {
                text.AppendLine($"{employee.FirstName} - {employee.Salary:F2}");
            }
            return text.ToString().TrimEnd();
        }
        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            StringBuilder text = new StringBuilder();
            var employees = context.Employees
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.Department,
                    e.Salary
                })
                .Where(e => e.Department.Name.Equals("Research and Development"))
                .OrderBy(e => e.Salary)
                .ThenByDescending(e => e.FirstName);
            foreach (var employee in employees)
            {
                text.AppendLine($"{employee.FirstName} {employee.LastName} from {employee.Department.Name} - ${employee.Salary:F2}");
            }
            return text.ToString().Trim();
        }
        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            Address address = new Address() { AddressText = "Vitoshka 15", TownId = 4 };

            context.Addresses.Add(address);

            Employee empl = context.Employees
                    .First(e => e.LastName == "Nakov");
            empl.Address = address;

            context.SaveChanges();

            var allEmpAdressess = context
                .Employees
                .OrderByDescending(x => x.AddressId)
                .Select(x => x.Address.AddressText)
                .Take(10)
                .ToArray();

            return string.Join(Environment.NewLine, allEmpAdressess);
        }
        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            var employees = context.Employees.Where(e => e.EmployeesProjects.Any(ep => ep.Project.StartDate.Year >= 2001 && ep.Project.StartDate.Year <= 2003))
                .Select(e => new
                {
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    ManagerFirstName = e.Manager.FirstName,
                    ManagerLastName = e.Manager.LastName,
                    Projects = e.EmployeesProjects.Select(ep => new
                    {
                        ProjectName = ep.Project.Name,
                        ProjectStartDate = ep.Project.StartDate,
                        ProjectEndDate = ep.Project.EndDate
                    })
                }).Take(10);

            StringBuilder employeeManagerResult = new StringBuilder();

            foreach (var employee in employees)
            {
                employeeManagerResult.AppendLine($"{employee.FirstName} {employee.LastName} - Manager: {employee.ManagerFirstName} {employee.ManagerLastName}");

                foreach (var project in employee.Projects)
                {
                    var startDate = project.ProjectStartDate.ToString("M/d/yyyy h:mm:ss tt");
                    var endDate = project.ProjectEndDate.HasValue ? project.ProjectEndDate.Value.ToString("M/d/yyyy h:mm:ss tt") : "not finished";

                    employeeManagerResult.AppendLine($"--{project.ProjectName} - {startDate} - {endDate}");
                }
            }
            return employeeManagerResult.ToString().TrimEnd();

        }
        public static string GetAddressesByTown(SoftUniContext context)
        {
            var addresses = context.Addresses
                    .OrderByDescending(a => a.Employees.Count)
                    .ThenBy(a => a.Town.Name)
                    .ThenBy(a => a.AddressText)
                    .Take(10)
                    .Select(a => new
                    {
                        Text = a.AddressText,
                        Town = a.Town.Name,
                        EmployeesCount = a.Employees.Count
                    })
                    .ToList();
            StringBuilder text = new StringBuilder();
            foreach (var address in addresses)
            {
                text.AppendLine($"{address.Text}, {address.Town} - {address.EmployeesCount} employees");
            }
            return text.ToString().Trim();
        }
        public static string GetEmployee147(SoftUniContext context) 
        {
            var empl = context.Employees
                .Where(x => x.EmployeeId == 147)
                .Select(x => new
                {
                    x.FirstName,
                    x.LastName,
                    x.JobTitle,
                    Projects = x.EmployeesProjects.Select(ep => new
                    {
                        ProjectName = ep.Project.Name
                    })
                })
                .FirstOrDefault();
            StringBuilder text = new StringBuilder();
            text.AppendLine($"{empl.FirstName} {empl.LastName} - {empl.JobTitle}");
            foreach (var projectName in empl.Projects.OrderBy(x => x.ProjectName))
            {
                text.AppendLine(projectName.ProjectName);
            }
            return text.ToString().Trim();
        }
        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context) 
        {
            var departments = context.Departments
                .Select(x => new 
                {
                    x.Employees,
                    x.Name,
                    x.Manager
                })
                .Where(x => x.Employees.Count > 5)
                .OrderBy(x => x.Name);

            StringBuilder text = new StringBuilder();
            foreach (var department in departments)
            {
                text.AppendLine($"{department.Name} – {department.Manager.FirstName} {department.Manager.LastName}");
                foreach (var emplyee in department.Employees.OrderBy(x => x.FirstName).ThenBy(x => x.LastName))
                {
                    text.AppendLine($"{emplyee.FirstName} {emplyee.LastName} - {department.Name}");
                }
            }
            return text.ToString().Trim();
        }
        public static string GetLatestProjects(SoftUniContext context) 
        {
            StringBuilder text = new StringBuilder();

            var projects = context.Projects.OrderByDescending(p => p.StartDate).Take(10)
                .Select(s => new
                {
                    ProjectName = s.Name,
                    ProjectDescription = s.Description,
                    ProjectStartDate = s.StartDate
                }).OrderBy(n => n.ProjectName).ToArray();

            foreach (var project in projects)
            {
                text.AppendLine(project.ProjectName);
                text.AppendLine(project.ProjectDescription);
                text.AppendLine(project.ProjectStartDate.ToString("M/d/yyyy h:mm:ss tt"));
            }
            return text.ToString().Trim();
        }
        public static string IncreaseSalaries(SoftUniContext context) 
        {
            StringBuilder text = new StringBuilder();
            var employees = context.Employees.Where(x => x.Department.Name == "Engineering" || x.Department.Name == "Tool Design" || x.Department.Name == "Marketing" || x.Department.Name == "Information Services");
            foreach (var employee in employees)
            {
                employee.Salary *= (decimal)1.12;
            }
            foreach (var employee in employees.OrderBy(x => x.FirstName).ThenBy(x => x.LastName))
            {
                text.AppendLine($"{employee.FirstName} {employee.LastName} (${employee.Salary:F2})");
            }

            return text.ToString().Trim();
        }
        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context) 
        {
            StringBuilder text = new StringBuilder();
            var employees = context.Employees.Select(x => new
            {
                x.FirstName,
                x.LastName,
                x.Salary,
                x.JobTitle
            })
                .Where(x => x.FirstName.StartsWith("Sa"))
                .OrderBy(x => x.FirstName).ThenBy(x => x.LastName);
            foreach (var emplyee in employees)
            {
                text.AppendLine($"{emplyee.FirstName} {emplyee.LastName} - {emplyee.JobTitle} - (${emplyee.Salary:F2})");
            }
            return text.ToString().Trim();
        }
        public static string DeleteProjectById(SoftUniContext context) 
        {
            StringBuilder text = new StringBuilder();

            var project = context.Projects.First(p => p.ProjectId == 2);

            context.EmployeesProjects.ToList().ForEach(ep => context.EmployeesProjects.Remove(ep));
            context.Projects.Remove(project);
            foreach (var item in context.Projects.Take(10))
            {
                text.AppendLine(item.Name);
            }

            return text.ToString().Trim();
        }
        public static string RemoveTown(SoftUniContext context) 
        {
            var town = context.Towns.Where(x => x.Name == "Seattle").FirstOrDefault();
            string townName = town.Name;

            context.Employees
                    .Where(e => e.Address.Town.Name == townName)
                    .ToList()
                    .ForEach(e => e.AddressId = null);

            int addressesCount = context.Addresses
                .Where(a => a.Town.Name == townName)
                .Count();

            context.Addresses
                .Where(a => a.Town.Name == townName)
                .ToList()
                .ForEach(a => context.Addresses.Remove(a));

            context.Towns
                .Remove(context.Towns
                    .SingleOrDefault(t => t.Name == townName));

            context.SaveChanges();

            return $"{addressesCount} addresses in Seattle were deleted";
        }
    }
}
