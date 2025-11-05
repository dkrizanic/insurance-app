# InsuranceApp Tests

Comprehensive test suite for the Insurance Partner Management System.

## Test Structure

### Services Tests (`Services/`)
- **PartnerServiceTests.cs** - Tests for partner business logic
  - GetAllPartnersAsync
  - GetPartnerByIdAsync
  - GetPartnerWithPoliciesAsync
  - CreatePartnerAsync (verifies CreatedAtUtc setting)
  - PartnerExistsAsync

- **PolicyServiceTests.cs** - Tests for policy business logic
  - CreatePolicyAsync (with various amounts)
  - PolicyExistsAsync
  - ViewModel to Model mapping

### Models Tests (`Models/`)
- **PartnerTests.cs** - Tests for Partner domain model
  - FullName concatenation
  - TotalPolicyAmount calculation
  - PolicyCount calculation
  - RequiresHighlight logic (>5 policies OR >5000 amount)
  - Property validation

- **PolicyTests.cs** - Tests for Policy domain model
  - Property validation
  - Valid policy numbers (10-15 characters)
  - Various amount ranges
  - Default values

### Controllers Tests (`Controllers/`)
- **PartnersControllerTests.cs** - Tests for PartnersController
  - Index action (list all partners)
  - Details action (GET)
  - Create actions (GET & POST)
  - AddPolicy actions (GET & POST)
  - Validation and error handling
  - RedirectToAction behaviors
  - TempData usage

### Validation Tests (`Validation/`)
- **ValidationTests.cs** - Data annotation validation tests
  - Partner field validations (FirstName, LastName, PartnerNumber, CroatianPIN, ExternalCode, Email)
  - Policy field validations (PolicyNumber, Amount)
  - PolicyViewModel validations

## Running Tests

### Run all tests
```powershell
dotnet test
```

### Run with detailed output
```powershell
dotnet test --verbosity normal
```

### Run specific test class
```powershell
dotnet test --filter "FullyQualifiedName~PartnerServiceTests"
```

### Run with code coverage
```powershell
dotnet test --collect:"XPlat Code Coverage"
```

## Test Statistics

- **Total Tests:** 78
- **Service Tests:** 14
- **Model Tests:** 18
- **Controller Tests:** 33
- **Validation Tests:** 13

## Testing Stack

- **xUnit** - Test framework
- **Moq** - Mocking framework
- **FluentAssertions** - Assertion library
- **Microsoft.AspNetCore.Mvc.Testing** - Integration testing support

## Test Patterns

### Arrange-Act-Assert
All tests follow the AAA pattern for clarity:
```csharp
[Fact]
public async Task ServiceMethod_Scenario_ExpectedBehavior()
{
    // Arrange - Set up test data and mocks
    // Act - Execute the method under test
    // Assert - Verify the results
}
```

### Theory Tests
Data-driven tests use `[Theory]` and `[InlineData]`:
```csharp
[Theory]
[InlineData(6, 0, true)]
[InlineData(3, 6000, true)]
public void RequiresHighlight_ShouldReturnCorrectValue(int count, decimal amount, bool expected)
{
    // Test implementation
}
```

### Mocking
Services are mocked using Moq:
```csharp
_partnerServiceMock.Setup(x => x.GetPartnerByIdAsync(partnerId))
    .ReturnsAsync(partner);
```

## Continuous Integration

These tests are designed to run in CI/CD pipelines:
- Fast execution (< 1 second)
- No external dependencies
- Deterministic results
- Clear failure messages

## Future Enhancements

- [ ] Integration tests with WebApplicationFactory
- [ ] Database integration tests with TestContainers
- [ ] Performance tests
- [ ] End-to-end UI tests
- [ ] Test data builders
