namespace ExamenApi.Models
{
    public class BudgetCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Limit { get; set; }
        public int MonthlyBudgetId { get; set; }
        public MonthlyBudget MonthlyBudget { get; set; }
        public ICollection<Expense> Expenses { get; set; }
    }
}
