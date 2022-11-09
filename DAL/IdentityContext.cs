using Microsoft.EntityFrameworkCore;

class IdentityContext : DbContext
{
    public DbSet<User> Users {get;set;} = null!;
    public IdentityContext(DbContextOptions options) : base(options) {}
}