using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeData
{
    public class CreateRecipeDto
    {
        public string Name { get; set; } = string.Empty;
        public List<CreateIngredientDto> IngredientsList { get; set; } = [];
        public List<CreateStepDto> StepsList { get; set; } = [];
    }
}
