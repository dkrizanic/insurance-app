using InsuranceApp.Models;
using InsuranceApp.Models.ViewModels;

namespace InsuranceApp.Services.Interfaces;

public interface IPolicyService
{
    Task<int> CreatePolicyAsync(PolicyViewModel model);
    Task<bool> PolicyExistsAsync(string policyNumber);
}
