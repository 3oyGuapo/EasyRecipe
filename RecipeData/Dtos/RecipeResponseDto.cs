using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeData.Dtos
{
    public record class RecipeResponseDto
    {
        public int Id { get; init; }
        public string RecipeName { get; init; } = string.Empty;

        public List<IngredientResponseDto> Ingredients { get; init; } = [];
        public List<StepResponseDto> Steps { get; init; } = [];

        public List<string> Tags { get; init; } = [];
        public DateTime CreatedAt { get; init; }
    }
}
