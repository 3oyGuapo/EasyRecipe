using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeData
{
    public class CreateIngredientDto
    {
        public string Name { get; set; } = "Unknown";
        public string UnitAmount { get; set; } = string.Empty;
    }
}
