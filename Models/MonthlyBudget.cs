namespace ExamenApi.Models
{
    public class MonthlyBudget
    {
        public int Id { get; set; }
        public DateTime Month { get; set; }
        public ICollection<BudgetCategory> Categories { get; set; }
    }
}
