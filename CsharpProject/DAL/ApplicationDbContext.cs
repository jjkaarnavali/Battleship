using System;
using System.Linq;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class ApplicationDbContext: DbContext
    {
        public DbSet<Game> Games { get; set; } = null!;
        public DbSet<GameOption> GameOptions { get; set; } = null!;
        public DbSet<Player> Players { get; set; } = null!;
        public DbSet<GameOptionBoat> GameOptionBoats { get; set; } = null!;
        public DbSet<Boat> Boats { get; set; } = null!;
        public DbSet<GameBoat> GameBoats { get; set; } = null!;
    
        
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            


            modelBuilder
                .Entity<Player>()
                .HasOne<Game>()
                .WithOne(x => x.PlayerA)
                .HasForeignKey<Game>(x => x.PlayerAId);
            
            modelBuilder
                .Entity<Player>()
                .HasOne<Game>()
                .WithOne(x => x.PlayerB)
                .HasForeignKey<Game>(x => x.PlayerBId);
            /*
            modelBuilder
                .Entity<Game>()
                .HasOne<Player>()
                .WithOne(x => x.Game)
                .HasForeignKey<Player>(x => x.GameId);
            modelBuilder
                .Entity<Game>()
                .HasOne<Player>()
                .WithOne(x => x.Game)
                .HasForeignKey<Player>(x => x.GameId);
                */
            

            // remove the cascade delete
            foreach (var relationship in modelBuilder.Model
                .GetEntityTypes()
                .Where(e => !e.IsOwned())
                .SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

        }
    }
}