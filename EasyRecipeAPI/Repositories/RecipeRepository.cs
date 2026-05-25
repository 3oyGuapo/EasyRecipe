using EasyRecipeAPI.DbContextData;
using Microsoft.EntityFrameworkCore;
using RecipeData;

namespace EasyRecipeAPI.Repositories
{
    public class RecipeRepository : IRecipeRepository
    {
        private readonly EasyRecipeDbContext _context;

        public RecipeRepository(EasyRecipeDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Recipe>> GetAllRecipesAsync()
        {
            return await _context.Recipes
                .Include(recipe => recipe.IngredientsList)
                .Include(recipe => recipe.StepsList)
                .Include(recipe => recipe.TagsList)
                .ToListAsync();
        }

        public async Task<Recipe?> GetRecipeByIdAsync(int id)
        {
            return await _context.Recipes
                .Include(recipe => recipe.IngredientsList)
                .Include(recipe => recipe.StepsList)
                .Include(recipe => recipe.TagsList)
                .FirstOrDefaultAsync(recipe => recipe.ID == id);
        }

        public async Task AddRecipeAsync(Recipe newRecipe)
        {
            await _context.Recipes.AddAsync(newRecipe);
        }

        public async Task DeleteRecipeAsync(Recipe recipeToDelete)
        {
            recipeToDelete.IsDeleted = true;
            recipeToDelete.DeletedAt = DateTime.UtcNow;
            await Task.CompletedTask;
        }

        public async Task<Tag?> GetTagByNameAsync(string name)
        {
            return await _context.Tags.FirstOrDefaultAsync(tag => tag.Name == name);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
