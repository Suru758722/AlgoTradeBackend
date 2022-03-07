using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SudhirTest.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<SymbolData> SymbolData { get; set; }
        public DbSet<Symbol> Symbol { get; set; }

    }
}


public class SymbolData
{
    [Key]
    public int Id { get; set; }
    public int? SymbolId { get; set; }
    public double Price { get; set; }
    public DateTime Time { get; set; }
    [ForeignKey("SymbolId")]
    public virtual Symbol Symbol { get; set; }
}

public class Symbol
{
    [Key]
    public int Id { get; set; }
    public string SymbolCode { get; set; }
    public string SymbolName { get; set; }

}