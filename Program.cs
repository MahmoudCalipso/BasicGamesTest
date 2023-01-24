using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Text.Json;
using BasicGames.Data;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<BasicGamesContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("BasicGamesContext") ?? 
    throw new InvalidOperationException("Connection string 'BasicGamesContext' not found.")
));

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Global settings: use the defaults, but serialize enums as strings
        // (because it really should be the default)
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter(JsonNamingPolicy.CamelCase, false));
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });
builder.Services.AddCors(p => p.AddPolicy("CorsApp", builder =>
    {
        builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
    }));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();
app.MapGet("/", async context =>
{
    context.Response.ContentLength = 0;
    var upgradeStream = await context.Features.Get<IHttpUpgradeFeature>().UpgradeAsync();
    await upgradeStream.WriteAsync(new byte[1]);
});
app.MapControllers();
app.UseCors("CorsApp");

app.Run();
