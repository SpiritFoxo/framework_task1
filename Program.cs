using Microsoft.AspNetCore.Http.Json;
using Pr1.MinWebService.Domain;
using Pr1.MinWebService.Errors;
using Pr1.MinWebService.Middlewares;
using Pr1.MinWebService.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JsonOptions>(options =>
{
	options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

builder.Services.AddSingleton<IDisciplineRepository, InMemoryDisciplineRepository>();

var app = builder.Build();

app.UseMiddleware<RequestIdMiddleware>();
app.UseMiddleware<TimingAndLogMiddleware>();
app.UseMiddleware<ErrorHandlingMiddleware>();

// GET /api/disciplines
app.MapGet("/api/disciplines", (IDisciplineRepository repo) =>
    Results.Ok(repo.GetAll()));

// GET /api/disciplines/{id}
app.MapGet("/api/disciplines/{id:guid}", (Guid id, IDisciplineRepository repo) =>
{
    var discipline = repo.GetById(id);
    if (discipline is null)
        throw new NotFoundException("Дисциплина не найдена");
    return Results.Ok(discipline);
});

// POST /api/disciplines
app.MapPost("/api/disciplines", (HttpContext ctx, CreateDisciplineRequest request, IDisciplineRepository repo) =>
{
	if (string.IsNullOrWhiteSpace(request.Name) || request.Name.Trim().Length is < 5 or > 150)
    	throw new ValidationException("The discipline name must be between 5 and 150 characters.");

	if (string.IsNullOrWhiteSpace(request.Lecturer) || request.Lecturer.Trim().Length is < 3 or > 100)
		throw new ValidationException("The lecturer's full name must be between 3 and 100 characters.");

	if (request.StudentCount < 0)
		throw new ValidationException("The number of students cannot be negative.");

	if (request.StudentCount > 1000)
		throw new ValidationException("The number of students cannot exceed 1000.");

	var created = repo.Create(request.Name.Trim(), request.Lecturer.Trim(), request.StudentCount);
    var location = $"/api/disciplines/{created.Id}";
    ctx.Response.Headers.Location = location;
    return Results.Created(location, created);
});

app.Run();

public partial class Program { }