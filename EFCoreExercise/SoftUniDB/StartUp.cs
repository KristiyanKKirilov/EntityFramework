using Microsoft.EntityFrameworkCore;
using SoftUni.Data;
using SoftUni.DTO;
using SoftUni.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SoftUni

{
    public class StartUp
    {
        static void Main(string[] args)
        {
            using SoftUniContext context = new();

            string json = @"{""firstName"": ""Stamo"", ""lastName"": ""Petkov"", ""grade"": 4 }";
            var student = JsonConvert.DeserializeAnonymousType(json, Templates.JsonTemplates.StudentTemplate);

        }

        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            var employees = context.Employees
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.MiddleName,
                    e.JobTitle,
                    e.Salary
                }).ToList();

            var result = string.Join(Environment.NewLine, employees.Select(e=> $"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.Salary:F2}"));
            return result;

        }

        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            var employees = context.Employees.Select(e => new
            {
                e.FirstName,
                e.Salary
            }).Where(e=> e.Salary > 50000)
            .OrderBy(e=>e.FirstName)
            .ToList();

            var result = string.Join(Environment.NewLine, employees.Select(e => $"{e.FirstName} - {e.Salary:F2}"));
            return result;


        }
        
        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            var employees = context.Employees.Select(e => new
            {
                e.FirstName,
                e.LastName,
                e.Department,
                e.Salary
            }).OrderBy(e=>e.Salary)
            .ThenByDescending(e=>e.FirstName)
            .Where(e => e.Department.Name == "Research and Development")
            .ToList();

            var result = string.Join(Environment.NewLine, employees.Select(e => $"{e.FirstName} {e.LastName} from {e.Department.Name} - ${e.Salary:f2}"));
            return result;
        }

        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            Address address = new() 
            { 
                AddressText = "Vitoshka 15", 
                TownId = 4 
            };

            var employee = context.Employees.FirstOrDefault(e => e.LastName == "Nakov");

            employee.Address = address;
            context.SaveChanges();

            var employeesAddresses = context.Employees
                .Select(e => new
                {
                    e.AddressId,
                    e.Address.AddressText
                }).OrderByDescending(e=>e.AddressId)
                .Take(10)
                .ToList();

            var result = string.Join(Environment.NewLine, employeesAddresses.Select(ea =>$"{ea.AddressText}"));
            return result;
        }

        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            var employees = context.Employees
                .AsNoTracking()
                .Select(e=> new
                {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    e.Salary
                })/*.Where(e => e.FirstName.StartsWith("Sa")*/
                .Where(e => EF.Functions.Like(e.FirstName, "sa%")
                ).OrderBy(e=>e.FirstName)
                .ThenBy(e=>e.LastName)
                .ToList();

            var result = string.Join(Environment.NewLine, employees.Select(e => $"{e.FirstName} {e.LastName} - {e.JobTitle} - (${e.Salary:F2})"));
            return result;
        }
    }
}