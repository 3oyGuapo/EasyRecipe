using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeData.Entities
{
    public class Step
    {
        public int ID { get; set; }
        public string StepContent { get; set; }
        public int StepOrder { get; set; }
        public int RecipeID { get; set; }
        public Recipe Recipe { get; set; }
    }
}
