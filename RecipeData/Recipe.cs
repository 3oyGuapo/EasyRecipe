using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RecipeData
{
    public class Recipe : IAuditable
    {
        [Key]
        public int ID { get; set; }
        public string RecipeName { get; set; }
        public List<Ingredient> IngredientsList { get; set; }
        public List<Step> StepsList { get; set; }
        public List<Tag> TagsList { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? LastModifiedAt { get; set; }
    }
}
