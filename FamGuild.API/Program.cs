using FamGuild.API.Features.RecurringTransactions.Create;
using FamGuild.API.Features.RecurringTransactions.Get;
using FamGuild.API.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<FamGuildDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Database")));

builder.Services.AddCreateRecurringTransactionCommandHandlerToDependencyInjection();
builder.Services.AddGetRecurringTransactionCommandHandlerToDependencyInjection();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) app.MapOpenApi();

app.UseHttpsRedirection();

app.RegisterCreateRecurringTransactionEndpoints();
app.RegisterGetRecurringTransactionEndpoints();

app.Run();