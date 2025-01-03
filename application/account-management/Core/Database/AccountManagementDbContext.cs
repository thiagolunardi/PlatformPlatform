using Microsoft.EntityFrameworkCore;
using PlatformPlatform.AccountManagement.Features.Authentication.Domain;
using PlatformPlatform.AccountManagement.Features.Signups.Domain;
using PlatformPlatform.AccountManagement.Features.Tenants.Domain;
using PlatformPlatform.AccountManagement.Features.Users.Domain;
using PlatformPlatform.SharedKernel.Domain;
using PlatformPlatform.SharedKernel.EntityFramework;
using PlatformPlatform.SharedKernel.ExecutionContext;

namespace PlatformPlatform.AccountManagement.Database;

public sealed class AccountManagementDbContext(DbContextOptions<AccountManagementDbContext> options, IExecutionContext executionContext)
    : SharedKernelDbContext<AccountManagementDbContext>(options, executionContext)
{
    public DbSet<Login> Logins => Set<Login>();

    public DbSet<Signup> Signups => Set<Signup>();

    public DbSet<Tenant> Tenants => Set<Tenant>();

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Login
        modelBuilder.MapStronglyTypedId<Login, LoginId, string>(t => t.Id);
        modelBuilder.MapStronglyTypedId<Login, TenantId, string>(u => u.TenantId);
        modelBuilder.MapStronglyTypedUuid<Login, UserId>(u => u.UserId);

        // Signup
        modelBuilder.MapStronglyTypedUuid<Signup, SignupId>(a => a.Id);
        modelBuilder.MapStronglyTypedNullableId<Signup, TenantId, string>(u => u.TenantId);

        // Tenant
        modelBuilder.MapStronglyTypedId<Tenant, TenantId, string>(t => t.Id);

        // User
        modelBuilder.MapStronglyTypedUuid<User, UserId>(u => u.Id);
        modelBuilder.MapStronglyTypedId<User, TenantId, string>(u => u.TenantId);
        modelBuilder.Entity<User>()
            .OwnsOne(e => e.Avatar, b => b.ToJson())
            .HasOne<Tenant>()
            .WithMany()
            .HasForeignKey(u => u.TenantId)
            .HasPrincipalKey(t => t.Id);
    }
}
