using InsuranceApp.Data.Repositories.Interfaces;
using InsuranceApp.Models;
using InsuranceApp.Models.ViewModels;
using InsuranceApp.Services.Interfaces;

namespace InsuranceApp.Services;

public class PartnerService : IPartnerService
{
    private readonly IPartnerRepository _partnerRepository;

    public PartnerService(IPartnerRepository partnerRepository)
    {
        _partnerRepository = partnerRepository;
    }

    public async Task<IEnumerable<PartnerListViewModel>> GetAllPartnersAsync()
    {
        return await _partnerRepository.GetAllAsync();
    }

    public async Task<Partner?> GetPartnerByIdAsync(int id)
    {
        return await _partnerRepository.GetByIdAsync(id);
    }

    public async Task<Partner?> GetPartnerWithPoliciesAsync(int id)
    {
        return await _partnerRepository.GetWithPoliciesAsync(id);
    }

    public async Task<int> CreatePartnerAsync(Partner partner)
    {
        // Business logic: Set creation timestamp
        partner.CreatedAtUtc = DateTime.UtcNow;
        
        return await _partnerRepository.CreateAsync(partner);
    }

    public async Task<bool> PartnerExistsAsync(string externalCode)
    {
        return await _partnerRepository.ExistsAsync(externalCode);
    }
}
