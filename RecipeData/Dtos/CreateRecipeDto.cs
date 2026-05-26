using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RecipeData.Dtos
{
    public class CreateRecipeDto
    {
        [Required(ErrorMessage = "Recipe name cannot be null")]
        [MaxLength(50, ErrorMessage = "Name is too long")]
        public string Name { get; set; } = string.Empty;

        [MinLength(1, ErrorMessage = "At least one ingredient is required")]
        public List<CreateIngredientDto> IngredientsList { get; set; } = [];

        [MinLength(1, ErrorMessage = "At least one step is required")]
        public List<CreateStepDto> StepsList { get; set; } = [];

        public List<string> TagsList { get; set; } = [];
    }
}
