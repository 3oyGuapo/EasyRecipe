using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeData.Dtos
{
    public record class IngredientResponseDto
    {
        public string Name { get; init; } = string.Empty;
        public string UnitAmount { get; init; } = string.Empty;
    }
}
