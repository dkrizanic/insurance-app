using InsuranceApp.Models.Enums;

namespace InsuranceApp.Models.ViewModels;

public class PartnerListViewModel
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string PartnerNumber { get; set; } = string.Empty;
    public string? CroatianPIN { get; set; }
    public PartnerType PartnerTypeId { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public bool IsForeign { get; set; }
    public string Gender { get; set; } = string.Empty;
    public int PolicyCount { get; set; }
    public decimal TotalPolicyAmount { get; set; }
    public bool RequiresHighlight { get; set; }
}