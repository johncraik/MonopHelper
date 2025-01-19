namespace MonopolyCL.Models.Validation;

public class ValidationResponse<T> : ValidationResponse
{
    public T? ReturnObj { get; set; }

    public ValidationResponse(string key, string msg)
        : base(key, msg)
    {
    }

    public ValidationResponse()
    {
    }
}

public class ValidationResponse
{
    public bool IsValid { get; }
    public string ErrorKey { get; }
    public string ErrorMsg { get; }

    public ValidationResponse(string key, string msg)
    {
        IsValid = false;
        ErrorKey = key;
        ErrorMsg = msg;
    }

    public ValidationResponse()
    {
        IsValid = true;
        ErrorKey = "";
        ErrorMsg = "";
    }
}