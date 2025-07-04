using System;

namespace ExamenApi.Models.Dtos
{
    public class ExpenseCreateDto
    {
        public decimal Amount { get; set; }
        public int BudgetCategoryId { get; set; }
        public DateTime Date { get; set; }
    }
}
