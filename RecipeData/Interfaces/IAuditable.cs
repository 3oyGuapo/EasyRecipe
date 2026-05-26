using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeData.Interfaces
{
    public interface IAuditable
    {
        DateTime CreatedAt { get; set; }
        DateTime? LastModifiedAt { get; set; }
    }
}
