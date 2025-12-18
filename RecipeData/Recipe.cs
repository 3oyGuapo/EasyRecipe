using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeData
{
    public class Recipe
    {
        public int ID { get; set; }
        public string RecipeName { get; set; }
        public List<Ingredient> IngredientList { get; set; }
        public List<Step> StepList { get; set; }
        public List<Tag> TagsList { get; set; }
    }
}
