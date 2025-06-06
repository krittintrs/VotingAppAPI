using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace VotingAppAPI.Data
{
    public class VotingDbContextFactory : IDesignTimeDbContextFactory<VotingDbContext>
    {
        public VotingDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<VotingDbContext>();
            optionsBuilder.UseSqlServer("Server=localhost;Database=VotingAppDb;Trusted_Connection=True;TrustServerCertificate=True;");

            return new VotingDbContext(optionsBuilder.Options);
        }
    }
}

