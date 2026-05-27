using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeData.Dtos
{
    public class StepResponseDto
    {
        public string StepContent { get; set; } = string.Empty;
        public int StepOrder { get; set; }
    }
}
