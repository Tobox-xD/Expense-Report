using NSwag.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Npgsql;

using static ExpenseEntry;

var builder = WebApplication.CreateBuilder(args);
var connectionString = "Host=localhost;Port=5432;Username=postgres;Password=Databases123!;Database=postgres";

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost3000", policy =>
    {
        policy.WithOrigins(
                "http://localhost:3000",
                "http://localhost:5500",
                "http://127.0.0.1:5500")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument();

var app = builder.Build();

app.UseCors("AllowLocalhost3000");

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi(config =>
    {
        config.DocumentTitle = "ExpenseReport API";
        config.Path = "/swagger";
        config.DocumentPath = "/swagger/{documentName}/swagger.json";
        config.DocExpansion = "list";
    });
}

app.Urls.Add("http://localhost:3000");

// GET
app.MapGet("/getReportById/{id}", async (int id) =>
{
    await using var conn = new NpgsqlConnection(connectionString);
    await conn.OpenAsync();

    await using var cmd = new NpgsqlCommand(
        "SELECT * FROM expense JOIN report ON expense.report_id = report.id WHERE report_id = @id;", conn);
    cmd.Parameters.AddWithValue("id", id);

    await using var reader = await cmd.ExecuteReaderAsync();

    var results = new List<Dictionary<string, object>>();
    while (await reader.ReadAsync())
    {
        var row = new Dictionary<string, object>();
        for (int i = 0; i < reader.FieldCount; i++)
        {
            row[reader.GetName(i)] = reader.GetValue(i);
        }
        results.Add(row);
    }

    return Results.Ok(results);
});

app.MapGet("/getReportSumById/{id}", async (int id) =>
{
    await using var conn = new NpgsqlConnection(connectionString);
    await conn.OpenAsync();

    await using var cmd = new NpgsqlCommand(
        "SELECT SUM(amount) FROM expense WHERE report_id = @id;", conn);
    cmd.Parameters.AddWithValue("id", id);

    var sumObj = await cmd.ExecuteScalarAsync();

    // sumObj can be DBNull if no rows, so handle null safely:
    decimal sum = sumObj != DBNull.Value && sumObj != null ? Convert.ToDecimal(sumObj) : 0m;

    return Results.Ok(sum);
});

app.MapPost("/addEntry/{id}", async (int id, ExpenseEntry entry) =>
{
    await using var conn = new NpgsqlConnection(connectionString);
    await conn.OpenAsync();

    var sql = @"INSERT INTO expense (description, amount, created_at, report_id) VALUES (@description, @amount, @created_at, @report_id)";
    await using var cmd = new NpgsqlCommand(sql, conn);

    cmd.Parameters.AddWithValue("description", entry.Description);
    cmd.Parameters.AddWithValue("amount", entry.Amount);
    cmd.Parameters.AddWithValue("created_at", entry.CreatedAt);
    cmd.Parameters.AddWithValue("report_id", id);

    await cmd.ExecuteNonQueryAsync();

    return Results.Ok();
});

app.MapPut("/changeEntry/{id}", (int id) => Results.Ok());

app.MapDelete("/deleteEntry/{id}", (int id) => Results.Ok());

app.Run();
