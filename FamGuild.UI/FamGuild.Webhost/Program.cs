using FamGuild.Core;
using FamGuild.UI.API.Treasury.AccountTransactions;
using FamGuild.UI.API.Treasury.RecurringTransactions;
using FamGuild.UI.Components;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddOpenApi();

builder.Services.AddDbContext<FamGuildDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Database")));

builder.Services.AddCreateRecurringTransactionCommandHandlerToDependencyInjection();
builder.Services.AddGetRecurringTransactionCommandHandlerToDependencyInjection();
builder.Services.AddGetAccountTransactionQueryHandlerToDependencyInjection();
builder.Services.AddCreateAccountTransactionHandlerToDependencyInjection();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.RegisterCreateRecurringTransactionEndpoints();
app.RegisterGetRecurringTransactionEndpoints();
app.RegisterCreateAccountTransactionEndpoints();
app.RegisterGetAccountTransactionEndpoints();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode();

app.Run();