using System.Diagnostics.CodeAnalysis;

namespace FamGuild.Core.Domain.Common.ResultPattern;

public class Result
{
    protected Result(bool isSuccess, Error error)
    {
        switch (isSuccess)
        {
            case true when error != Error.None:
                throw new InvalidOperationException("Error code should be Error.None when result is successful.");
            case false when error == Error.None:
                throw new InvalidOperationException(
                    "Error code should not be Error.None when result is not successful.");
            default:
                IsSuccess = isSuccess;
                Error = error;
                break;
        }
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }

    public static Result Success()
    {
        return new Result(true, Error.None);
    }

    public static Result Failure(Error error)
    {
        return new Result(false, error);
    }

    public static Result<T> Success<T>(T value)
    {
        return new Result<T>(value, true, Error.None);
    }

    public static Result<T> Failure<T>(Error error)
    {
        return new Result<T>(default, false, error);
    }

    public static Result<T> Create<T>(T? value)
    {
        return value is not null ? Success(value) : Failure<T>(Error.NullValue);
    }
}

public class Result<T> : Result
{
    private readonly T? _value;

    protected internal Result(T? value, bool isSuccess, Error error) : base(isSuccess, error)
    {
        _value = value;
    }

    [NotNull] public T Value => _value! ?? throw new InvalidOperationException("Result has no value.");

    public static implicit operator Result<T>(T? value)
    {
        return Create(value);
    }
}