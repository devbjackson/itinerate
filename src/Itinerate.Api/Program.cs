using Microsoft.EntityFrameworkCore;
using Itinerate.Infrastructure;
using Microsoft.AspNetCore.Http.HttpResults;
using Itinerate.Domain;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 1. Define a group for our itinerary endpoints for better organization
var itineraryApi = app.MapGroup("/api/itineraries");

// 2. Define the GET endpoint to retrieve all itineraries
itineraryApi.MapGet("/", async (ApplicationDbContext db) =>
{
    var itineraries = await db.Itineraries.ToListAsync();
    return TypedResults.Ok(itineraries);
});

// 3. Define the POST endpoint to create a new itinerary
// NOTE: We are using the 'Itinerary' class from ApplicationDbContext.cs for this
itineraryApi.MapPost("/", async (Itinerary itinerary, ApplicationDbContext db) =>
{
    db.Itineraries.Add(itinerary);
    await db.SaveChangesAsync();
    return TypedResults.Created($"/api/itineraries/{itinerary.Id}", itinerary);
});

app.Run();