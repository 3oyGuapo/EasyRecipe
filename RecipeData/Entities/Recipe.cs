using RecipeData.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RecipeData.Entities
{
    public class Recipe : IAuditable, ISoftDeletable
    {
        [Key]
        public int ID { get; set; }
        public string RecipeName { get; set; }
        public List<Ingredient> IngredientsList { get; set; }
        public List<Step> StepsList { get; set; }
        public List<Tag> TagsList { get; set; }

        // IAuditable implementation
        public DateTime CreatedAt { get; set; }
        public DateTime? LastModifiedAt { get; set; }

        // ISoftDeletable implementation
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
