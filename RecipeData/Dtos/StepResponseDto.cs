using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeData.Dtos
{
    public record class StepResponseDto
    {
        public string StepContent { get; init; } = string.Empty;
        public int StepOrder { get; init; }
    }
}
