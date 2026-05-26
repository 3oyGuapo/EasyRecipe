using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeData
{
    public class RecipeQueryParameters
    {
        private const int MaxPageSize = 50;

        private int _pageSize = 10;

        public int PageNumber { get; set; }
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
        public string? SearchQuery { get; set; }
        public string? SortBy { get; set; }
        public string SortOrder { get; set; }
    }
}
