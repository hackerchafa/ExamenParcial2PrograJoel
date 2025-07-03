namespace ExamenApi.Models
{
    public class Expense
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public int BudgetCategoryId { get; set; }
        public BudgetCategory BudgetCategory { get; set; }
        public DateTime Date { get; set; }
    }
}
