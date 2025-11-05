using FluentAssertions;
using InsuranceApp.Controllers;
using InsuranceApp.Models;
using InsuranceApp.Models.Enums;
using InsuranceApp.Models.ViewModels;
using InsuranceApp.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;

namespace InsuranceApp.Tests.Controllers;

public class PartnersControllerTests
{
    private readonly Mock<IPartnerService> _partnerServiceMock;
    private readonly Mock<IPolicyService> _policyServiceMock;
    private readonly PartnersController _controller;
    private readonly ITempDataDictionary _tempData;

    public PartnersControllerTests()
    {
        _partnerServiceMock = new Mock<IPartnerService>();
        _policyServiceMock = new Mock<IPolicyService>();
        _controller = new PartnersController(_partnerServiceMock.Object, _policyServiceMock.Object);
        
        // Setup TempData
        var httpContext = new DefaultHttpContext();
        var tempDataProvider = new Mock<ITempDataProvider>();
        _tempData = new TempDataDictionary(httpContext, tempDataProvider.Object);
        _controller.TempData = _tempData;
    }

    [Fact]
    public async Task Index_ShouldReturnViewWithPartners()
    {
        // Arrange
        var partners = new List<PartnerListViewModel>
        {
            new() { Id = 1, FullName = "John Doe" },
            new() { Id = 2, FullName = "Jane Smith" }
        };
        _partnerServiceMock.Setup(x => x.GetAllPartnersAsync())
            .ReturnsAsync(partners);

        // Act
        var result = await _controller.Index();

        // Assert
        var viewResult = result.Should().BeOfType<ViewResult>().Subject;
        var model = viewResult.Model.Should().BeAssignableTo<IEnumerable<PartnerListViewModel>>().Subject;
        model.Should().HaveCount(2);
    }

    [Fact]
    public async Task Details_WhenPartnerExists_ShouldReturnViewWithPartner()
    {
        // Arrange
        var partnerId = 1;
        var partner = new Partner
        {
            Id = partnerId,
            FirstName = "John",
            LastName = "Doe",
            PartnerNumber = "12345678901234567890",
            ExternalCode = "EXT-1234567890",
            CreateByUser = "test@example.com",
            Gender = Gender.M,
            PartnerTypeId = PartnerType.Personal
        };
        _partnerServiceMock.Setup(x => x.GetPartnerWithPoliciesAsync(partnerId))
            .ReturnsAsync(partner);

        // Act
        var result = await _controller.Details(partnerId);

        // Assert
        var viewResult = result.Should().BeOfType<ViewResult>().Subject;
        var model = viewResult.Model.Should().BeOfType<Partner>().Subject;
        model.Id.Should().Be(partnerId);
    }

    [Fact]
    public async Task Details_WhenPartnerDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        var partnerId = 999;
        _partnerServiceMock.Setup(x => x.GetPartnerWithPoliciesAsync(partnerId))
            .ReturnsAsync((Partner?)null);

        // Act
        var result = await _controller.Details(partnerId);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public void Create_Get_ShouldReturnViewWithNewPartner()
    {
        // Act
        var result = _controller.Create();

        // Assert
        var viewResult = result.Should().BeOfType<ViewResult>().Subject;
        var model = viewResult.Model.Should().BeOfType<Partner>().Subject;
        model.Id.Should().Be(0);
    }

    [Fact]
    public async Task Create_Post_WithValidModel_ShouldRedirectToIndex()
    {
        // Arrange
        var partner = new Partner
        {
            FirstName = "John",
            LastName = "Doe",
            PartnerNumber = "12345678901234567890",
            ExternalCode = "EXT-1234567890",
            CreateByUser = "test@example.com",
            Gender = Gender.M,
            PartnerTypeId = PartnerType.Personal
        };
        _partnerServiceMock.Setup(x => x.PartnerExistsAsync(partner.ExternalCode))
            .ReturnsAsync(false);
        _partnerServiceMock.Setup(x => x.CreatePartnerAsync(partner))
            .ReturnsAsync(1);

        // Act
        var result = await _controller.Create(partner);

        // Assert
        var redirectResult = result.Should().BeOfType<RedirectToActionResult>().Subject;
        redirectResult.ActionName.Should().Be("Index");
        _controller.TempData["NewPartnerId"].Should().Be(1);
    }

    [Fact]
    public async Task Create_Post_WithExistingExternalCode_ShouldReturnViewWithError()
    {
        // Arrange
        var partner = new Partner
        {
            FirstName = "John",
            LastName = "Doe",
            PartnerNumber = "12345678901234567890",
            ExternalCode = "EXT-1234567890",
            CreateByUser = "test@example.com",
            Gender = Gender.M,
            PartnerTypeId = PartnerType.Personal
        };
        _partnerServiceMock.Setup(x => x.PartnerExistsAsync(partner.ExternalCode))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.Create(partner);

        // Assert
        var viewResult = result.Should().BeOfType<ViewResult>().Subject;
        viewResult.Model.Should().Be(partner);
        _controller.ModelState.Should().ContainKey("ExternalCode");
    }

