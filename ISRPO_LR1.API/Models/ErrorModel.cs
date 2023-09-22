namespace ISRPO_LR1.API.Models;

public class ErrorModel
{
    public string Message { get; set; }

    public ErrorModel(string message)
    {
        Message = message;
    }
}