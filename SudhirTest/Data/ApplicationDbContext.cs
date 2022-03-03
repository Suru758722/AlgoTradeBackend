using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SudhirTest.Data
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Test> Test { get; set; }
    }
}


public class Test
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
}