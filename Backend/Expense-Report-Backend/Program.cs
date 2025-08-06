using Npgsql;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// SQL Connection
var connectionString = "Host=localhost;Port=5432;Username=postgres;Password=Databases123!;Database=postgres";

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.Urls.Add("http://localhost:3000");

app.UseHttpsRedirection();
app.UseCors("AllowAll");



// GET Requests 
app.MapGet("/getReportById/{id}", (string id) =>
{
    var report = new List<object>();

    using var connection = new NpgsqlConnection(connectionString);
    connection.Open();

    var sql = @"
        SELECT *
        FROM expense
        JOIN report ON expense.report_id = report.id
        WHERE expense.report_id = @id;
    ";

    using var cmd = new NpgsqlCommand(sql, connection);
    cmd.Parameters.AddWithValue("id", int.Parse(id));

    using var reader = cmd.ExecuteReader();

    while (reader.Read())
    {
        report.Add(new
        {
            Id = reader.GetInt32(0),
            Description = reader.GetString(1),
            Amount = reader.GetDecimal(2),
            CreatedAt = reader.GetDateTime(3),
            ReportId = reader.GetInt32(4)
        });
    }

    return report;
});

app.MapGet("/getReportSumById/{id}", (string id) =>
{

    decimal SumOfAmounts = 0;

    using var connection = new NpgsqlConnection(connectionString);
    connection.Open();

    var sql = @"
        SELECT SUM(expense.amount)
        FROM expense 
        JOIN report ON expense.report_id = report.id
        WHERE report_id = @id;
    ";

    using var cmd = new NpgsqlCommand(sql, connection);
    cmd.Parameters.AddWithValue("id", int.Parse(id));

    using var reader = cmd.ExecuteReader();

    while (reader.Read())
    {
        SumOfAmounts = reader.GetDecimal(0);
    }


    return SumOfAmounts;
});

// POST requests



app.MapPost("/addEntry/{id}", async (int id, ExpenseCreateDto dto) =>
{
    // basic validation
    if (string.IsNullOrWhiteSpace(dto.Description) || dto.Amount < 0)
        return Results.BadRequest("Invalid input");

    await using var connection = new NpgsqlConnection(connectionString);
    await connection.OpenAsync();

    var sql = @"
        INSERT INTO expense (description, amount, created_at, report_id)
        VALUES (@description, @amount, @created_at, @report_id);
    ";

    await using var cmd = new NpgsqlCommand(sql, connection);
    cmd.Parameters.AddWithValue("description", dto.Description);
    cmd.Parameters.AddWithValue("amount", dto.Amount);
    cmd.Parameters.AddWithValue("created_at", dto.CreatedAt);
    cmd.Parameters.AddWithValue("report_id", id);

    var rows = await cmd.ExecuteNonQueryAsync();

    return rows > 0
        ? Results.Created($"/getReportById/{id}", null)
        : Results.Problem("Insert did not affect any rows.");
});


app.Run();


// "Datatype" for the JSON POST request
public record ExpenseCreateDto(string Description, decimal Amount, DateTime CreatedAt);


