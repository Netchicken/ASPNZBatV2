using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPNZBat.Models;

namespace ASPNZBat.Data
{
    public class SeatBookingDBContext : DbContext
    {
        public DbSet<SeatBooking> SeatBooking { get; set; }
        public DbSet<Students> Students { get; set; }
        public DbSet<AdminData> AdminData { get; set; }

        public DbSet<SiteSettings> SiteSettings { get; set; }
        public SeatBookingDBContext(DbContextOptions<SeatBookingDBContext> options)
            : base(options)
        {
        }
        public SeatBookingDBContext()
        {
        }

        //https://stackoverflow.com/questions/50657268/no-database-provider-has-been-configured-for-this-dbcontext-using-sqlite

        //public VisitorDbContext CreateDbContext(string[] args)
        //{
        //    var builder = new DbContextOptionsBuilder<VisitorDbContext>();
        //    builder.UseSqlite("Data Source=VMan.db");

        //    return new VisitorDbContext(builder.Options);
        //}

        //removed when controllers were created as it throws an error
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source = Seating.db");
        }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

        }



    }
}
