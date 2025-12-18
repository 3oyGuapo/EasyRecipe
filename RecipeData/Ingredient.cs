using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeData
{
    public class Ingredient
    {
        public int ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string UnitAmount { get; set; }
        public int RecipeID { get; set; }
        public Recipe Recipe { get; set; }
    }
}
