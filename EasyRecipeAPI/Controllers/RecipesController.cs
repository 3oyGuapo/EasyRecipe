using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EasyRecipeAPI.DbContextData;
using Microsoft.EntityFrameworkCore;
using EasyRecipeAPI.Services;
using RecipeData.Dtos;
using RecipeData.Results;
using RecipeData.Parameters;
using RecipeData.Entities;

namespace EasyRecipeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipesController : ControllerBase
    {
        private readonly IRecipeService _recipeService;

        public RecipesController(IRecipeService service)
        {
            _recipeService = service;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<RecipeResponseDto>> GetRecipe(int id)
        {
            var recipe = await _recipeService.GetRecipeByIdAsync(id);

            if (recipe == null)
            {
                return NotFound();
            }

            return Ok(recipe);
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<RecipeResponseDto>>> GetRecipes([FromQuery] RecipeQueryParameters query)
        {
            var pagedResult = await _recipeService.GetPagedRecipesAsync(query);
            return Ok(pagedResult);
        }

        [HttpPost]
        public async Task<ActionResult<RecipeResponseDto>> AddRecipe(CreateRecipeDto newRecipeDto)
        {
            var createdRecipe = await _recipeService.CreateRecipeAsync(newRecipeDto);

            return CreatedAtAction(nameof(GetRecipe), new { id = createdRecipe.Id }, createdRecipe);
        }

        /// <summary>
        /// Delete method that deletes a recipe from the database
        /// </summary>
        /// <param name="id">Id use to find the target recipe that wants to delete</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipe(int id)
        {
            await _recipeService.DeleteRecipeByIdAsync(id);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRecipe(int id, CreateRecipeDto newRecipeDto)
        {
            await _recipeService.UpdateRecipeByIdAsync(id, newRecipeDto);
            return NoContent();
        }
    }
}
