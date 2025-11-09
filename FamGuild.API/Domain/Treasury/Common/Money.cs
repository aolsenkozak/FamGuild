using Microsoft.EntityFrameworkCore;

namespace FamGuild.API.Domain.Treasury.Common;

[Owned]
public record Money(decimal Value, string CurrencyCode);