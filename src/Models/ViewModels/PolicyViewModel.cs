using System.ComponentModel.DataAnnotations;

namespace InsuranceApp.Models.ViewModels;

public class PolicyViewModel
{
    [Required(ErrorMessage = "Policy number is required")]
    [StringLength(15, MinimumLength = 10, ErrorMessage = "Policy number must be between 10 and 15 characters")]
    [Display(Name = "Policy Number")]
    public string PolicyNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Amount is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
    [Display(Name = "Amount")]
    public decimal Amount { get; set; }

    [Required]
    public int PartnerId { get; set; }

    [Display(Name = "Partner")]
    public string PartnerName { get; set; } = string.Empty;
}