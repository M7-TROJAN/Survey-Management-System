namespace SurveyBasket.Web.Entities;

public class AuditableEntity
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string CreatedById { get; set; } = string.Empty;
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedById { get; set; }

    public ApplicationUser CreatedBy { get; set; } = default!;
    public ApplicationUser? UpdatedBy { get; set; }
}
