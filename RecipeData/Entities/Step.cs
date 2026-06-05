using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeData.Entities
{
    public class Step
    {
        public int Id { get; set; }
        public string StepContent { get; set; } = string.Empty;
        public int StepOrder { get; set; }
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }
    }
}