    [Fact]
    public async Task Create_Post_WithInvalidModel_ShouldReturnView()
    {
        // Arrange
        var partner = new Partner();
        _controller.ModelState.AddModelError("FirstName", "Required");

        // Act
        var result = await _controller.Create(partner);

        // Assert
        var viewResult = result.Should().BeOfType<ViewResult>().Subject;
        viewResult.Model.Should().Be(partner);
    }

    [Fact]
    public async Task AddPolicy_Get_WhenPartnerExists_ShouldReturnViewWithViewModel()
    {
        // Arrange
        var partnerId = 1;
        var partner = new Partner
        {
            Id = partnerId,
            FirstName = "John",
            LastName = "Doe"
        };
        _partnerServiceMock.Setup(x => x.GetPartnerWithPoliciesAsync(partnerId))
            .ReturnsAsync(partner);

        // Act
        var result = await _controller.AddPolicy(partnerId);

        // Assert
        var viewResult = result.Should().BeOfType<ViewResult>().Subject;
        var model = viewResult.Model.Should().BeOfType<PolicyViewModel>().Subject;
        model.PartnerId.Should().Be(partnerId);
        model.PartnerName.Should().Be("John Doe");
    }

    [Fact]
    public async Task AddPolicy_Get_WhenPartnerIdIsNull_ShouldRedirectToIndex()
    {
        // Act
        var result = await _controller.AddPolicy((int?)null);

        // Assert
        var redirectResult = result.Should().BeOfType<RedirectToActionResult>().Subject;
        redirectResult.ActionName.Should().Be("Index");
    }

    [Fact]
    public async Task AddPolicy_Get_WhenPartnerDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        var partnerId = 999;
        _partnerServiceMock.Setup(x => x.GetPartnerWithPoliciesAsync(partnerId))
            .ReturnsAsync((Partner?)null);

        // Act
        var result = await _controller.AddPolicy(partnerId);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task AddPolicy_Post_WithValidModel_ShouldRedirectToIndex()
    {
        // Arrange
        var viewModel = new PolicyViewModel
        {
            PartnerId = 1,
            PolicyNumber = "POL-1234567890",
            Amount = 1500.50m,
            PartnerName = "John Doe"
        };
        _policyServiceMock.Setup(x => x.PolicyExistsAsync(viewModel.PolicyNumber))
            .ReturnsAsync(false);
        _policyServiceMock.Setup(x => x.CreatePolicyAsync(viewModel))
            .ReturnsAsync(1);

        // Act
        var result = await _controller.AddPolicy(viewModel);

        // Assert
        var redirectResult = result.Should().BeOfType<RedirectToActionResult>().Subject;
        redirectResult.ActionName.Should().Be("Index");
        _controller.TempData["SuccessMessage"].Should().Be("Policy added successfully!");
    }

    [Fact]
    public async Task AddPolicy_Post_WithExistingPolicyNumber_ShouldReturnViewWithError()
    {
        // Arrange
        var viewModel = new PolicyViewModel
        {
            PartnerId = 1,
            PolicyNumber = "POL-1234567890",
            Amount = 1500.50m,
            PartnerName = "John Doe"
        };
        var partner = new Partner
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe"
        };
        _policyServiceMock.Setup(x => x.PolicyExistsAsync(viewModel.PolicyNumber))
            .ReturnsAsync(true);
        _partnerServiceMock.Setup(x => x.GetPartnerWithPoliciesAsync(viewModel.PartnerId))
            .ReturnsAsync(partner);

        // Act
        var result = await _controller.AddPolicy(viewModel);

        // Assert
        var viewResult = result.Should().BeOfType<ViewResult>().Subject;
        viewResult.Model.Should().Be(viewModel);
        _controller.ModelState.Should().ContainKey("PolicyNumber");
    }

    [Fact]
    public async Task AddPolicy_Post_WithInvalidModel_ShouldReturnView()
    {
        // Arrange
        var viewModel = new PolicyViewModel
        {
            PartnerId = 1,
            PartnerName = "John Doe"
        };
        var partner = new Partner
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe"
        };
        _controller.ModelState.AddModelError("PolicyNumber", "Required");
        _partnerServiceMock.Setup(x => x.GetPartnerWithPoliciesAsync(viewModel.PartnerId))
            .ReturnsAsync(partner);

        // Act
        var result = await _controller.AddPolicy(viewModel) as ViewResult;

        // Assert
        result.Should().NotBeNull();
        result!.Model.Should().Be(viewModel);
    }
}
