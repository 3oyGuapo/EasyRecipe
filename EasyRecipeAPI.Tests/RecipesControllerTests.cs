using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Microsoft.EntityFrameworkCore;
using EasyRecipeAPI.DbContextData;
using RecipeData;
using EasyRecipeAPI;
using EasyRecipeAPI.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace EasyRecipeAPI.Tests
{
    public class RecipesControllerTests
    {
        private readonly DbContextOptions<EasyRecipeDbContext> _dbOptions;

        public RecipesControllerTests()
        {
            _dbOptions = new DbContextOptionsBuilder<EasyRecipeDbContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task GetRecipes_ReturnAllRecipes()
        {
            //Arrange
            using (var context = new EasyRecipeDbContext(_dbOptions))
            {
                context.Recipes.Add(new Recipe { RecipeName = "Test Recipe Name 1" });
                context.Recipes.Add(new Recipe { RecipeName = "Test Recipe Name 2" });

                //Save changes to the inMemory database
                await context.SaveChangesAsync();
            }

            //Act
            using (var context = new EasyRecipeDbContext(_dbOptions))
            {
                var controller = new RecipesController(context);

                var result = await controller.GetRecipes();

                //Assert
                var actionResult = Assert.IsType<ActionResult<IEnumerable<Recipe>>>(result);

                var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);

                var recipes = Assert.IsAssignableFrom<IEnumerable<Recipe>>(okResult.Value);

                //Check does it contain exactly 2 recipes
                Assert.Equal(2, recipes.Count());
            }
        }
    }
}
