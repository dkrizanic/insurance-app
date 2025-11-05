using System.ComponentModel.DataAnnotations;
using InsuranceApp.Models.Enums;

namespace InsuranceApp.Models;

public class Partner
{
    public int Id { get; set; }

    [Required(ErrorMessage = "First name is required")]
    [StringLength(255, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 255 characters")]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required")]
    [StringLength(255, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 255 characters")]
    [Display(Name = "Last Name")]
    public string LastName { get; set; } = string.Empty;

    [Display(Name = "Full Name")]
    public string FullName => $"{FirstName} {LastName}";

    public string? Address { get; set; }

    [Required(ErrorMessage = "Partner number is required")]
    [StringLength(20, MinimumLength = 20, ErrorMessage = "Partner number must be exactly 20 digits")]
    [RegularExpression(@"^\d{20}$", ErrorMessage = "Partner number must contain exactly 20 digits")]
    [Display(Name = "Partner Number")]
    public string PartnerNumber { get; set; } = string.Empty;

    [Display(Name = "Croatian PIN")]
    [StringLength(11, ErrorMessage = "Croatian PIN must be 11 characters")]
    [RegularExpression(@"^\d{11}$", ErrorMessage = "Croatian PIN must be 11 digits")]
    public string? CroatianPIN { get; set; }

    [Required(ErrorMessage = "Partner type is required")]
    [Display(Name = "Partner Type")]
    public PartnerType PartnerTypeId { get; set; }

    [Display(Name = "Created At")]
    public DateTime CreatedAtUtc { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    [StringLength(255, ErrorMessage = "Email must not exceed 255 characters")]
    [Display(Name = "Created By")]
    public string CreateByUser { get; set; } = string.Empty;

    [Required(ErrorMessage = "Is Foreign flag is required")]
    [Display(Name = "Is Foreign")]
    public bool IsForeign { get; set; }

    [Required(ErrorMessage = "External code is required")]
    [StringLength(20, MinimumLength = 10, ErrorMessage = "External code must be between 10 and 20 characters")]
    [Display(Name = "External Code")]
    public string ExternalCode { get; set; } = string.Empty;

    [Required(ErrorMessage = "Gender is required")]
    public Gender Gender { get; set; }

    public virtual ICollection<Policy> Policies { get; set; } = new List<Policy>();

    [Display(Name = "Total Policy Amount")]
    public decimal TotalPolicyAmount => Policies.Sum(p => p.Amount);

    [Display(Name = "Policy Count")]
    public int PolicyCount => Policies.Count;

    public bool RequiresHighlight => PolicyCount > 5 || TotalPolicyAmount > 5000;
}