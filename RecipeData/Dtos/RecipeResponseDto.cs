using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeData.Dtos
{
    public class RecipeResponseDto
    {
        public int Id { get; set; }
        public string RecipeName { get; set; } = string.Empty;

        public List<IngredientResponseDto> Ingredients { get; set; } = [];
        public List<StepResponseDto> Steps { get; set; } = [];

        public List<string> Tags { get; set; } = [];
        public DateTime CreatedAt { get; set; }
    }
}
