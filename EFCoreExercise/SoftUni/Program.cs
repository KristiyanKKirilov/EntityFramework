using Microsoft.EntityFrameworkCore;
using SoftUni.Data;
using System;

namespace SoftUni
{
    public class Program
    {
        static void Main(string[] args)
        {
           SoftUniContext context = new SoftUniContext();
            var result = GetEmployeesFullInformation(context);
            Console.WriteLine(result);
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
            return string.Join(Environment.NewLine, employees.Select(e=> $"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.Salary:f2}"));
        }
    }
}