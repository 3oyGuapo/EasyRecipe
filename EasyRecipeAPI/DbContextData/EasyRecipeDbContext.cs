using Microsoft.EntityFrameworkCore;
using RecipeData;

namespace EasyRecipeAPI.DbContextData
{
    public class EasyRecipeDbContext : DbContext
    {
        public EasyRecipeDbContext(DbContextOptions<EasyRecipeDbContext> options) : base(options)
        {

        }

        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Step> Steps { get; set; }
        public DbSet<Tag> Tags { get; set; }


        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var auditableEntries = ChangeTracker.Entries<IAuditable>()
                .Where(entry => entry.State == EntityState.Added || entry.State == EntityState.Modified);

            foreach (var auditableEntry in auditableEntries)
            {
                if (auditableEntry.State == EntityState.Added)
                {
                    auditableEntry.Entity.CreatedAt = DateTime.UtcNow;
                }

                else if (auditableEntry.State == EntityState.Modified)
                {
                    auditableEntry.Entity.LastModifiedAt = DateTime.UtcNow;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
