using System.Data;
using Dapper;
using InsuranceApp.Models;
using InsuranceApp.Models.ViewModels;
using InsuranceApp.Data.Repositories.Interfaces;

namespace InsuranceApp.Data.Repositories;

public class PartnerRepository : IPartnerRepository
{
    private readonly IDbConnection _db;

    public PartnerRepository(IDbConnection db)
    {
        _db = db;
    }

    public async Task<IEnumerable<PartnerListViewModel>> GetAllAsync()
    {
        const string sql = @"
            SELECT 
                p.Id,
                p.FirstName + ' ' + p.LastName as FullName,
                p.PartnerNumber,
                p.CroatianPIN,
                p.PartnerTypeId,
                p.CreatedAtUtc,
                p.IsForeign,
                p.Gender,
                COUNT(pol.Id) as PolicyCount,
                COALESCE(SUM(pol.Amount), 0) as TotalPolicyAmount,
                CASE 
                    WHEN COUNT(pol.Id) > 5 OR COALESCE(SUM(pol.Amount), 0) > 5000 
                    THEN 1 
                    ELSE 0 
                END as RequiresHighlight
            FROM Partners p
            LEFT JOIN Policies pol ON p.Id = pol.PartnerId
            GROUP BY 
                p.Id, p.FirstName, p.LastName, p.PartnerNumber, p.CroatianPIN,
                p.PartnerTypeId, p.CreatedAtUtc, p.IsForeign, p.Gender
            ORDER BY p.CreatedAtUtc DESC";

        return await _db.QueryAsync<PartnerListViewModel>(sql);
    }

    public async Task<Partner?> GetByIdAsync(int id)
    {
        const string sql = "SELECT * FROM Partners WHERE Id = @Id";
        return await _db.QueryFirstOrDefaultAsync<Partner>(sql, new { Id = id });
    }

    public async Task<int> CreateAsync(Partner partner)
    {
        const string sql = @"
            INSERT INTO Partners (
                FirstName, LastName, Address, PartnerNumber, CroatianPIN,
                PartnerTypeId, CreateByUser, IsForeign, ExternalCode, Gender
            ) VALUES (
                @FirstName, @LastName, @Address, @PartnerNumber, @CroatianPIN,
                @PartnerTypeId, @CreateByUser, @IsForeign, @ExternalCode, @Gender
            );
            SELECT CAST(SCOPE_IDENTITY() as int)";

        return await _db.QuerySingleAsync<int>(sql, partner);
    }

    public async Task<bool> ExistsAsync(string externalCode)
    {
        const string sql = "SELECT COUNT(1) FROM Partners WHERE ExternalCode = @ExternalCode";
        var count = await _db.ExecuteScalarAsync<int>(sql, new { ExternalCode = externalCode });
        return count > 0;
    }

    public async Task<Partner?> GetWithPoliciesAsync(int id)
    {
        const string sql = @"
            SELECT p.*, pol.*
            FROM Partners p
            LEFT JOIN Policies pol ON p.Id = pol.PartnerId
            WHERE p.Id = @Id";

        var partnerDictionary = new Dictionary<int, Partner>();
        
        await _db.QueryAsync<Partner, Policy, Partner>(
            sql,
            (partner, policy) =>
            {
                if (!partnerDictionary.TryGetValue(partner.Id, out var existingPartner))
                {
                    existingPartner = partner;
                    existingPartner.Policies = new List<Policy>();
                    partnerDictionary.Add(partner.Id, existingPartner);
                }

                if (policy != null)
                {
                    existingPartner.Policies.Add(policy);
                }

                return existingPartner;
            },
            new { Id = id },
            splitOn: "Id"
        );

        return partnerDictionary.Values.FirstOrDefault();
    }
}