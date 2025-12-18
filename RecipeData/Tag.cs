using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeData
{
    public class Tag
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public List<Recipe> RecipesList { get; set; }
    }
}
