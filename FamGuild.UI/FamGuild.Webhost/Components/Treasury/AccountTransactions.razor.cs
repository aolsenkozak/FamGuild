using FamGuild.Core.Application.Common;
using FamGuild.Core.Application.Treasury.AccountTransactions;
using FamGuild.Core.Application.Treasury.AccountTransactions.Get;
using FamGuild.Core.Domain.Common.ResultPattern;
using FamGuild.UI.Common;
using Microsoft.AspNetCore.Components;

namespace FamGuild.UI.Components.Treasury;

public partial class AccountTransactions : ComponentBase
{
    [Inject]
    public required IQueryHandler<GetAccountTransactionsQuery, Result<List<AccountTransactionDto>>> QueryHandler { get; set; }
    
    private IQueryable<AccountTransactionDto> _accountTransactions = 
        Enumerable.Empty<AccountTransactionDto>().AsQueryable();
    
    private string _errorMessage =  string.Empty;

    protected override async Task OnInitializedAsync()
    {
        DateRange transactionDateRange = new(DateOnly.FromDateTime(DateTime.Now), DateOnly.FromDateTime(DateTime.Now));
        
        _accountTransactions = await GetAccountTransactions(transactionDateRange);
    }

    private async Task<IQueryable<AccountTransactionDto>> GetAccountTransactions(DateRange dateRange)
    {
        var query = new GetAccountTransactionsQuery(dateRange.FromDate, dateRange.ToDate);
        var result = await QueryHandler.HandleAsync(query);

        if (result.IsFailure)
        {
            _errorMessage = result.Error.Message;
            return Enumerable.Empty<AccountTransactionDto>().AsQueryable();
        }

        return result.Value.AsQueryable();
    }

    
    
    

}