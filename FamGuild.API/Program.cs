using FamGuild.API.Features.RecurringItems.Create;
using FamGuild.API.Features.RecurringItems.Get;
using FamGuild.API.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<FamGuildDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Database")));

builder.Services.AddCreateRecurringItemCommandHandlerToDependencyInjection();
builder.Services.AddGetRecurringItemCommandHandlerToDependencyInjection();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) app.MapOpenApi();

app.UseHttpsRedirection();

app.RegisterCreateRecurringItemEndpoints();
app.RegisterGetRecurringItemEndpoints();

app.Run();