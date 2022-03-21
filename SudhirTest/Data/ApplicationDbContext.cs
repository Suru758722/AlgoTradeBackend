using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SudhirTest.Entity;
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

        public DbSet<ExchangeSegment> ExchangeSegment { get; set; }
        public DbSet<Instrument> Instrument { get; set; }
        public DbSet<InstrumentData> InstrumentData { get; set; }
        public DbSet<Reliance> Reliance { get; set; }
        public DbSet<Hdfc> Hdfc { get; set; }
        public DbSet<HdfcBank> HdfcBank { get; set; }
        public DbSet<IciciBank> IciciBank { get; set; }
        public DbSet<HindUnilvr> HindUnilvr { get; set; }
        public DbSet<Tcs> Tcs { get; set; }
        public DbSet<Sbin> Sbin { get; set; }
        public DbSet<Infy> Infy { get; set; }
        public DbSet<LT> LAndT { get; set; }
        public DbSet<KotakBank> KotakBank { get; set; }

    }
}


