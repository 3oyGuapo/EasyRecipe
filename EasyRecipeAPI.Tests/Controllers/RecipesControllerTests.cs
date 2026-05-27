using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Microsoft.EntityFrameworkCore;
using EasyRecipeAPI.DbContextData;
using EasyRecipeAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using EasyRecipeAPI.Services;
using RecipeData.Entities;
using RecipeData.Dtos;

namespace EasyRecipeAPI.Tests.Controllers
{
    public class RecipesControllerTests
    {
        [Fact]
        public async Task GetRecipes_RecipeExists_ReturnsOkWithRecipes()
        {
            // 1. Arrange
            var mockService = Substitute.For<IRecipeService>();

            int recipeId = 5;
            var testRecipe = new RecipeResponseDto
            {
                Id = recipeId,
                RecipeName = "Test Recipe",
                Ingredients = new List<IngredientResponseDto>(),
                Steps = new List<StepResponseDto>(),
                Tags = new List<String>()
            };

            mockService.GetRecipeByIdAsync(recipeId).Returns(testRecipe);

            var controller = new RecipesController(mockService);


            // 2. Act
            var actionResult = await controller.GetRecipe(recipeId);


            // 3. Assert
            Assert.NotNull(actionResult);

            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);

            var returnedRecipe = Assert.IsType<RecipeResponseDto>(okResult.Value);

            Assert.Equal(recipeId, returnedRecipe.Id);
            Assert.Equal("Test Recipe", returnedRecipe.RecipeName);
        }


        [Fact]
        public async Task GetRecipe_RecipeNotFound_ReturnsNotFound()
        {
            // 1. Arrange
            var mockService = Substitute.For<IRecipeService>();
            var notExistingId = 999;

            mockService.GetRecipeByIdAsync(notExistingId).Returns((RecipeResponseDto?)null);

            var controller = new RecipesController(mockService);

            // 2. Act
            var actionResult = await controller.GetRecipe(notExistingId);

            // 3. Assert
            Assert.IsType<NotFoundResult>(actionResult.Result);
        }


        [Fact]
        public async Task DeleteRecipe_IdNotFound_ReturnsNotFound()
        {
            // 1. Arrange
            var mockService = Substitute.For<IRecipeService>();
            int notExistingId = 999;

            mockService.DeleteRecipeByIdAsync(notExistingId).Returns(Task.FromException<KeyNotFoundException>(new KeyNotFoundException($"Recipe with ID {notExistingId} not found.")));

            var controller = new RecipesController(mockService);

            // 2. Act && 3. Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                await controller.DeleteRecipe(notExistingId);
            });
        }

        [Fact]
        public async Task DeleteRecipe_IdExists_ReturnsOk()
        {
            // 1. Arrange
            var mockService = Substitute.For<IRecipeService>();
            int recipeId = 3;

            mockService.DeleteRecipeByIdAsync(recipeId).Returns(Task.CompletedTask);

            var controller = new RecipesController(mockService);

            //2, Act
            IActionResult actionResult = await controller.DeleteRecipe(recipeId);

            // 3. Assert
            Assert.IsType<OkResult>(actionResult);
        }
    }
}
