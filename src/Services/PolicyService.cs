using InsuranceApp.Data.Repositories.Interfaces;
using InsuranceApp.Models;
using InsuranceApp.Models.ViewModels;
using InsuranceApp.Services.Interfaces;

namespace InsuranceApp.Services;

public class PolicyService : IPolicyService
{
    private readonly IPolicyRepository _policyRepository;

    public PolicyService(IPolicyRepository policyRepository)
    {
        _policyRepository = policyRepository;
    }

    public async Task<int> CreatePolicyAsync(PolicyViewModel model)
    {
        // Business logic: Create policy from view model
        var policy = new Policy
        {
            PartnerId = model.PartnerId,
            PolicyNumber = model.PolicyNumber,
            Amount = model.Amount,
            CreatedAtUtc = DateTime.UtcNow
        };

        return await _policyRepository.CreateAsync(policy);
    }

    public async Task<bool> PolicyExistsAsync(string policyNumber)
    {
        return await _policyRepository.ExistsAsync(policyNumber);
    }
}
