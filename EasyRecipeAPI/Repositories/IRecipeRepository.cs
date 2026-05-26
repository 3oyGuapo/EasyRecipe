using RecipeData;

namespace EasyRecipeAPI.Repositories
{
    public interface IRecipeRepository
    {
        Task<IEnumerable<Recipe>> GetAllRecipesAsync();
        Task<Recipe?> GetRecipeByIdAsync(int id);
        Task AddRecipeAsync(Recipe newRecipe);
        Task DeleteRecipeAsync(Recipe deleteRecipe);
        Task<Tag?> GetTagByNameAsync(string name);
        Task SaveChangesAsync();
        Task<(IEnumerable<Recipe> Items, int TotalCount)> GetPagedRecipesAsync(RecipeQueryParameters query);
    }
}
