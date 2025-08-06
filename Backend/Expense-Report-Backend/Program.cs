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


app.Run();



