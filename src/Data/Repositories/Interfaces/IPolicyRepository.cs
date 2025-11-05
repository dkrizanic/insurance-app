using InsuranceApp.Models;

namespace InsuranceApp.Data.Repositories.Interfaces;

public interface IPolicyRepository
{
    Task<IEnumerable<Policy>> GetByPartnerIdAsync(int partnerId);
    Task<int> CreateAsync(Policy policy);
    Task<bool> ExistsAsync(string policyNumber);
}
