using System.Data;
using Microsoft.Data.SqlClient;
using InsuranceApp.Data.Repositories;
using InsuranceApp.Data.Repositories.Interfaces;
using InsuranceApp.Services;
using InsuranceApp.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add database connection for Dapper
builder.Services.AddScoped<IDbConnection>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    return new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
});

// Register repositories
builder.Services.AddScoped<IPartnerRepository, PartnerRepository>();
builder.Services.AddScoped<IPolicyRepository, PolicyRepository>();

// Register services
builder.Services.AddScoped<IPartnerService, PartnerService>();
builder.Services.AddScoped<IPolicyService, PolicyService>();

var app = builder.Build();

// Initialize database schema using SQL script
using (var scope = app.Services.CreateScope())
{
    var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    
    await EnsureDatabaseCreatedAsync(connectionString!);
}

async Task EnsureDatabaseCreatedAsync(string connectionString)
{
    using var connection = new SqlConnection(connectionString);
    await connection.OpenAsync();
    
    // Check if tables already exist
    var tableExistsQuery = @"
        SELECT COUNT(*) 
        FROM INFORMATION_SCHEMA.TABLES 
        WHERE TABLE_NAME IN ('Partners', 'Policies')";
    
    using var checkCommand = new SqlCommand(tableExistsQuery, connection);
    var result = await checkCommand.ExecuteScalarAsync();
    var tableCount = result != null ? (int)result : 0;
    
    if (tableCount == 0)
    {
        // Read and execute the schema script
        var schemaScript = await File.ReadAllTextAsync(
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Scripts", "InitialSchema.sql"));
        
        // Split by GO statements and execute each batch
        var batches = schemaScript.Split(new[] { "\r\nGO\r\n", "\nGO\n", "GO" }, 
            StringSplitOptions.RemoveEmptyEntries);
        
        foreach (var batch in batches)
        {
            if (!string.IsNullOrWhiteSpace(batch))
            {
                using var command = new SqlCommand(batch, connection);
                await command.ExecuteNonQueryAsync();
            }
        }
        
        Console.WriteLine("Database schema created successfully.");
    }
    else
    {
        Console.WriteLine("Database schema already exists.");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Partners}/{action=Index}/{id?}");

app.Run();