namespace ExamenApi.Models
{
    public class BudgetCategory
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public decimal Limit { get; set; }
        public int MonthlyBudgetId { get; set; }
        public required MonthlyBudget MonthlyBudget { get; set; }
        public required ICollection<Expense> Expenses { get; set; }
    }
}
