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

namespace EasyRecipeAPI.Tests
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
                Name = "Beef noodles",
                IngredientsList = new List<CreateIngredientDto>
                {
                    new CreateIngredientDto { Name = "Beef", UnitAmount = "100g" }
                },
                StepsList = new List<CreateStepDto>
                {
                    new CreateStepDto { StepContent = "Cook the beef", StepOrder = 1 }
                },
                TagsList = new List<string> { "Noodle" }
            };

            var existingTag = new Tag { ID = 99, Name = "Noodle" };

            mockRepository.GetTagByNameAsync("Noodle").Returns(existingTag);


            // 2. Act
            Recipe resultRecipe = await service.CreateRecipeAsync(inputDto);


            // 3. Assert
            Assert.NotNull(resultRecipe);
            Assert.Equal("Beef noodles", resultRecipe.RecipeName);

            Assert.Single(resultRecipe.TagsList);

            var linkedTag = resultRecipe.TagsList[0];
            Assert.Equal("Noodle", linkedTag.Name);

            Assert.Equal(99, linkedTag.ID);

            await mockRepository.Received(1).AddRecipeAsync(resultRecipe);

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
