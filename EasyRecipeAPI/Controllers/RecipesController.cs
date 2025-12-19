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
    }
}
