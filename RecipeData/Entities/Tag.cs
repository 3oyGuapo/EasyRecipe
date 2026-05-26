using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeData.Entities
{
    public class Tag
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public List<Recipe> RecipesList { get; set; }
    }
}
