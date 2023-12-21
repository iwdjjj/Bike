using Bike.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using System.Text.RegularExpressions;

namespace Bike.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<Address> Address { get; set; }
        public DbSet<BikeType> BikeType { get; set; }
        public DbSet<Height> Height { get; set; }
        public DbSet<MainAddress> MainAddress { get; set; }
        public DbSet<Models.Route> Routes { get; set; }

        public DbSet<CustomUser> CustomUsers { get; set; }
        public DbSet<Doljnost> Doljnosts { get; set; }

        public DbSet<Route_CountOtchet> Route_CountOtchet { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.Route>().HasOne(r => r.Address1).WithMany(a => a.Route1).HasForeignKey(r => r.AddressId1);
            modelBuilder.Entity<Models.Route>().HasOne(r => r.Address2).WithMany(a => a.Route2).HasForeignKey(r => r.AddressId2);

            modelBuilder.Entity<Models.Route>().ToTable(tb => tb.HasTrigger("Time"));
            base.OnModelCreating(modelBuilder);
        }
    }
}