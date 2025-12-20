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
            var recipes = await _context.Recipes.Include(recipe => recipe.IngredientsList).Include(recipe => recipe.StepsList).ToListAsync();

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

            foreach (CreateIngredientDto ingredientDto in newRecipeDto.IngredientsList)
            {
                //Ingredient ingredient = new Ingredient() { Name = ingredientDto.Name, UnitAmount = ingredientDto.UnitAmount };
                newRecipe.IngredientsList.Add(new Ingredient() { Name = ingredientDto.Name, UnitAmount = ingredientDto.UnitAmount });
            }

            foreach (CreateStepDto stepDto in newRecipeDto.StepsList)
            {
                //Step step = new Step() { StepContent = stepDto.StepContent, StepOrder = stepDto.StepOrder };
                newRecipe.StepsList.Add(new Step() { StepContent = stepDto.StepContent, StepOrder = stepDto.StepOrder });
            }

            _context.Recipes.Add(newRecipe);

            await _context.SaveChangesAsync();

            return Ok(newRecipe);
        }
    }
}
