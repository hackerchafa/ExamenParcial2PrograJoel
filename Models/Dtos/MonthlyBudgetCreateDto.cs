using System;
using System.Collections.Generic;

namespace ExamenApi.Models.Dtos
{
    public class MonthlyBudgetCreateDto
    {
        public DateTime Month { get; set; }
        public required List<BudgetCategoryCreateDto> Categories { get; set; }
    }
}
