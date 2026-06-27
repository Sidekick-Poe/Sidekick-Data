using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Sidekick.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DataDbContext>
{
    public DataDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<DataDbContext>();
        builder.UseSqlite("Filename=../../data/data.db");
        return new DataDbContext(builder.Options);
    }
}
