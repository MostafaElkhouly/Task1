using Domain.Entities;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
//using System.Data.Entity.Core.Objects;

namespace Domain.Configration.EntitiesProperties
{
    public partial class AppDbContext : DbContext
    {

        
        public AppDbContext(DbContextOptions<AppDbContext>options) : base(options)
        {
        }

        public AppDbContext() : base()
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //builder.Entity<User>().ToTable("User");
           
            builder.AddAppDbProperties();


        }

        public class ApplicationContextDbFactory : IDesignTimeDbContextFactory<AppDbContext>
        {
            AppDbContext IDesignTimeDbContextFactory<AppDbContext>.CreateDbContext(string[] args)
            {
                var options = new DbContextOptionsBuilder<AppDbContext>();


                return new AppDbContext(options.Options);
            }
        }

        
    }
}
