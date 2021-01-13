using System;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class ApplicationDbContext: DbContext
    {
        public DbSet<Pizza> Pizzas { get; set; } = null!;
        
        public DbSet<Order> Orders { get; set; } = null!;
        
        public DbSet<AddComponent> AddComponents { get; set; } = null!;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        
        
        

    }
}