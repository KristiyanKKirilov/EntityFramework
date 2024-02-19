using Intro.Models;
using Microsoft.EntityFrameworkCore;
using System;

SoftUniContext context = new();
var employee = await context.Employees
    .Include(e => e.Department)
    .Include(e => e.Manager)
    .Include(e => e.Projects)
    .Select(e=> new
    {
        Id = e.EmployeeId,
        Name = e.FirstName + " " + e.LastName,
        Department = e.Department.Name,
        Manager = e.Manager.FirstName + " " + e.Manager.LastName,
        Projects = e.Projects.Select(p => new
        {
            p.Name,
            p.StartDate,
            p.EndDate
        })
    })
    .FirstOrDefaultAsync(e => e.Id == 147);

;

//Console.WriteLine($"{employee?.FirstName} {employee?.LastName},\n" +
//    $"Department: {employee?.Department.Name},\n" +
//    $"Manager: {employee?.Manager?.FirstName} {employee?.Manager.LastName},\n" +
//    $"First Project: {employee?.Projects?.OrderBy(p=>p.StartDate)?.FirstOrDefault()?.Name}");