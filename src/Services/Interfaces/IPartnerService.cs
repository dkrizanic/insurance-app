using InsuranceApp.Models;
using InsuranceApp.Models.ViewModels;

namespace InsuranceApp.Services.Interfaces;

public interface IPartnerService
{
    Task<IEnumerable<PartnerListViewModel>> GetAllPartnersAsync();
    Task<Partner?> GetPartnerByIdAsync(int id);
    Task<Partner?> GetPartnerWithPoliciesAsync(int id);
    Task<int> CreatePartnerAsync(Partner partner);
    Task<bool> PartnerExistsAsync(string externalCode);
}
