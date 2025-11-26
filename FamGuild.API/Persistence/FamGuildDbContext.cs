using FamGuild.API.Domain.Treasury;
using Microsoft.EntityFrameworkCore;

namespace FamGuild.API.Persistence;

public class FamGuildDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<RecurringTransaction>  RecurringItems { get; set; }
}

