using EasyRecipeAPI.DbContextData;
using EasyRecipeAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using RecipeData.Entities;
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
                    Ingredients = new List<Ingredient>
                    {
                        new Ingredient { Name = "Beef", UnitAmount = "200g" },
                        new Ingredient { Name = "Noodles", UnitAmount = "400g" }
                    },
                    Steps = new List<Step>
                    {
                        new Step { StepContent = "Cook beef", StepOrder = 1 },
                        new Step { StepContent = "Cook noodles", StepOrder = 2 },
                        new Step { StepContent = "Put beef and noodles together with flavours", StepOrder = 3 }
                    },
                    Tags = new List<Tag>
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

            Assert.NotNull(recipe.Ingredients);
            Assert.Equal(2, recipe.Ingredients.Count);
            Assert.Contains(recipe.Ingredients, i => i.Name == "Beef");

            Assert.NotNull(recipe.Steps);
            Assert.Equal(3, recipe.Steps.Count);
            Assert.Equal(1, recipe.Steps[0].StepOrder);

            Assert.NotNull(recipe.Tags);
            Assert.Equal(2, recipe.Tags.Count);
            Assert.Contains(recipe.Tags, t => t.Name == "Main Dish");
        }

        [Fact]
        public async Task SaveChangesAsync_AuditableEntity_ShouldAutomaticallySetTimes()
        {
            // 1. Arrange
            var dbOptions = new DbContextOptionsBuilder<EasyRecipeDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var newRecipe = new Recipe
            {
                RecipeName = "Mussroom noodle"
            };

            // 2. Act
            DateTime beforeCreateTime = DateTime.UtcNow;

            using (var context = new EasyRecipeDbContext(dbOptions))
            {
                context.Recipes.Add(newRecipe);

                await context.SaveChangesAsync();
            }

            // 3. Assert
            using (var context = new EasyRecipeDbContext(dbOptions))
            {
                var savedRecipe = await context.Recipes.FirstAsync();

                Assert.NotEqual(DateTime.MinValue, savedRecipe.CreatedAt);

                Assert.True(savedRecipe.CreatedAt >= beforeCreateTime);

                Assert.Null(savedRecipe.LastModifiedAt);
            }

            // 4. Act
            DateTime beforeUpdateTime = DateTime.UtcNow;

            await Task.Delay(10);

            using (var context = new EasyRecipeDbContext(dbOptions))
            {
                var recipeToUpdate = await context.Recipes.FirstAsync();
                recipeToUpdate.RecipeName = "Vegetable noodle";

                await context.SaveChangesAsync();
            }

            using (var context = new EasyRecipeDbContext(dbOptions))
            {
                var updatedRecipe = await context.Recipes.FirstAsync();

                Assert.NotNull(updatedRecipe.LastModifiedAt);

                Assert.True(updatedRecipe.LastModifiedAt >= beforeUpdateTime);

                Assert.True(updatedRecipe.CreatedAt < beforeUpdateTime);
            }
        }
    }
}
