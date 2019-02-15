using BoilerPlate.ModelLayer.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BoilerPlate.ModelLayer.Identity
{
    
    public class BoilerPlateDbContext : DbContext
    {
        
        public DbSet<USER> Users { get; set; }
     

        public BoilerPlateDbContext(DbContextOptions<BoilerPlateDbContext> options) : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
                                  

            foreach (Microsoft.EntityFrameworkCore.Metadata.IMutableForeignKey relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

           


        }


    }
}
