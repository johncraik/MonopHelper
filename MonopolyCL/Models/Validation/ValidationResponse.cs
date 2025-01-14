namespace MonopolyCL.Models.Validation;

public class ValidationResponse
{
    public bool IsValid { get; }
    public string ErrorKey { get; }
    public string ErrorMsg { get; }

    public ValidationResponse(bool isValid, string key, string msg)
    {
        IsValid = isValid;
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