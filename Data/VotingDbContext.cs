using Microsoft.EntityFrameworkCore;
using VotingAppAPI.Models;

namespace VotingAppAPI.Data
{
    public class VotingDbContext : DbContext
    {
        public VotingDbContext(DbContextOptions<VotingDbContext> options)
            : base(options)
        {
        }

        public DbSet<Vote> Votes { get; set; }
        public DbSet<Option> Options { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Vote>()
                .HasMany(v => v.Options)
                .WithOne(o => o.Vote)
                .HasForeignKey(o => o.VoteId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
