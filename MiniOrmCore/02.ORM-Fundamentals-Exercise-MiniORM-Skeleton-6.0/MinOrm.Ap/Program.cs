using MinOrm.Ap.Entities;

namespace MinOrm.Ap
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var connectionString =
                "Server=DESKTOP-M5SEPFK\\SQLEXPRESS;Database=MiniORM;Integrated Security=True;TrustServerCertificate=True;";

            var softUniContext = new SoftUniDbContext(connectionString);

            var employee = softUniContext.Employees.FirstOrDefault();
            Console.WriteLine(employee.FirstName);

        }
    }
}