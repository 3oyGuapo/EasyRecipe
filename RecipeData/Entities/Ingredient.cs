using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeData.Entities
{
    public class Ingredient
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string UnitAmount { get; set; }
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }
    }
}
