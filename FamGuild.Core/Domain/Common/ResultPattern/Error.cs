namespace FamGuild.Core.Domain.Common.ResultPattern;

public record Error(string Code, string Message)
{
    public static Error None = new(string.Empty, string.Empty);
    public static Error NullValue = new("Error.NullValue", "Null value found where it is not allowed.");
}