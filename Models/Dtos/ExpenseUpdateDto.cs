using System;

namespace ExamenApi.Models.Dtos
{
    public class ExpenseUpdateDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public int BudgetCategoryId { get; set; }
        public DateTime Date { get; set; }
    }
}
