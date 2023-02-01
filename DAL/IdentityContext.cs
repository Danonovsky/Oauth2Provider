using Microsoft.EntityFrameworkCore;
using Oauth2Provider.Entities;

namespace Oauth2Provider.DAL;

class IdentityContext : DbContext
{
    public DbSet<User> Users {get;set;} = null!;
    public IdentityContext(DbContextOptions options) : base(options) {}
    public IdentityContext() : base(){}
}