using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RecipeData.Dtos
{
    public class CreateStepDto
    {
        [Required(ErrorMessage = "Step description is required.")]
        [MaxLength(100)]
        public string StepContent { get; set; } = string.Empty;

        [Range(1,999, ErrorMessage = "Step order must be between 1 to 999")]
        public int StepOrder { get; set; }
    }
}
