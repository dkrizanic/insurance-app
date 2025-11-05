using FluentAssertions;
using InsuranceApp.Data.Repositories.Interfaces;
using InsuranceApp.Models;
using InsuranceApp.Models.ViewModels;
using InsuranceApp.Services;
using Moq;

namespace InsuranceApp.Tests.Services;

public class PolicyServiceTests
{
    private readonly Mock<IPolicyRepository> _policyRepositoryMock;
    private readonly PolicyService _sut;

    public PolicyServiceTests()
    {
        _policyRepositoryMock = new Mock<IPolicyRepository>();
        _sut = new PolicyService(_policyRepositoryMock.Object);
    }

    [Fact]
    public async Task CreatePolicyAsync_ShouldSetCreatedAtUtcAndCreatePolicy()
    {
        // Arrange
        var viewModel = new PolicyViewModel
        {
            PartnerId = 1,
            PolicyNumber = "POL-123456789",
            Amount = 1500.50m,
            PartnerName = "John Doe"
        };
        var expectedId = 1;
        _policyRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<Policy>()))
            .ReturnsAsync(expectedId);

        var beforeCreate = DateTime.UtcNow;

        // Act
        var result = await _sut.CreatePolicyAsync(viewModel);

        var afterCreate = DateTime.UtcNow;

        // Assert
        result.Should().Be(expectedId);
        _policyRepositoryMock.Verify(x => x.CreateAsync(It.Is<Policy>(p =>
            p.PartnerId == viewModel.PartnerId &&
            p.PolicyNumber == viewModel.PolicyNumber &&
            p.Amount == viewModel.Amount &&
            p.CreatedAtUtc >= beforeCreate &&
            p.CreatedAtUtc <= afterCreate
        )), Times.Once);
    }

    [Fact]
    public async Task CreatePolicyAsync_ShouldMapViewModelToPolicy()
    {
        // Arrange
        var viewModel = new PolicyViewModel
        {
            PartnerId = 5,
            PolicyNumber = "POL-987654321",
            Amount = 3000.75m,
            PartnerName = "Jane Smith"
        };
        var expectedId = 10;
        Policy? capturedPolicy = null;
        _policyRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<Policy>()))
            .Callback<Policy>(p => capturedPolicy = p)
            .ReturnsAsync(expectedId);

        // Act
        var result = await _sut.CreatePolicyAsync(viewModel);

        // Assert
        result.Should().Be(expectedId);
        capturedPolicy.Should().NotBeNull();
        capturedPolicy!.PartnerId.Should().Be(viewModel.PartnerId);
        capturedPolicy.PolicyNumber.Should().Be(viewModel.PolicyNumber);
        capturedPolicy.Amount.Should().Be(viewModel.Amount);
    }

    [Fact]
    public async Task PolicyExistsAsync_WhenPolicyExists_ShouldReturnTrue()
    {
        // Arrange
        var policyNumber = "POL-123456789";
        _policyRepositoryMock.Setup(x => x.ExistsAsync(policyNumber))
            .ReturnsAsync(true);

        // Act
        var result = await _sut.PolicyExistsAsync(policyNumber);

        // Assert
        result.Should().BeTrue();
        _policyRepositoryMock.Verify(x => x.ExistsAsync(policyNumber), Times.Once);
    }

    [Fact]
    public async Task PolicyExistsAsync_WhenPolicyDoesNotExist_ShouldReturnFalse()
    {
        // Arrange
        var policyNumber = "POL-999999999";
        _policyRepositoryMock.Setup(x => x.ExistsAsync(policyNumber))
            .ReturnsAsync(false);

        // Act
        var result = await _sut.PolicyExistsAsync(policyNumber);

        // Assert
        result.Should().BeFalse();
        _policyRepositoryMock.Verify(x => x.ExistsAsync(policyNumber), Times.Once);
    }

    [Theory]
    [InlineData(100.00)]
    [InlineData(0.01)]
    [InlineData(999999.99)]
    public async Task CreatePolicyAsync_WithVariousAmounts_ShouldCreateSuccessfully(decimal amount)
    {
        // Arrange
        var viewModel = new PolicyViewModel
        {
            PartnerId = 1,
            PolicyNumber = $"POL-{amount:0000}",
            Amount = amount,
            PartnerName = "Test Partner"
        };
        var expectedId = 1;
        _policyRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<Policy>()))
            .ReturnsAsync(expectedId);

        // Act
        var result = await _sut.CreatePolicyAsync(viewModel);

        // Assert
        result.Should().Be(expectedId);
        _policyRepositoryMock.Verify(x => x.CreateAsync(It.Is<Policy>(p => p.Amount == amount)), Times.Once);
    }
}
