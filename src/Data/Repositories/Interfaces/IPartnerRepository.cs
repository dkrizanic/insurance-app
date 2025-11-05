using InsuranceApp.Models;
using InsuranceApp.Models.ViewModels;

namespace InsuranceApp.Data.Repositories.Interfaces;

public interface IPartnerRepository
{
    Task<IEnumerable<PartnerListViewModel>> GetAllAsync();
    Task<Partner?> GetByIdAsync(int id);
    Task<int> CreateAsync(Partner partner);
    Task<bool> ExistsAsync(string externalCode);
    Task<Partner?> GetWithPoliciesAsync(int id);
}
