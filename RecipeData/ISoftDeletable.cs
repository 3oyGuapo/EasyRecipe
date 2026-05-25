using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeData
{
    public interface ISoftDeletable
    {
        bool IsDeleted { get; set; }
        DateTime? DeletedAt { get; set; }
    }
}
