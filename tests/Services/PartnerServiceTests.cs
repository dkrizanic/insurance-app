using FluentAssertions;
using InsuranceApp.Data.Repositories.Interfaces;
using InsuranceApp.Models;
using InsuranceApp.Models.Enums;
using InsuranceApp.Models.ViewModels;
using InsuranceApp.Services;
using Moq;

namespace InsuranceApp.Tests.Services;

public class PartnerServiceTests
{
    private readonly Mock<IPartnerRepository> _partnerRepositoryMock;
    private readonly PartnerService _sut;

    public PartnerServiceTests()
    {
        _partnerRepositoryMock = new Mock<IPartnerRepository>();
        _sut = new PartnerService(_partnerRepositoryMock.Object);
    }

    [Fact]
    public async Task GetAllPartnersAsync_ShouldReturnAllPartners()
    {
        // Arrange
        var expectedPartners = new List<PartnerListViewModel>
        {
            new() { Id = 1, FullName = "John Doe", PartnerNumber = "12345678901234567890" },
            new() { Id = 2, FullName = "Jane Smith", PartnerNumber = "09876543210987654321" }
        };
        _partnerRepositoryMock.Setup(x => x.GetAllAsync())
            .ReturnsAsync(expectedPartners);

        // Act
        var result = await _sut.GetAllPartnersAsync();

        // Assert
        result.Should().BeEquivalentTo(expectedPartners);
        _partnerRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetPartnerByIdAsync_WhenPartnerExists_ShouldReturnPartner()
    {
        // Arrange
        var partnerId = 1;
        var expectedPartner = new Partner
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
        _partnerRepositoryMock.Setup(x => x.GetByIdAsync(partnerId))
            .ReturnsAsync(expectedPartner);

        // Act
        var result = await _sut.GetPartnerByIdAsync(partnerId);

        // Assert
        result.Should().BeEquivalentTo(expectedPartner);
        _partnerRepositoryMock.Verify(x => x.GetByIdAsync(partnerId), Times.Once);
    }

    [Fact]
    public async Task GetPartnerByIdAsync_WhenPartnerDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        var partnerId = 999;
        _partnerRepositoryMock.Setup(x => x.GetByIdAsync(partnerId))
            .ReturnsAsync((Partner?)null);

        // Act
        var result = await _sut.GetPartnerByIdAsync(partnerId);

        // Assert
        result.Should().BeNull();
        _partnerRepositoryMock.Verify(x => x.GetByIdAsync(partnerId), Times.Once);
    }

    [Fact]
    public async Task GetPartnerWithPoliciesAsync_ShouldReturnPartnerWithPolicies()
    {
        // Arrange
        var partnerId = 1;
        var expectedPartner = new Partner
        {
            Id = partnerId,
            FirstName = "John",
            LastName = "Doe",
            PartnerNumber = "12345678901234567890",
            ExternalCode = "EXT-1234567890",
            CreateByUser = "test@example.com",
            Gender = Gender.M,
            PartnerTypeId = PartnerType.Personal,
            Policies = new List<Policy>
            {
                new() { Id = 1, PolicyNumber = "POL-123456", Amount = 1000m, PartnerId = partnerId },
                new() { Id = 2, PolicyNumber = "POL-789012", Amount = 2000m, PartnerId = partnerId }
            }
        };
        _partnerRepositoryMock.Setup(x => x.GetWithPoliciesAsync(partnerId))
            .ReturnsAsync(expectedPartner);

        // Act
        var result = await _sut.GetPartnerWithPoliciesAsync(partnerId);

        // Assert
        result.Should().NotBeNull();
        result!.Policies.Should().HaveCount(2);
        result.TotalPolicyAmount.Should().Be(3000m);
        _partnerRepositoryMock.Verify(x => x.GetWithPoliciesAsync(partnerId), Times.Once);
    }

    [Fact]
    public async Task CreatePartnerAsync_ShouldSetCreatedAtUtcAndCreatePartner()
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
        var expectedId = 1;
        _partnerRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<Partner>()))
            .ReturnsAsync(expectedId);

        var beforeCreate = DateTime.UtcNow;

        // Act
        var result = await _sut.CreatePartnerAsync(partner);

        var afterCreate = DateTime.UtcNow;

        // Assert
        result.Should().Be(expectedId);
        partner.CreatedAtUtc.Should().BeOnOrAfter(beforeCreate).And.BeOnOrBefore(afterCreate);
        _partnerRepositoryMock.Verify(x => x.CreateAsync(partner), Times.Once);
    }

    [Fact]
    public async Task PartnerExistsAsync_WhenPartnerExists_ShouldReturnTrue()
    {
        // Arrange
        var externalCode = "EXT-1234567890";
        _partnerRepositoryMock.Setup(x => x.ExistsAsync(externalCode))
            .ReturnsAsync(true);

        // Act
        var result = await _sut.PartnerExistsAsync(externalCode);

        // Assert
        result.Should().BeTrue();
        _partnerRepositoryMock.Verify(x => x.ExistsAsync(externalCode), Times.Once);
    }

    [Fact]
    public async Task PartnerExistsAsync_WhenPartnerDoesNotExist_ShouldReturnFalse()
    {
        // Arrange
        var externalCode = "EXT-9999999999";
        _partnerRepositoryMock.Setup(x => x.ExistsAsync(externalCode))
            .ReturnsAsync(false);

        // Act
        var result = await _sut.PartnerExistsAsync(externalCode);

        // Assert
        result.Should().BeFalse();
        _partnerRepositoryMock.Verify(x => x.ExistsAsync(externalCode), Times.Once);
    }
}
