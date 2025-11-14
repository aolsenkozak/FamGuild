using FamGuild.API.Domain.Common.ResultPattern;
using FamGuild.API.Domain.Treasury;
using FamGuild.API.Features.Common;
using FamGuild.API.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FamGuild.API.Features.RecurringItems.Create;

public class CreateRecurringItemHandler(FamGuildDbContext dbContext)
    : ICommandHandler<CreateRecurringItemCommand, Result<Guid>>
{
    public async Task<Result<Guid>> HandleAsync(CreateRecurringItemCommand command, CancellationToken ct = default)
    {
        var recurringItemResult = RecurringItem.Create(
            command.Type,
            command.Name,
            command.Amount,
            command.Category,
            command.StartDate,
            command.EndDate,
            command.Frequency
        );

        if (recurringItemResult.IsFailure)
        {
            return Result.Failure<Guid>(recurringItemResult.Error);
        }
        
        var newRecurringItem = recurringItemResult.Value;
        
        dbContext.RecurringItems.Add(newRecurringItem);
        try
        {
            await dbContext.SaveChangesAsync(ct);
        }
        catch (Exception ex) when (ex is DbUpdateException || ex is DbUpdateConcurrencyException)
        {
            var error = new Error("DBError", $"Error occurred while adding Recurring Item: {ex.Message}");
            return Result.Failure<Guid>(error);
        }

        return Result.Success(newRecurringItem.Id);
    }


}