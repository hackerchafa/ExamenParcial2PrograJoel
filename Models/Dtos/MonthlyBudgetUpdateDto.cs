using System;
using System.Collections.Generic;

namespace ExamenApi.Models.Dtos
{
    public class MonthlyBudgetUpdateDto
    {
        public int Id { get; set; }
        public DateTime Month { get; set; }
        public required List<BudgetCategoryUpdateDto> Categories { get; set; }
    }
}
