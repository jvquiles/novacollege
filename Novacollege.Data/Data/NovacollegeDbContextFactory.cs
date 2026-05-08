using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Novacollege.Data.Data;

public class NovacollegeDbContextFactory : IDesignTimeDbContextFactory<NovacollegeDbContext>
{
    public NovacollegeDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<NovacollegeDbContext>();
        optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=Novacollege;Integrated Security=True;TrustServerCertificate=True");
        return new NovacollegeDbContext(optionsBuilder.Options);
    }
}