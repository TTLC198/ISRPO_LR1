namespace ISRPO_LR1.Domain.Service;

public class ServiceResponseModel<T>
{
    public string Error { get; set; }
    public T Item { get; set; }
    public bool Success { get; set; }
}