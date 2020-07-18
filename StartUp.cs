using SoftUni.Data;
using SoftUni.Models;
using System;
using System.Linq;
using System.Text;

namespace SoftUni
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            using (var context = new SoftUniContext())
            {
                Console.WriteLine(RemoveTown(context));
            }
        }

        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var employees = context.Employees.OrderBy(e => e.EmployeeId).Select(e => new
            {
                EmployeeInfo = $"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.Salary:F2}"
            })
                    .ToList();

            foreach (var employee in employees)
            {
                sb.AppendLine(employee.EmployeeInfo);
            }

            var result = sb.ToString().Trim();
            return result;
        }

        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var employees = context
                .Employees
                .Where(x => x.Salary > 50000)
                .OrderBy(x => x.FirstName)
                .ToList();

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} - {employee.Salary:F2}");
            }

            var result = sb.ToString().Trim();
            return result;
        }

        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var employees = context.Employees.Where(x => x.DepartmentId == Department.ResearchAndDevelopmentDepartmentId).OrderBy(x => x.Salary).ThenByDescending(x => x.FirstName).ToList();
            //var researchDepartmentName = context.Departments.FirstOrDefault(x => x.DepartmentId == Department.ResearchAndDevelopmentDepartmentId)?.Name; --> My idea
            var researchDepartmentName = "Research and Development"; //SoftUni Idea

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} from {researchDepartmentName} - ${employee.Salary:F2}");
            }

            var result = sb.ToString().Trim();
            return result;
        }

        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            var newAddress = new Address
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };

            var nakov = context.Employees.FirstOrDefault(x => x.LastName == "Nakov");
            nakov.Address = newAddress;

            context.Add(newAddress);
            context.Update(nakov);
            context.SaveChanges();

            var employeesAddresses = context
                .Employees
                .OrderByDescending(x => x.AddressId)
                .Take(10)
                .Select(e => e.Address.AddressText)
                .ToList();

            var sb = new StringBuilder();
            foreach (var address in employeesAddresses)
            {
                sb.AppendLine(address);
            }

            var result = sb.ToString().Trim();
            return result;
        }

        public static string GetAddressesByTown(SoftUniContext context)
        {
            var selectedAddresses = context.Addresses
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

            var sb = new StringBuilder();

            foreach (var address in selectedAddresses)
            {
                sb.AppendLine($"{address.Text}, {address.Town} - {address.EmployeesCount} employees");
            }

            return sb.ToString();
        }

        public static string GetEmployee147(SoftUniContext context)
        {
            var employee147 = context.Employees.First(x => x.EmployeeId == 147);
            var projectsIds = context.EmployeesProjects.Where(x => x.EmployeeId == employee147.EmployeeId).Select(x => x.ProjectId).ToList();
            var projectsNames = context.Projects.Where(x => projectsIds.Contains(x.ProjectId)).OrderBy(x => x.Name).Select(x => x.Name).ToList();

            var sb = new StringBuilder();
            sb.AppendLine($"{employee147.FirstName} {employee147.LastName} - {employee147.JobTitle}");
            foreach (var name in projectsNames)
            {
                sb.AppendLine(name);
            }

            return sb.ToString().Trim();
        }

        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            var departments = context.Departments
                .Where(x => x.Employees.Count > 5)
                .OrderBy(x => x.Employees.Count)
                .ThenBy(x => x.Name)
                .Select(x => new 
                {
                    DepartmentName = x.Name,
                    ManagerName = $"{x.Manager.FirstName} {x.Manager.LastName}",
                    Employees = x.Employees.OrderBy(e => e.FirstName).ThenBy(e => e.LastName).ToList(),
                    x.ManagerId
                })
                .ToList();

            var employees = context.Employees.ToList();

            var sb = new StringBuilder();

            foreach (var department in departments)
            {
                sb.AppendLine($"{department.DepartmentName} - {department.ManagerName}");
                foreach (var employee in department.Employees)
                {
                    if (department.ManagerId != employee.EmployeeId)
                    {
                        sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");
                    }
                }
            }

            return sb.ToString().Trim();
        }

        public static string GetLatestProjects(SoftUniContext context)
        {
            var projects = context.Projects
                .OrderByDescending(x => x.StartDate)
                .Take(10)
                .OrderBy(x => x.Name)
                .Select(x => new 
                {
                    x.Name,
                    x.Description,
                    StartDate = x.StartDate.ToString("M/d/yyyy h:mm:ss tt")
                })
                .ToList();

            var sb = new StringBuilder();
            foreach (var project in projects)
            {
                sb.AppendLine(project.Name);
                sb.AppendLine(project.Description);
                sb.AppendLine(project.StartDate);
            }

            return sb.ToString().Trim();
        }

        public static string IncreaseSalaries(SoftUniContext context)
        {
            var employees = context.Employees
                .Where(x => x.Department.Name == "Engineering" ||
                    x.Department.Name == "Tool Design" ||
                    x.Department.Name == "Marketing " ||
                    x.Department.Name == "Information Services")
                .OrderBy(x => x.FirstName)
                .ThenBy(x => x.LastName)
                .ToList();

            var sb = new StringBuilder();
            foreach (var employee in employees)
            {
                employee.Salary *= 1.12m;

                sb.AppendLine($"{employee.FirstName} {employee.LastName} (${employee.Salary:F2})");
            }

            return sb.ToString().Trim();
        }

        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            var employees = context.Employees
                .Where(x => x.FirstName.StartsWith("Sa"))
                .OrderBy(x => x.FirstName)
                .ThenBy(x => x.LastName)
                .ToList();

            var sb = new StringBuilder();
            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle} - (${employee.Salary:F2})");
            }

            return sb.ToString().Trim();
        }

        public static string DeleteProjectById(SoftUniContext context)
        {
            var deleteProject = context.Projects.First(x => x.ProjectId == 2);
            var project2Employees = context.EmployeesProjects.Where(x => x.ProjectId == 2).ToList();
            var emplIds = project2Employees.Select(x => x.EmployeeId).ToList();
            var employees = context.Employees.Where(x => emplIds.Contains(x.EmployeeId)).ToList();
            employees.ForEach(e => e.EmployeesProjects = null);
            context.UpdateRange(employees);
            context.RemoveRange(project2Employees);
            context.Remove(deleteProject);
            context.SaveChanges();

            var projects = context.Projects.Take(10).ToList();

            var sb = new StringBuilder();
            foreach (var project in projects)
            {
                sb.AppendLine(project.Name);
            }

            return sb.ToString().Trim();
        }

        public static string RemoveTown(SoftUniContext context)
        {
            var seattleTown = context.Towns.First(x => x.Name == "Seattle");
            var addresses = context.Addresses.Where(x => x.Town.TownId == seattleTown.TownId).ToList();

            var result = $"{addresses.Count()} addresses in Seattle were deleted";

            var addressesIds = addresses.Select(x => x.AddressId).ToList();
            var seattleEmployees = context.Employees.Where(x => addressesIds.Contains(x.AddressId.Value)).ToList();
            seattleEmployees.ForEach(e => e.AddressId = null);

            context.UpdateRange(seattleEmployees);
            context.RemoveRange(addresses);
            context.Remove(seattleTown);

            return result;
        }
    }
}
