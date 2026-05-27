using RecipeData.Dtos;
using RecipeData.Entities;
using RecipeData.Parameters;
using RecipeData.Results;

namespace EasyRecipeAPI.Services
{
    public interface IRecipeService
    {
        Task<IEnumerable<RecipeResponseDto>> GetAllRecipesAsync();
        Task<RecipeResponseDto?> GetRecipeByIdAsync(int id);
        Task<RecipeResponseDto> CreateRecipeAsync(CreateRecipeDto newRecipeDto);
        Task UpdateRecipeByIdAsync(int id, CreateRecipeDto updateRecipeDto);
        Task DeleteRecipeByIdAsync(int id);
        Task<PagedResult<RecipeResponseDto>> GetPagedRecipesAsync(RecipeQueryParameters query);
    }
}
