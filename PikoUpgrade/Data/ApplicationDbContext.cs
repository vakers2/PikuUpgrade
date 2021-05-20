using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PikoUpgrade.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PikoUpgrade.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }

        public DbSet<Room> Rooms { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Room>().Property(x => x.Id).HasDefaultValueSql("NEWID()");

            builder
                .Entity<Room>()
                .HasMany(p => p.Participators)
                .WithMany(p => p.Rooms)
                .UsingEntity(j => j.ToTable("UserRooms"));

            builder.Entity<Room>()
                .HasOne(p => p.Host);
        }
    }
}
