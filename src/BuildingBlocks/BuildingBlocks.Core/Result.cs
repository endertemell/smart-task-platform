namespace BuildingBlocks.Core;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public List<string> Errors { get; }

    protected Result(bool isSuccess, List<string> errors)
    {
        if (isSuccess && errors.Count > 0 || !isSuccess && errors.Count == 0)
        {
            throw new InvalidOperationException();
        }

        IsSuccess = isSuccess;
        Errors = errors;
    }

    public static Result Success() => new(true, []);
    public static Result Failure(string errorMessage) => new(false, [errorMessage]);
    public static Result Failure(List<string> errors) => new(false, errors);
}

public class Result<T> : Result
{
    private readonly T? _value;

    protected internal Result(T? value, bool isSuccess, List<string> errors) 
        : base(isSuccess, errors)
    {
        _value = value;
    }

    public T Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("The value of a failure result can not be accessed.");

    public static Result<T> Success(T value) => new(value, true, []);
    public new static Result<T> Failure(string errorMessage) => new(default, false, new List<string> { errorMessage });
    public new static Result<T> Failure(List<string> errors) => new(default, false, errors);
}
