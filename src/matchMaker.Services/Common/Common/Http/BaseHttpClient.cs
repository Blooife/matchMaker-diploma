using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;

namespace Common.Http;

public class BaseHttpClient
{
    private readonly IHttpClientFactory _httpFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly string _baseUrl;

    /// <summary>
    /// Конструктор
    /// </summary>
    protected BaseHttpClient(
        IHttpClientFactory httpFactory,
        IHttpContextAccessor httpContextAccessor,
        string baseUrl)
    {
        _httpFactory = httpFactory;
        _httpContextAccessor = httpContextAccessor;
        _baseUrl = baseUrl;
    }

    /// <summary>
    /// Создание инстанса HttpClient
    /// </summary>
    protected HttpClient CreateHttpClient()
    {
        var httpClient = _httpFactory.CreateClient();
        httpClient.BaseAddress = new Uri(_baseUrl);
        SetAccessToken(httpClient);
        return httpClient;
    }
    
    /// <summary>
    /// Выполнить GET-запрос и получить десериализованный ответ
    /// </summary>
    protected async Task<TResult?> GetData<TResult>(string url)
    {
        var httpClient = CreateHttpClient();
        var response = await httpClient.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            return await CreateResponse<TResult>(response);
        }

        return default;
    }

    /// <summary>
    /// Отправить Post с данными в теле запроса без ответа
    /// </summary>
    protected async Task PostData<TParam>(TParam data, string url)
    {
        var httpClient = CreateHttpClient();
        Func<Task<HttpResponseMessage>> httpMethod = async () => await httpClient.PostAsJsonAsync(url, data);
        await SendDataByHttpMethod(httpMethod);
    }

    /// <summary>
    /// Отправить Post с данными в теле запроса с ответом
    /// </summary>
    protected async Task<TResult> PostData<TParam, TResult>(TParam data, string url)
    {
        var httpClient = CreateHttpClient();
        Func<Task<HttpResponseMessage>> httpMethod = async () => await httpClient.PostAsJsonAsync(url, data);
        return await SendDataByHttpMethod<TResult>(httpMethod);
    }

    /// <summary>
    /// Отправить пустой Post с ответом
    /// </summary>
    protected async Task<TResult> PostData<TResult>(string url)
    {
        return await PostData<object, TResult>(null, url);
    }

    /// <summary>
    /// Отправить пустой Post
    /// </summary>
    protected async Task PostData(string url)
    {
        await PostData<object>(null, url);
    }

    /// <summary>
    /// Отправить данные с использованием Http Post метода и обработкой ошибок, без данных в response
    /// </summary>
    private async Task SendDataByHttpMethod(Func<Task<HttpResponseMessage>> httpMethod)
    {
        HttpResponseMessage response = await httpMethod();

        if (response.IsSuccessStatusCode)
        {
            return;
        }
    }

    /// <summary>
    /// Отправить данные с использованием Http Post метода и обработкой ошибок, с данными в response
    /// </summary>
    private async Task<TResult> SendDataByHttpMethod<TResult>(Func<Task<HttpResponseMessage>> httpMethod)
    {
        var response = await httpMethod();
        if (response.IsSuccessStatusCode)
        {
            TResult result = await CreateResponse<TResult>(response);
            return result;
        }
        return default;
    }

    /// <summary>
    /// Получить десериализванный ответ запроса
    /// </summary>
    protected async Task<TValue> CreateResponse<TValue>(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        TValue value;

        var settings = new JsonSerializerSettings { MaxDepth = 128 };

        if (typeof(TValue) == typeof(string))
        {
            value = GetRawBody<TValue>(content);
        }
        else
        {
            value = JsonConvert.DeserializeObject<TValue>(content, settings);
        }

        return value;
    }

    protected TValue GetRawBody<TValue>(string content)
    {
        return (TValue)Convert.ChangeType(content, typeof(string));
    }

    private void SetAccessToken(HttpClient httpClient)
    {
        var bearerToken = _httpContextAccessor.HttpContext?.Request.Headers[HeaderNames.Authorization].ToString();
        
        if (!string.IsNullOrEmpty(bearerToken))
        {
            httpClient.DefaultRequestHeaders.Add(HeaderNames.Authorization, [bearerToken]);
        }
    }
}