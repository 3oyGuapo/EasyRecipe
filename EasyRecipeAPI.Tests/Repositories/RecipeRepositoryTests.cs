using EasyRecipeAPI.DbContextData;
using EasyRecipeAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using RecipeData;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyRecipeAPI.Tests.Repositories
{
    public class RecipeRepositoryTests
    {
        private DbContextOptions<EasyRecipeDbContext> CreateNewInMemoryDatabaseOptions()
        {
            return new DbContextOptionsBuilder<EasyRecipeDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task GetAllRecipesAsync_ShouldReturnAllRecipesWithIncludedRelations()
        {
            // 1. Arrange
            var dbOptions = CreateNewInMemoryDatabaseOptions();

            using (var context = new EasyRecipeDbContext(dbOptions))
            {
                var sampleRecipe = new Recipe
                {
                    RecipeName = "Beef noodle",
                    IngredientsList = new List<Ingredient>
                    {
                        new Ingredient { Name = "Beef", UnitAmount = "200g" },
                        new Ingredient { Name = "Noodles", UnitAmount = "400g" }
                    },
                    StepsList = new List<Step>
                    {
                        new Step { StepContent = "Cook beef", StepOrder = 1 },
                        new Step { StepContent = "Cook noodles", StepOrder = 2 },
                        new Step { StepContent = "Put beef and noodles together with flavours", StepOrder = 3 }
                    },
                    TagsList = new List<Tag>
                    {
                        new Tag { Name = "Asian" },
                        new Tag { Name = "Main Dish" }
                    }
                };

                context.Recipes.Add(sampleRecipe);
                await context.SaveChangesAsync();
            }

            // 2. Act
            List<Recipe> resultRecipes;

            using (var context = new EasyRecipeDbContext(dbOptions))
            {
                var repository = new RecipeRepository(context);

                var recipes = await repository.GetAllRecipesAsync();
                resultRecipes = recipes.ToList();
            }

            // 3. Assert
            Assert.Single(resultRecipes);

            var recipe = resultRecipes[0];
            Assert.Equal("Beef noodle", recipe.RecipeName);

            Assert.NotNull(recipe.IngredientsList);
            Assert.Equal(2, recipe.IngredientsList.Count);
            Assert.Contains(recipe.IngredientsList, i => i.Name == "Beef");

            Assert.NotNull(recipe.StepsList);
            Assert.Equal(3, recipe.StepsList.Count);
            Assert.Equal(1, recipe.StepsList[0].StepOrder);

            Assert.NotNull(recipe.TagsList);
            Assert.Equal(2, recipe.TagsList.Count);
            Assert.Contains(recipe.TagsList, t => t.Name == "Main Dish");
        }
    }
}
