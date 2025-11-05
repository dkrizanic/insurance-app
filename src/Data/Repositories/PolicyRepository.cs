using System.Data;
using Dapper;
using InsuranceApp.Models;
using InsuranceApp.Data.Repositories.Interfaces;

namespace InsuranceApp.Data.Repositories;

public class PolicyRepository : IPolicyRepository
{
    private readonly IDbConnection _db;

    public PolicyRepository(IDbConnection db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Policy>> GetByPartnerIdAsync(int partnerId)
    {
        const string sql = "SELECT * FROM Policies WHERE PartnerId = @PartnerId";
        return await _db.QueryAsync<Policy>(sql, new { PartnerId = partnerId });
    }

    public async Task<int> CreateAsync(Policy policy)
    {
        const string sql = @"
            INSERT INTO Policies (PartnerId, PolicyNumber, Amount)
            VALUES (@PartnerId, @PolicyNumber, @Amount);
            SELECT CAST(SCOPE_IDENTITY() as int)";

        return await _db.QuerySingleAsync<int>(sql, policy);
    }

    public async Task<bool> ExistsAsync(string policyNumber)
    {
        const string sql = "SELECT COUNT(1) FROM Policies WHERE PolicyNumber = @PolicyNumber";
        var count = await _db.ExecuteScalarAsync<int>(sql, new { PolicyNumber = policyNumber });
        return count > 0;
    }
}