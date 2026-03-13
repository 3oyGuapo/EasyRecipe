using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RecipeData
{
    public class CreateIngredientDto
    {
        [Required(ErrorMessage = "Ingredient name is required.")]
        [MaxLength(30)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Amount is required")]
        [MaxLength(10)]
        public string UnitAmount { get; set; } = string.Empty;
    }
}
