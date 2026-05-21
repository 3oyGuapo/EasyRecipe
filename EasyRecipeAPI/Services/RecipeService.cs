using EasyRecipeAPI.Repositories;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.ExpressionTranslators.Internal;
using RecipeData;


namespace EasyRecipeAPI.Services
{
    public class RecipeService :IRecipeService
    {
        private readonly IRecipeRepository _repository;

        public RecipeService(IRecipeRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Recipe>> GetAllRecipesAsync()
        {
            return await _repository.GetAllRecipesAsync();
        }

        public async Task<Recipe?> GetRecipeByIdAsync(int id)
        {
            return await _repository.GetRecipeByIdAsync(id);
        }

        public async Task<Recipe> CreateRecipeAsync(CreateRecipeDto newRecipeDto)
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
                Tag? existingTag = await _repository.GetTagByNameAsync(tagName);

                if (existingTag != null)
                {
                    newRecipe.TagsList.Add(existingTag);
                }
                else
                {
                    newRecipe.TagsList.Add(new Tag() { Name = tagName });
                }
            }

            await _repository.AddRecipeAsync(newRecipe);
            await _repository.SaveChangesAsync();

            return newRecipe;
        }

        public async Task UpdateRecipeByIdAsync(int id, CreateRecipeDto updateRecipeDto)
        {
            var recipeToUpdate = await _repository.GetRecipeByIdAsync(id);

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
                Tag? existingTag = await _repository.GetTagByNameAsync(tagDto);

                if (existingTag != null)
                {
                    recipeToUpdate.TagsList.Add(existingTag);
                }
                else
                {
                    recipeToUpdate.TagsList.Add(new Tag() { Name = tagDto });
                }
            }

            await _repository.SaveChangesAsync();
        }

        public async Task DeleteRecipeByIdAsync(int id)
        {
            var recipeToDelete = await _repository.GetRecipeByIdAsync(id);

            if (recipeToDelete == null)
            {
                throw new KeyNotFoundException($"Key with ID {id} not found.");
            }

            await _repository.DeleteRecipeAsync(recipeToDelete);
            await _repository.SaveChangesAsync();
        }
    }
}
