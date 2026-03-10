using Microsoft.EntityFrameworkCore;
using SubscribersWebAPI.Data;
using SubscribersWebAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Lägg till Entity Framework med SQL Server
builder.Services.AddDbContext<SubscriberContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Lägg till våra services
builder.Services.AddScoped<ISubscriberService, SubscriberService>();
builder.Services.AddScoped<IAdvertisementService, AdvertisementService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Subscribers Web API med Annonssystem",
        Version = "v1",
        Description = "Web API för hantering av tidningens prenumeranter och annonssystem"
    });
});

// Lägg till CORS för att annonssystemet ska kunna komma åt API:et
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAdvertisementSystem", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Subscribers API v1");
        c.RoutePrefix = "swagger"; // Flytta Swagger från root
    });
}

app.UseHttpsRedirection();

// Aktivera CORS
app.UseCors("AllowAdvertisementSystem");

// Aktivera static files för webbgränssnitt
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

// Skapa databasen och kör migrationer automatiskt vid start
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<SubscriberContext>();
    context.Database.EnsureCreated();
}

app.Run();
