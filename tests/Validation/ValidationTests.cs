using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using InsuranceApp.Models;
using InsuranceApp.Models.Enums;
using InsuranceApp.Models.ViewModels;

namespace InsuranceApp.Tests.Validation;

public class ValidationTests
{
    private static IList<ValidationResult> ValidateModel(object model)
    {
        var validationResults = new List<ValidationResult>();
        var ctx = new ValidationContext(model, null, null);
        Validator.TryValidateObject(model, ctx, validationResults, true);
        return validationResults;
    }

    #region Partner Validation Tests

    [Fact]
    public void Partner_WithValidData_ShouldPassValidation()
    {
        // Arrange
        var partner = new Partner
        {
            FirstName = "John",
            LastName = "Doe",
            PartnerNumber = "12345678901234567890",
            ExternalCode = "EXT-1234567890",
            CreateByUser = "test@example.com",
            Gender = "M",
            PartnerTypeId = PartnerType.Personal
        };

        // Act
        var results = ValidateModel(partner);

        // Assert
        results.Should().BeEmpty();
    }

    [Theory]
    [InlineData("")]
    [InlineData("A")]
    [InlineData(null)]
    public void Partner_WithInvalidFirstName_ShouldFailValidation(string? firstName)
    {
        // Arrange
        var partner = new Partner
        {
            FirstName = firstName!,
            LastName = "Doe",
            PartnerNumber = "12345678901234567890",
            ExternalCode = "EXT-1234567890",
            CreateByUser = "test@example.com",
            Gender = "M",
            PartnerTypeId = PartnerType.Personal
        };

        // Act
        var results = ValidateModel(partner);

        // Assert
        results.Should().NotBeEmpty();
        results.Should().Contain(r => r.MemberNames.Contains("FirstName"));
    }

    [Theory]
    [InlineData("123456789")]          // Too short (9 digits)
    [InlineData("123456789012345678901")] // Too long (21 digits)
    [InlineData("1234567890ABCDE12345")] // Contains letters
    public void Partner_WithInvalidPartnerNumber_ShouldFailValidation(string partnerNumber)
    {
        // Arrange
        var partner = new Partner
        {
            FirstName = "John",
            LastName = "Doe",
            PartnerNumber = partnerNumber,
            ExternalCode = "EXT-1234567890",
            CreateByUser = "test@example.com",
            Gender = "M",
            PartnerTypeId = PartnerType.Personal
        };

        // Act
        var results = ValidateModel(partner);

        // Assert
        results.Should().NotBeEmpty();
        results.Should().Contain(r => r.MemberNames.Contains("PartnerNumber"));
    }

    [Theory]
    [InlineData("123456789")]    // Too short (9 digits)
    [InlineData("123456789012")]  // Too long (12 digits)
    [InlineData("1234567890A")]   // Contains letter
    public void Partner_WithInvalidCroatianPIN_ShouldFailValidation(string croatianPIN)
    {
        // Arrange
        var partner = new Partner
        {
            FirstName = "John",
            LastName = "Doe",
            PartnerNumber = "12345678901234567890",
            CroatianPIN = croatianPIN,
            ExternalCode = "EXT-1234567890",
            CreateByUser = "test@example.com",
            Gender = "M",
            PartnerTypeId = PartnerType.Personal
        };

        // Act
        var results = ValidateModel(partner);

        // Assert
        results.Should().NotBeEmpty();
        results.Should().Contain(r => r.MemberNames.Contains("CroatianPIN"));
    }

    [Theory]
    [InlineData("SHORT")]          // Too short (5 characters)
    [InlineData("TOOLONGEXTERNALCODE123")] // Too long (24 characters)
    public void Partner_WithInvalidExternalCode_ShouldFailValidation(string externalCode)
    {
        // Arrange
        var partner = new Partner
        {
            FirstName = "John",
            LastName = "Doe",
            PartnerNumber = "12345678901234567890",
            ExternalCode = externalCode,
            CreateByUser = "test@example.com",
            Gender = "M",
            PartnerTypeId = PartnerType.Personal
        };

        // Act
        var results = ValidateModel(partner);

        // Assert
        results.Should().NotBeEmpty();
        results.Should().Contain(r => r.MemberNames.Contains("ExternalCode"));
    }

