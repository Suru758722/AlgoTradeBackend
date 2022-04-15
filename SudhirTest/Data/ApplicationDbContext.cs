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
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            foreach (var entity in builder.Model.GetEntityTypes())
            {
                // Replace table names
             //   entity.Relational().TableName = entity.Relational().TableName.ToSnakeCase();
                  entity.SetTableName(entity.GetTableName().ToLower());
                // Replace column names            
                foreach (var property in entity.GetProperties())
                {
                    property.SetColumnName(property.GetColumnName().ToLower());
                  //  property.Relational().ColumnName = property.Relational().ColumnName.ToSnakeCase();
                }

                //foreach (var key in entity.GetKeys())
                //{
                //    key.Relational().Name = key.Relational().Name.ToSnakeCase();
                //}

                //foreach (var key in entity.GetForeignKeys())
                //{
                //    key.Relational().Name = key.Relational().Name.ToSnakeCase();
                //}

                //foreach (var index in entity.GetIndexes())
                //{
                //    index.Relational().Name = index.Relational().Name.ToSnakeCase();
                //}
            }
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
        public DbSet<LT> LT { get; set; }
        public DbSet<KotakBank> KotakBank { get; set; }
        public DbSet<OptionInstrument> OptionInstrument { get; set; }
        public DbSet<OptionLtpCall> OptionLtpCall { get; set; }
        public DbSet<OptionLtpPut> OptionLtpPut { get; set; }
        public DbSet<OptionLtqCall> OptionLtqCall { get; set; }
        public DbSet<OptionLtqPut> OptionLtqPut { get; set; }
        public DbSet<OptionOICall> OptionOICall { get; set; }
        public DbSet<OptionOIPut> OptionOIPut { get; set; }
        public DbSet<OptionIVCall> OptionIVCall { get; set; }
        public DbSet<OptionIVPut> OptionIVPut { get; set; }
        public DbSet<NiftyFut> NiftyFut { get; set; }
    }
}


