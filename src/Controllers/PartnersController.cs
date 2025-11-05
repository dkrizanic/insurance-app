using InsuranceApp.Models;
using InsuranceApp.Models.ViewModels;
using InsuranceApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceApp.Controllers;

public class PartnersController : Controller
{
    private readonly IPartnerService _partnerService;
    private readonly IPolicyService _policyService;

    public PartnersController(IPartnerService partnerService, IPolicyService policyService)
    {
        _partnerService = partnerService;
        _policyService = policyService;
    }

    public async Task<IActionResult> Index()
    {
        var partners = await _partnerService.GetAllPartnersAsync();
        return View(partners);
    }

    [HttpPost]
    public async Task<IActionResult> GetDetails(int id)
    {
        var partner = await _partnerService.GetPartnerWithPoliciesAsync(id);
        if (partner == null)
        {
            return NotFound();
        }

        return PartialView("_PartnerDetails", partner);
    }

    public async Task<IActionResult> Details(int id)
    {
        var partner = await _partnerService.GetPartnerWithPoliciesAsync(id);
        if (partner == null)
        {
            return NotFound();
        }

        return View(partner);
    }

    public IActionResult Create()
    {
        return View(new Partner());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Partner partner)
    {
        if (!ModelState.IsValid)
        {
            return View(partner);
        }

        if (await _partnerService.PartnerExistsAsync(partner.ExternalCode))
        {
            ModelState.AddModelError("ExternalCode", "External code must be unique");
            return View(partner);
        }

        var id = await _partnerService.CreatePartnerAsync(partner);

        TempData["NewPartnerId"] = id;
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> AddPolicy(int? id)
    {
        if (id == null)
        {
            return RedirectToAction(nameof(Index));
        }

        var partner = await _partnerService.GetPartnerWithPoliciesAsync(id.Value);
        if (partner == null)
        {
            return NotFound();
        }

        var model = new PolicyViewModel
        {
            PartnerId = id.Value,
            PartnerName = partner.FullName
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddPolicy(PolicyViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var partner = await _partnerService.GetPartnerWithPoliciesAsync(model.PartnerId);
            model.PartnerName = partner?.FullName ?? string.Empty;
            return View(model);
        }

        if (await _policyService.PolicyExistsAsync(model.PolicyNumber))
        {
            ModelState.AddModelError("PolicyNumber", "Policy number must be unique");
            var partner = await _partnerService.GetPartnerWithPoliciesAsync(model.PartnerId);
            model.PartnerName = partner?.FullName ?? string.Empty;
            return View(model);
        }

        await _policyService.CreatePolicyAsync(model);

        TempData["SuccessMessage"] = "Policy added successfully!";
        return RedirectToAction(nameof(Index));
    }
}