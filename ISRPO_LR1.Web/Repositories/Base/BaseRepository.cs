using System.Text.Json;
using ISRPO_LR1.Domain.Service;

namespace ISRPO_LR1.Web.Repositories.Base;

public abstract class BaseRepository<T>
{
    protected readonly HttpClient HttpClient;
    protected readonly ILogger<BaseRepository<T>> Logger;

    protected string BasePath => $"{typeof(T).Name.ToLower()}s";

    public BaseRepository(HttpClient httpClient, ILogger<BaseRepository<T>> logger)
    {
        HttpClient = httpClient;
        Logger = logger;
    }

    public virtual async Task<ServiceResponseModel<List<T>?>> Get()
    {
        try
        {
            var response = await HttpClient.GetAsync(BasePath);
            if (!response.IsSuccessStatusCode)
                return new ServiceResponseModel<List<T>?>()
                {
                    Error = await response.Content.ReadAsStringAsync()
                };
            return new ServiceResponseModel<List<T>?>()
            {
                Success = true,
                Item = await JsonSerializer.DeserializeAsync<List<T>>(await response.Content.ReadAsStreamAsync())
            };
        }
        catch (Exception exception)
        {
            Logger.LogError(exception.Message);
            return new ServiceResponseModel<List<T>?>()
            {
                Error = exception.Message
            };
        }
    }
    
    public virtual async Task<ServiceResponseModel<T?>> Get(int id)
    {
        try
        {
            var response = await HttpClient.GetAsync($"{BasePath}/{id}");
            if (!response.IsSuccessStatusCode)
                return new ServiceResponseModel<T?>()
                {
                    Error = await response.Content.ReadAsStringAsync()
                };
            return new ServiceResponseModel<T?>()
            {
                Success = true,
                Item = await JsonSerializer.DeserializeAsync<T>(await response.Content.ReadAsStreamAsync())
            };
        }
        catch (Exception exception)
        {
            Logger.LogError(exception.Message);
            return new ServiceResponseModel<T?>()
            {
                Error = exception.Message
            };
        }
    }
    
    public virtual async Task<ServiceResponseModel<T?>> Post(T model)
    {
        try
        {
            var httpContent = new StringContent(JsonSerializer.Serialize(model), System.Text.Encoding.UTF8, "application/json");
            var response = await HttpClient.PostAsync($"{BasePath}", httpContent);
            if (!response.IsSuccessStatusCode)
                return new ServiceResponseModel<T?>()
                {
                    Error = await response.Content.ReadAsStringAsync()
                };
            return new ServiceResponseModel<T?>()
            {
                Success = true,
                Item = await JsonSerializer.DeserializeAsync<T>(await response.Content.ReadAsStreamAsync())
            };
        }
        catch (Exception exception)
        {
            Logger.LogError(exception.Message);
            return new ServiceResponseModel<T?>()
            {
                Error = exception.Message
            };
        }
    }
    
    public virtual async Task<ServiceResponseModel<T?>> Put(
        int id,
        T model)
    {
        try
        {
            var httpContent = new StringContent(JsonSerializer.Serialize(model), System.Text.Encoding.UTF8, "application/json");
            var response = await HttpClient.PutAsync($"{BasePath}", httpContent);
            if (!response.IsSuccessStatusCode)
                return new ServiceResponseModel<T?>()
                {
                    Error = await response.Content.ReadAsStringAsync()
                };
            return new ServiceResponseModel<T?>()
            {
                Success = true,
                Item = await JsonSerializer.DeserializeAsync<T>(await response.Content.ReadAsStreamAsync())
            };
        }
        catch (Exception exception)
        {
            Logger.LogError(exception.Message);
            return new ServiceResponseModel<T?>()
            {
                Error = exception.Message
            };
        }
    }
    
    public virtual async Task<bool> Delete(int id)
    {
        try
        {
            var response = await HttpClient.DeleteAsync($"{BasePath}/{id}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception exception)
        {
            Logger.LogError(exception.Message);
            return false;
        }
    }
}