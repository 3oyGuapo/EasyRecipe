using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RecipeData.Dtos
{
    public class UpdateRecipeDto
    {
        [Required(ErrorMessage = "Recipe name cannot be null")]
        [MaxLength(50, ErrorMessage = "Name is too long")]
        public string RecipeName { get; set; } = string.Empty;

        [MinLength(1, ErrorMessage = "At least one ingredient is required")]
        public List<CreateIngredientDto> Ingredients { get; set; } = [];

        [MinLength(1, ErrorMessage = "At least one step is required")]
        public List<CreateStepDto> Steps { get; set; } = [];

        public List<string> Tags { get; set; } = [];

    }
}
