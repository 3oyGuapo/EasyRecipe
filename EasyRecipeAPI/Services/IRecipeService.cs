using RecipeData;

namespace EasyRecipeAPI.Services
{
    public interface IRecipeService
    {
        Task<IEnumerable<Recipe>> GetAllRecipesAsync();
        Task<Recipe?> GetRecipeByIdAsync(int id);
        Task<Recipe> CreateRecipeAsync(CreateRecipeDto newRecipeDto);
        Task UpdateRecipeByIdAsync(int id, CreateRecipeDto updateRecipeDto);
        Task DeleteRecipeByIdAsync(int id);
        Task<PagedResult<Recipe>> GetPagedRecipesAsync(RecipeQueryParameters query);
    }
}
