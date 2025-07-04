namespace ExamenApi.Models.Dtos
{
    public class BudgetCategoryUpdateDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public decimal Limit { get; set; }
        public int MonthlyBudgetId { get; set; }
    }
}
