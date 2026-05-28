using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeData.Entities
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Recipe> Recipes { get; set; }
    }
}
