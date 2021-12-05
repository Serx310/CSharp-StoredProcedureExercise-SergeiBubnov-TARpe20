using Microsoft.EntityFrameworkCore;
using StoredProcedure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoredProcedure.Data
{
    public class StoredProcedureDBContext : DbContext
    {
        public StoredProcedureDBContext(DbContextOptions<StoredProcedureDBContext> options) : base(options) {}

        public DbSet<Employee> Employee { get; set; }
    }
}
