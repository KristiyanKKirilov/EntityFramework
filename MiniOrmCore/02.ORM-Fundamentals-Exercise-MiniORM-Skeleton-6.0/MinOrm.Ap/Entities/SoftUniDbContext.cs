using MiniORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinOrm.Ap.Entities
{
    public class SoftUniDbContext : DbContext
    {
        public SoftUniDbContext(string connectionString) 
            : base(connectionString)
        {

        }

        public DbSet<Employee> Employees { get; set; }

    }
}
