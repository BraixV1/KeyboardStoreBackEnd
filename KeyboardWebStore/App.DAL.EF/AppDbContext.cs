using App.Domain;
using App.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF;

public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid, IdentityUserClaim<Guid>, AppUserRole,
    IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>

{
    public DbSet<AppRefreshToken> RefreshTokens { get; set; } = default!;
    public DbSet<Contest> Contests { get; set; } = default!;
    public DbSet<Category> Categories { get; set; } = default!;
    public DbSet<Keyboard> Keyboards { get; set; } = default!;
    public DbSet<KeyboardBuild> KeyboardBuilds { get; set; } = default!;
    public DbSet<KeyboardPart> KeyboardParts { get; set; } = default!;
    public DbSet<KeyboardRating> KeyboardRatings { get; set; } = default!;
    public DbSet<Order> Orders { get; set; } = default!;
    public DbSet<OrderItem> OrderItems { get; set; } = default!;
    public DbSet<Part> Parts { get; set; } = default!;
    public DbSet<PartCategory> partCategories { get; set; } = default!;
    public DbSet<Warehouse> Warehouses { get; set; } = default!;
    public DbSet<WarehousePart> WarehouseParts { get; set; } = default!;
    public DbSet<PartInBuild> PartInBuilds { get; set; } = default!;

    public DbSet<PartRating> PartRatings { get; set; } = default!;
    

    public AppDbContext(DbContextOptions options) : base(options)
    {
    }


    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entity in ChangeTracker.Entries().Where(e => e.State != EntityState.Deleted))
        {
            foreach (var prop in entity
                         .Properties
                         .Where(x => x.Metadata.ClrType == typeof(DateTime)))
            {
                Console.WriteLine(prop);
                prop.CurrentValue = ((DateTime) prop.CurrentValue).ToUniversalTime();
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}