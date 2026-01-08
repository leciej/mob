namespace SolutionOrdersReact.Server.Dto;

public class UserStatsDto
{
    public int PurchasedCount { get; set; }
    public decimal TotalSpent { get; set; }
    public int RatedCount { get; set; }
    public double AverageRating { get; set; }
    public int CommentsCount { get; set; }
}
