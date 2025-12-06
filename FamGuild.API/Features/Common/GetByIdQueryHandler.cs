using FamGuild.Domain.Common.ResultPattern;
using FamGuild.API.Persistence;

namespace FamGuild.API.Features.Common;

public class GetByIdQueryHandler<TEntity>(FamGuildDbContext dbContext) :
    IQueryHandler<GetByIdQuery, Result<TEntity>>
        where TEntity : class
{
    public async Task<Result<TEntity>> HandleAsync(GetByIdQuery query, CancellationToken ct = default)
    {
        var entity = await dbContext.Set<TEntity>().FindAsync(query.Id, ct);

        if (entity is null)
        {
            var error = new Error("NotFound", "The entity was not found.");
            return Result.Failure<TEntity>(error);
        }
        
        return Result.Success(entity);
    }
}