using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Sidekick.Data.Ninja;

public class NinjaDbDesignTime : IDesignTimeDbContextFactory<NinjaDbContext>
{
    public NinjaDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<NinjaDbContext>();
        builder.UseSqlite("Filename=ninja.db");
        return new NinjaDbContext(builder.Options);
    }
}
