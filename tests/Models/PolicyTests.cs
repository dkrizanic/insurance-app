using FluentAssertions;
using InsuranceApp.Models;

namespace InsuranceApp.Tests.Models;

public class PolicyTests
{
    [Fact]
    public void Policy_WithValidProperties_ShouldBeCreated()
    {
        // Arrange & Act
        var policy = new Policy
        {
            Id = 1,
            PartnerId = 1,
            PolicyNumber = "POL-1234567890",
            Amount = 1500.50m,
            CreatedAtUtc = DateTime.UtcNow
        };

        // Assert
        policy.Id.Should().Be(1);
        policy.PartnerId.Should().Be(1);
        policy.PolicyNumber.Should().Be("POL-1234567890");
        policy.Amount.Should().Be(1500.50m);
        policy.CreatedAtUtc.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Theory]
    [InlineData(0.01)]
    [InlineData(100)]
    [InlineData(5000)]
    [InlineData(999999.99)]
    public void Policy_WithVariousValidAmounts_ShouldAcceptAmount(decimal amount)
    {
        // Arrange & Act
        var policy = new Policy
        {
            Amount = amount
        };

        // Assert
        policy.Amount.Should().Be(amount);
    }

    [Theory]
    [InlineData("1234567890")]   // 10 characters (minimum)
    [InlineData("12345678901")]  // 11 characters
    [InlineData("123456789012")] // 12 characters
    [InlineData("1234567890123")] // 13 characters
    [InlineData("12345678901234")] // 14 characters
    [InlineData("123456789012345")] // 15 characters (maximum)
    public void Policy_WithValidPolicyNumbers_ShouldAcceptPolicyNumber(string policyNumber)
    {
        // Arrange & Act
        var policy = new Policy
        {
            PolicyNumber = policyNumber
        };

        // Assert
        policy.PolicyNumber.Should().Be(policyNumber);
        policy.PolicyNumber.Length.Should().BeInRange(10, 15);
    }

    [Fact]
    public void Policy_DefaultValues_ShouldBeInitialized()
    {
        // Arrange & Act
        var policy = new Policy();

        // Assert
        policy.Id.Should().Be(0);
        policy.PartnerId.Should().Be(0);
        policy.PolicyNumber.Should().Be(string.Empty);
        policy.Amount.Should().Be(0);
        policy.Partner.Should().BeNull();
    }
}