    [Theory]
    [InlineData("invalid-email")]
    [InlineData("@example.com")]
    [InlineData("user@")]
    [InlineData("")]
    public void Partner_WithInvalidEmail_ShouldFailValidation(string email)
    {
        // Arrange
        var partner = new Partner
        {
            FirstName = "John",
            LastName = "Doe",
            PartnerNumber = "12345678901234567890",
            ExternalCode = "EXT-1234567890",
            CreateByUser = email,
            Gender = "M",
            PartnerTypeId = PartnerType.Personal
        };

        // Act
        var results = ValidateModel(partner);

        // Assert
        results.Should().NotBeEmpty();
        results.Should().Contain(r => r.MemberNames.Contains("CreateByUser"));
    }

    #endregion

    #region Policy Validation Tests

    [Fact]
    public void Policy_WithValidData_ShouldPassValidation()
    {
        // Arrange
        var policy = new Policy
        {
            PartnerId = 1,
            PolicyNumber = "POL-1234567890",
            Amount = 1500.50m
        };

        // Act
        var results = ValidateModel(policy);

        // Assert
        results.Should().BeEmpty();
    }

    [Theory]
    [InlineData("POL-12345")]      // Too short (9 characters)
    [InlineData("POL-1234567890123456")] // Too long (21 characters)
    public void Policy_WithInvalidPolicyNumber_ShouldFailValidation(string policyNumber)
    {
        // Arrange
        var policy = new Policy
        {
            PartnerId = 1,
            PolicyNumber = policyNumber,
            Amount = 1500.50m
        };

        // Act
        var results = ValidateModel(policy);

        // Assert
        results.Should().NotBeEmpty();
        results.Should().Contain(r => r.MemberNames.Contains("PolicyNumber"));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-100)]
    [InlineData(-0.01)]
    public void Policy_WithInvalidAmount_ShouldFailValidation(decimal amount)
    {
        // Arrange
        var policy = new Policy
        {
            PartnerId = 1,
            PolicyNumber = "POL-1234567890",
            Amount = amount
        };

        // Act
        var results = ValidateModel(policy);

        // Assert
        results.Should().NotBeEmpty();
        results.Should().Contain(r => r.MemberNames.Contains("Amount"));
    }

    #endregion

    #region PolicyViewModel Validation Tests

    [Fact]
    public void PolicyViewModel_WithValidData_ShouldPassValidation()
    {
        // Arrange
        var viewModel = new PolicyViewModel
        {
            PartnerId = 1,
            PolicyNumber = "POL-1234567890",
            Amount = 1500.50m,
            PartnerName = "John Doe"
        };

        // Act
        var results = ValidateModel(viewModel);

        // Assert
        results.Should().BeEmpty();
    }

    [Fact]
    public void PolicyViewModel_WithMissingPolicyNumber_ShouldFailValidation()
    {
        // Arrange
        var viewModel = new PolicyViewModel
        {
            PartnerId = 1,
            PolicyNumber = "",
            Amount = 1500.50m,
            PartnerName = "John Doe"
        };

        // Act
        var results = ValidateModel(viewModel);

        // Assert
        results.Should().NotBeEmpty();
        results.Should().Contain(r => r.MemberNames.Contains("PolicyNumber"));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void PolicyViewModel_WithInvalidAmount_ShouldFailValidation(decimal amount)
    {
        // Arrange
        var viewModel = new PolicyViewModel
        {
            PartnerId = 1,
            PolicyNumber = "POL-1234567890",
            Amount = amount,
            PartnerName = "John Doe"
        };

        // Act
        var results = ValidateModel(viewModel);

        // Assert
        results.Should().NotBeEmpty();
        results.Should().Contain(r => r.MemberNames.Contains("Amount"));
    }

    #endregion
}

