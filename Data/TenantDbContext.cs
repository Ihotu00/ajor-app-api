using AjorApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AjorApi.Data;

public class TenantDbContext : DbContext
{
    public TenantDbContext(DbContextOptions<TenantDbContext> options)
        : base(options)
    {
    }


    public DbSet<Organization> Organizations { get; set; } = null!;
}