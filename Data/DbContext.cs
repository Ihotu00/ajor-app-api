using Microsoft.EntityFrameworkCore;
using AjorApi.Models;

namespace AjorApi.Data;

public class DBContext : DbContext
{
    public int TenantId { get; set; }
    private readonly ITenantService _tenantService;
    public DBContext(DbContextOptions<DBContext> options, ITenantService tenantService)
        : base(options)
    {
        _tenantService = tenantService;
        TenantId = _tenantService.GetOrganization();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Users>().Property(p => p.Id).ValueGeneratedOnAdd();
        modelBuilder.Entity<Users>().HasQueryFilter(a => a.OrganizationId == TenantId);
        modelBuilder.Entity<Contribution>().HasQueryFilter(a => a.OrganizationId == TenantId);
        // modelBuilder.Entity<Contributor>().HasQueryFilter(a => a.Contributions.OrganizationId == TenantId);
        // modelBuilder.Entity<Contributor>().HasQueryFilter(a => a.Users.OrganizationId == TenantId);
    }

    public DbSet<Contribution> Contributions { get; set; } = null!;
    public DbSet<Contributor> Contributors { get; set; } = null!;
    public DbSet<Users> Users { get; set; } = null!;
}