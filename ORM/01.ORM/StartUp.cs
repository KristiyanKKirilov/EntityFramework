using _01.ORM.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace _01.ORM
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            ApplicationDBContext db = new ();

            var towns = db.Towns
                .Include(t=> t.Country);

            Console.WriteLine(towns.ToQueryString());
            Console.WriteLine("---------------------------");

            foreach (var town in towns)
            {
                Console.WriteLine($"{town.Name} is in {town.Country?.Name}");
            }
        }
    }
}