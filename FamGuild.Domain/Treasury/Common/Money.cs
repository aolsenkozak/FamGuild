using Microsoft.EntityFrameworkCore;

namespace FamGuild.Domain.Treasury.Common;

[Owned]
public record Money(decimal Value, string CurrencyCode);