namespace ExamenApi.Models.Dtos
{
    public class BudgetCategoryCreateDto
    {
        public required string Name { get; set; }
        public decimal Limit { get; set; }
        public int MonthlyBudgetId { get; set; }
    }
}
