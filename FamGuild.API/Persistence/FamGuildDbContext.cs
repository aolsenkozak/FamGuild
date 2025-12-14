using FamGuild.API.Treasury.AccountTransactions;
using FamGuild.API.Treasury.RecurringTransactions;
using FamGuild.Domain.Treasury;
using Microsoft.EntityFrameworkCore;

namespace FamGuild.API.Persistence;

public class FamGuildDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<RecurringTransaction> RecurringTransactions { get; set; }
    public DbSet<AccountTransaction> AccountTransactions { get; set; }
}