using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeData
{
    public class CreateStepDto
    {
        public string StepContent { get; set; } = "No steps";
        public int StepOrder { get; set; }
    }
}
