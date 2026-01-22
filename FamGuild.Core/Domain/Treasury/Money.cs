using Microsoft.EntityFrameworkCore;

namespace FamGuild.Core.Domain.Treasury;

[Owned]
public record Money(decimal Value, string CurrencyCode);