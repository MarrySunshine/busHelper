using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Entity;

namespace EasyNavigator.Libs
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Schemas.AddressSchema> AddressItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=Address.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Make Blog.Url required
            modelBuilder.Entity<Schemas.AddressSchema>()
                .Property(b => b.Id)
                .IsRequired();
        }
    }
}
