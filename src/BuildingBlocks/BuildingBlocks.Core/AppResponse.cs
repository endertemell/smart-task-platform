namespace BuildingBlocks.Core;

public class AppResponse<T>
{
    public T? Data { get; set; }
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
    public List<string>? Errors { get; set; }

    public static AppResponse<T> Success(T data, string? message = null)
    {
        return new AppResponse<T> { Data = data, IsSuccess = true, Message = message };
    }

    public static AppResponse<T> Failure(List<string> errors, string? message = null)
    {
        return new AppResponse<T> { IsSuccess = false, Errors = errors, Message = message };
    }

    public static AppResponse<T> Failure(string error, string? message = null)
    {
        return new AppResponse<T> { IsSuccess = false, Errors = [error], Message = message };
    }
}
