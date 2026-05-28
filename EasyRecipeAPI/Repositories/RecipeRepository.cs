using EasyRecipeAPI.DbContextData;
using Microsoft.EntityFrameworkCore;
using RecipeData.Entities;
using RecipeData.Parameters;

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
                .Include(recipe => recipe.Ingredients)
                .Include(recipe => recipe.Steps)
                .Include(recipe => recipe.Tags)
                .ToListAsync();
        }

        public async Task<Recipe?> GetRecipeByIdAsync(int id)
        {
            return await _context.Recipes
                .Include(recipe => recipe.Ingredients)
                .Include(recipe => recipe.Steps)
                .Include(recipe => recipe.Tags)
                .FirstOrDefaultAsync(recipe => recipe.Id == id);
        }

        public async Task AddRecipeAsync(Recipe newRecipe)
        {
            _context.Recipes.Add(newRecipe);
            await Task.CompletedTask;
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

        public async Task<(IEnumerable<Recipe> Items, int TotalCount)> GetPagedRecipesAsync(RecipeQueryParameters query)
        {
            var baseQuery = _context.Recipes.AsQueryable();

            if (!string.IsNullOrEmpty(query.SearchQuery))
            {
                baseQuery = baseQuery.Where(recipe => recipe.RecipeName.Contains(query.SearchQuery));
            }
            
            int totalCount = await baseQuery.CountAsync();

            var items = await baseQuery
                .Include(recipe => recipe.Ingredients)
                .Include(recipe => recipe.Steps)
                .Include(recipe => recipe.Tags)
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            return (items, totalCount);
        }
    }
}
