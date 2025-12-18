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
    }
}
