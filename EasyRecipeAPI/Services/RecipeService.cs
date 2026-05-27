using EasyRecipeAPI.Repositories;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.ExpressionTranslators.Internal;
using RecipeData.Dtos;
using RecipeData.Entities;
using RecipeData.Parameters;
using RecipeData.Results;


namespace EasyRecipeAPI.Services
{
    public class RecipeService :IRecipeService
    {
        private readonly IRecipeRepository _recipeRepository;

        public RecipeService(IRecipeRepository repository)
        {
            _recipeRepository = repository;
        }

        public async Task<IEnumerable<RecipeResponseDto>> GetAllRecipesAsync()
        {
            var recipes = await _recipeRepository.GetAllRecipesAsync();

            return recipes.Select(MapToResponseDto);
        }

        public async Task<RecipeResponseDto?> GetRecipeByIdAsync(int id)
        {
            var recipe = await _recipeRepository.GetRecipeByIdAsync(id);
            if (recipe == null)
            {
                return null;
            }

            return MapToResponseDto(recipe);
        }

        public async Task<RecipeResponseDto> CreateRecipeAsync(CreateRecipeDto newRecipeDto)
        {
            Recipe newRecipe = new Recipe()
            {
                RecipeName = newRecipeDto.Name,
                IngredientsList = new List<Ingredient>(),
                StepsList = new List<Step>(),
                TagsList = new List<Tag>()
            };

            foreach (var ingredientDto in newRecipeDto.IngredientsList)
            {
                newRecipe.IngredientsList.Add(new Ingredient()
                    {
                    Name = ingredientDto.Name,
                    UnitAmount = ingredientDto.UnitAmount
                });
            }

            foreach (var stepDto in newRecipeDto.StepsList)
            {
                newRecipe.StepsList.Add(new Step()
                {
                    StepContent = stepDto.StepContent,
                    StepOrder = stepDto.StepOrder
                });
            }

            foreach (string tagName in newRecipeDto.TagsList)
            {
                Tag? existingTag = await _recipeRepository.GetTagByNameAsync(tagName);

                if (existingTag != null)
                {
                    newRecipe.TagsList.Add(existingTag);
                }
                else
                {
                    newRecipe.TagsList.Add(new Tag() { Name = tagName });
                }
            }

            await _recipeRepository.AddRecipeAsync(newRecipe);
            await _recipeRepository.SaveChangesAsync();

            return MapToResponseDto(newRecipe);
        }

        public async Task UpdateRecipeByIdAsync(int id, CreateRecipeDto updateRecipeDto)
        {
            var recipeToUpdate = await _recipeRepository.GetRecipeByIdAsync(id);

            if (recipeToUpdate == null)
            {
                throw new KeyNotFoundException($"Recipe with ID {id} not found.");
            }

            recipeToUpdate.RecipeName = updateRecipeDto.Name;

            recipeToUpdate.IngredientsList.Clear();
            recipeToUpdate.StepsList.Clear();
            recipeToUpdate.TagsList.Clear();

            foreach (var ingredientDto in updateRecipeDto.IngredientsList)
            {
                recipeToUpdate.IngredientsList.Add(new Ingredient()
                {
                    Name = ingredientDto.Name,
                    UnitAmount = ingredientDto.UnitAmount
                });
            }

            foreach (var stepDto in updateRecipeDto.StepsList)
            {
                recipeToUpdate.StepsList.Add(new Step() 
                {
                    StepContent = stepDto.StepContent,
                    StepOrder = stepDto.StepOrder
                });
            }

            foreach (string tagDto in updateRecipeDto.TagsList)
            {
                Tag? existingTag = await _recipeRepository.GetTagByNameAsync(tagDto);

                if (existingTag != null)
                {
                    recipeToUpdate.TagsList.Add(existingTag);
                }
                else
                {
                    recipeToUpdate.TagsList.Add(new Tag() { Name = tagDto });
                }
            }

            await _recipeRepository.SaveChangesAsync();
        }

        public async Task DeleteRecipeByIdAsync(int id)
        {
            var recipeToDelete = await _recipeRepository.GetRecipeByIdAsync(id);

            if (recipeToDelete == null)
            {
                throw new KeyNotFoundException($"Key with ID {id} not found.");
            }

            await _recipeRepository.DeleteRecipeAsync(recipeToDelete);
            await _recipeRepository.SaveChangesAsync();
        }

        public async Task<PagedResult<RecipeResponseDto>> GetPagedRecipesAsync(RecipeQueryParameters query)
        {
            var (items, totalCount) = await _recipeRepository.GetPagedRecipesAsync(query);

            return new PagedResult<RecipeResponseDto>
            {
                Items = items.Select(MapToResponseDto).ToList(),
                TotalCount = totalCount,
                PageSize = query.PageSize,
                CurrentPage = query.PageNumber
            };
            
        }



        private RecipeResponseDto MapToResponseDto(Recipe recipe)
        {
            return new RecipeResponseDto
            {
                Id = recipe.ID,
                Ingredients = recipe.IngredientsList.Select(ingredient => new IngredientResponseDto
                {
                    Name = ingredient.Name,
                    UnitAmount = ingredient.UnitAmount
                }).ToList(),
                Steps = recipe.StepsList.Select(step => new StepResponseDto
                {
                    StepContent = step.StepContent,
                    StepOrder = step.StepOrder
                }).ToList(),
                Tags = recipe.TagsList.Select(tag => tag.Name).ToList(),
                CreatedAt = recipe.CreatedAt
            };
        }
    }
}
