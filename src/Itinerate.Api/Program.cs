using Microsoft.EntityFrameworkCore;
using Itinerate.Infrastructure;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Text.Json.Serialization;
using Itinerate.Domain;

var builder = WebApplication.CreateBuilder(args);

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:4200") 
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
    }
}

// Configure the HTTP request pipeline.
// Unconditionally enable Swagger and Swagger UI for this project
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseCors(MyAllowSpecificOrigins); 

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