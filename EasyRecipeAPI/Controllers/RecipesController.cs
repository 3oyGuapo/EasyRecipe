using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EasyRecipeAPI.DbContextData;
using RecipeData;
using Microsoft.EntityFrameworkCore;

namespace EasyRecipeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipesController : ControllerBase
    {
        private readonly EasyRecipeDbContext _context;

        public RecipesController(EasyRecipeDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Recipe>>> GetRecipes()
        {
            var recipes = await _context.Recipes
                .Include(recipe => recipe.IngredientsList)
                .Include(recipe => recipe.StepsList)
                .Include(recipe => recipe.TagsList)
                .ToListAsync();

            return Ok(recipes);
        }

        [HttpPost]
        public async Task<ActionResult<Recipe>> AddRecipe(CreateRecipeDto newRecipeDto)
        {
            Recipe newRecipe = new Recipe()
            {
                RecipeName = newRecipeDto.Name
            };

            newRecipe.IngredientsList = [];
            newRecipe.StepsList = [];
            newRecipe.TagsList = [];

            //Add ingredients
            foreach (CreateIngredientDto ingredientDto in newRecipeDto.IngredientsList)
            {
                //Ingredient ingredient = new Ingredient() { Name = ingredientDto.Name, UnitAmount = ingredientDto.UnitAmount };
                newRecipe.IngredientsList.Add(new Ingredient() { Name = ingredientDto.Name, UnitAmount = ingredientDto.UnitAmount });
            }

            //Add steps
            foreach (CreateStepDto stepDto in newRecipeDto.StepsList)
            {
                //Step step = new Step() { StepContent = stepDto.StepContent, StepOrder = stepDto.StepOrder };
                newRecipe.StepsList.Add(new Step() { StepContent = stepDto.StepContent, StepOrder = stepDto.StepOrder });
            }

            foreach (string tagName in newRecipeDto.TagsList)
            {
                Tag existingTag = await _context.Tags.FirstOrDefaultAsync(t => t.Name == tagName);
                
                if (existingTag != null)
                {
                    newRecipe.TagsList.Add(existingTag);
                }

                else
                {
                    Tag newTag = new Tag { Name = tagName };
                    newRecipe.TagsList.Add(newTag);
                }
            }

            _context.Recipes.Add(newRecipe);

            await _context.SaveChangesAsync();

            //For debugging
            /*
            Console.WriteLine($"[Debug] Recipe Name: {newRecipe.RecipeName}");
            Console.WriteLine($"[Debug] Tags Count in Memory: {newRecipe.TagsList?.Count ?? -1}");

            if (newRecipe.TagsList != null)
            {
                foreach (var t in newRecipe.TagsList)
                {
                    Console.WriteLine($"[Debug] Tag: {t.Name}, ID: {t.ID}");
                }
            }*/

            return Ok(newRecipe);
        }
    }
}
