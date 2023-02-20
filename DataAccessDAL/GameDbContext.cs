using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ModelsEL;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessDAL
{
    public class GameDbContext : DbContext
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public DbSet<User> Users { get; set; }

        [Key]
        public DbSet<Gamelog> Gamelogs { get; set; }

        [Key]
        public DbSet<Scenario> Scenarios { get; set; }

        [Key]
        public DbSet<PossibleDealerHands> PossibleDealerHands { get; set; }

        [Key]
        public DbSet<PossiblePlayerHands> PossiblePlayerHands { get; set; }

        public GameDbContext(DbContextOptions<GameDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=GameDb");
        }
    }

    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<GameDbContext>
    {
        public GameDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<GameDbContext>();
            var connectionString = "Server =.;Database = BlackJackDB;User Id=admin;Password=password;Encrypt=False;";
            return new GameDbContext(optionsBuilder.Options);
        }
    }
}