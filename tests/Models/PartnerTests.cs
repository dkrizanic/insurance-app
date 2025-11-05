using FluentAssertions;
using InsuranceApp.Models;
using InsuranceApp.Models.Enums;

namespace InsuranceApp.Tests.Models;

public class PartnerTests
{
    [Fact]
    public void FullName_ShouldConcatenateFirstAndLastName()
    {
        // Arrange
        var partner = new Partner
        {
            FirstName = "John",
            LastName = "Doe"
        };

        // Act
        var fullName = partner.FullName;

        // Assert
        fullName.Should().Be("John Doe");
    }

    [Fact]
    public void TotalPolicyAmount_WithNoPolicies_ShouldReturnZero()
    {
        // Arrange
        var partner = new Partner
        {
            Policies = new List<Policy>()
        };

        // Act
        var total = partner.TotalPolicyAmount;

        // Assert
        total.Should().Be(0);
    }

    [Fact]
    public void TotalPolicyAmount_WithMultiplePolicies_ShouldReturnSum()
    {
        // Arrange
        var partner = new Partner
        {
            Policies = new List<Policy>
            {
                new() { Amount = 1000m },
                new() { Amount = 2500.50m },
                new() { Amount = 1500.25m }
            }
        };

        // Act
        var total = partner.TotalPolicyAmount;

        // Assert
        total.Should().Be(5000.75m);
    }

    [Fact]
    public void PolicyCount_WithNoPolicies_ShouldReturnZero()
    {
        // Arrange
        var partner = new Partner
        {
            Policies = new List<Policy>()
        };

        // Act
        var count = partner.PolicyCount;

        // Assert
        count.Should().Be(0);
    }

    [Fact]
    public void PolicyCount_WithMultiplePolicies_ShouldReturnCorrectCount()
    {
        // Arrange
        var partner = new Partner
        {
            Policies = new List<Policy>
            {
                new() { PolicyNumber = "POL-1" },
                new() { PolicyNumber = "POL-2" },
                new() { PolicyNumber = "POL-3" }
            }
        };

        // Act
        var count = partner.PolicyCount;

        // Assert
        count.Should().Be(3);
    }

    [Theory]
    [InlineData(6, 0, true)]      // More than 5 policies
    [InlineData(10, 0, true)]     // More than 5 policies
    [InlineData(5, 5001, true)]   // Exactly 5 policies, amount > 5000
    [InlineData(3, 6000, true)]   // Less than 5 policies, amount > 5000
    [InlineData(5, 5000, false)]  // Exactly 5 policies, amount = 5000
    [InlineData(3, 4999, false)]  // Less than 5 policies, amount < 5000
    [InlineData(0, 0, false)]     // No policies, no amount
    public void RequiresHighlight_ShouldReturnCorrectValue(int policyCount, decimal totalAmount, bool expectedHighlight)
    {
        // Arrange
        var policies = Enumerable.Range(1, policyCount)
            .Select(i => new Policy
            {
                PolicyNumber = $"POL-{i}",
                Amount = policyCount > 0 ? totalAmount / policyCount : 0
            })
            .ToList();

        var partner = new Partner
        {
            Policies = policies
        };

        // Act
        var requiresHighlight = partner.RequiresHighlight;

        // Assert
        requiresHighlight.Should().Be(expectedHighlight);
    }

    [Fact]
    public void Partner_WithAllProperties_ShouldBeValid()
    {
        // Arrange & Act
        var partner = new Partner
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            Address = "123 Main St",
            PartnerNumber = "12345678901234567890",
            CroatianPIN = "12345678901",
            PartnerTypeId = PartnerType.Personal,
            CreatedAtUtc = DateTime.UtcNow,
            CreateByUser = "test@example.com",
            IsForeign = false,
            ExternalCode = "EXT-1234567890",
            Gender = Gender.M
        };

        // Assert
        partner.FirstName.Should().Be("John");
        partner.LastName.Should().Be("Doe");
        partner.FullName.Should().Be("John Doe");
        partner.PartnerNumber.Should().HaveLength(20);
        partner.CroatianPIN.Should().HaveLength(11);
        partner.ExternalCode.Should().HaveLength(14);
        partner.Gender.Should().Be(Gender.M);
        partner.PartnerTypeId.Should().Be(PartnerType.Personal);
    }
}
