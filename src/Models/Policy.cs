using System.ComponentModel.DataAnnotations;

namespace InsuranceApp.Models;

public class Policy
{
    public int Id { get; set; }

    [Required]
    public int PartnerId { get; set; }

    [Required(ErrorMessage = "Policy number is required")]
    [StringLength(15, MinimumLength = 10, ErrorMessage = "Policy number must be between 10 and 15 characters")]
    [Display(Name = "Policy Number")]
    public string PolicyNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Amount is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
    public decimal Amount { get; set; }

    [Display(Name = "Created At")]
    public DateTime CreatedAtUtc { get; set; }

    public virtual Partner? Partner { get; set; }
}