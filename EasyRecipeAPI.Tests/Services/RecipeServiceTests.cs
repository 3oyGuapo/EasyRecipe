using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using NSubstitute;
using RecipeData;
using EasyRecipeAPI.Services;
using EasyRecipeAPI.Repositories;
using RecipeData.Dtos;
using RecipeData.Entities;

namespace EasyRecipeAPI.Tests.Services
{
    public class RecipeServiceTests
    {

        [Fact]
        public async Task CreateRecipeAsync_TagAlreadyExists_ShouldReuseExistingTag()
        {
            // 1. Arrange
            var mockRepository = Substitute.For<IRecipeRepository>();

            var service = new RecipeService(mockRepository);

            var inputDto = new CreateRecipeDto
            {
                RecipeName = "Beef noodles",
                Ingredients = new List<CreateIngredientDto>
                {
                    new CreateIngredientDto { Name = "Beef", UnitAmount = "100g" }
                },
                Steps = new List<CreateStepDto>
                {
                    new CreateStepDto { StepContent = "Cook the beef", StepOrder = 1 }
                },
                Tags = new List<string> { "Noodle" }
            };

            var existingTag = new Tag { Id = 99, Name = "Noodle" };

            mockRepository.GetTagByNameAsync("Noodle").Returns(existingTag);


            // 2. Act
            RecipeResponseDto resultRecipe = await service.CreateRecipeAsync(inputDto);


            // 3. Assert
            Assert.NotNull(resultRecipe);
            Assert.Equal("Beef noodles", resultRecipe.RecipeName);

            Assert.Single(resultRecipe.Tags);

            await mockRepository.Received(1).AddRecipeAsync(Arg.Is<Recipe>(recipe =>
            recipe.RecipeName == "Beef noodles" &&
            recipe.Tags.Count == 1 &&
            recipe.Tags[0].Id == 99
            ));

            await mockRepository.Received(1).SaveChangesAsync();
        }

        [Fact]
        public async Task DeleteRecipeAsync_IdDoesNotExist_ShouldThrowKeyNotFoundException()
        {
            // 1. Arrange
            var mockRepository = Substitute.For<IRecipeRepository>();
            var service = new RecipeService(mockRepository);

            int notExistingId = 999;

            mockRepository.GetRecipeByIdAsync(notExistingId).Returns(Task.FromResult<Recipe?>(null));

            // 2. Act & 3. Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                await service.DeleteRecipeByIdAsync(notExistingId);
            });

            Assert.Equal($"Key with ID {notExistingId} not found.", exception.Message);

            await mockRepository.DidNotReceive().DeleteRecipeAsync(Arg.Any<Recipe>());
            await mockRepository.DidNotReceive().SaveChangesAsync();
        }
    }
}
