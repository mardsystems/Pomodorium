﻿using Newtonsoft.Json;
using Pomodorium.Utils;
using System.Text;

namespace Pomodorium.Support;

public class WebApiContext : IDisposable
{
    //private readonly AppHostingContext _appHostingContext;
    private readonly StringBuilder _log = new();

    private HttpClient _httpClient;

    public HttpClient HttpClient
    {
        get
        {
            _httpClient ??= AppHostingContext.CreateHttpClient();

            return _httpClient;
        }
    }

    //public WebApiContext(AppHostingContext appHostingContext)
    //{
    //    _appHostingContext = appHostingContext;
    //}

    public void Dispose()
    {
        GC.SuppressFinalize(this);

        if (_httpClient != null)
        {
            _httpClient.Dispose();
            _httpClient = null;
        }
    }

    public WebApiResponse<TData> ExecuteGet<TData>(string endpoint)
    {
        // execute request
        // (we need to use the same HttpClient otherwise the auth token cookie gets lost)
        var httpResponse = HttpClient.GetAsync(endpoint).Result;

        LogResponse(httpResponse);

        SanityCheck(httpResponse, 500);

        var responseMessage = GetResponseMessage(httpResponse);

        Console.WriteLine(responseMessage);

        var responseContent = ReadContent(httpResponse);

        TData responseData = default;

        if ((int)httpResponse.StatusCode >= 200 && (int)httpResponse.StatusCode < 300)
            responseData = typeof(TData) == typeof(string)
                ? (TData)(object)responseContent
                : JsonConvert.DeserializeObject<TData>(responseContent);

        return new WebApiResponse<TData>
        {
            StatusCode = httpResponse.StatusCode,
            ResponseMessage = responseMessage,
            ResponseData = responseData
        };
    }

    public WebApiResponse<TData> ExecutePost<TData>(string endpoint, object data)
    {
        return ExecuteSend<TData>(endpoint, data, HttpMethod.Post);
    }

    public WebApiResponse ExecutePost(string endpoint, object data)
    {
        return ExecuteSend<string>(endpoint, data, HttpMethod.Post);
    }

    public WebApiResponse ExecutePut(string endpoint, object data)
    {
        return ExecuteSend<string>(endpoint, data, HttpMethod.Put);
    }

    public WebApiResponse<TData> ExecuteDelete<TData>(string endpoint)
    {
        var httpResponse = HttpClient.DeleteAsync(endpoint).Result;

        LogResponse(httpResponse);

        SanityCheck(httpResponse, 500);

        var responseMessage = GetResponseMessage(httpResponse);

        Console.WriteLine(responseMessage);

        var responseContent = ReadContent(httpResponse);

        TData responseData = default;

        if ((int)httpResponse.StatusCode >= 200 && (int)httpResponse.StatusCode < 300)
            responseData = typeof(TData) == typeof(string)
                ? (TData)(object)responseContent
                : JsonConvert.DeserializeObject<TData>(responseContent);

        return new WebApiResponse<TData>
        {
            StatusCode = httpResponse.StatusCode,
            ResponseMessage = responseMessage,
            ResponseData = responseData
        };
    }

    private WebApiResponse<TData> ExecuteSend<TData>(string endpoint, object data, HttpMethod httpMethod)
    {
        var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

        var httpRequest = new HttpRequestMessage(httpMethod, endpoint)
        {
            Content = content
        };

        var httpResponse = HttpClient.SendAsync(httpRequest).Result;

        LogResponse(httpResponse);

        // for post requests the 2xx, 3xx and 4xx status codes are all "valid" results
        SanityCheck(httpResponse, 500);

        var responseMessage = GetResponseMessage(httpResponse);

        Console.WriteLine(responseMessage);

        var responseContent = ReadContent(httpResponse);

        TData responseData = default;

        if ((int)httpResponse.StatusCode >= 200 && (int)httpResponse.StatusCode < 300)
            responseData = typeof(TData) == typeof(string)
                ? (TData)(object)responseContent
                : JsonConvert.DeserializeObject<TData>(responseContent);

        return new WebApiResponse<TData>
        {
            StatusCode = httpResponse.StatusCode,
            ResponseMessage = responseMessage,
            ResponseData = responseData
        };
    }

    private static string ReadContent(HttpResponseMessage response)
    {
        try
        {
            return response.Content.ReadAsStringAsync().Result;
        }
        catch
        {
            return null;
        }
    }

    // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
    private static void SanityCheck(HttpResponseMessage response, int upperRange = 300)
    {
        if ((int)response.StatusCode < 200 || (int)response.StatusCode >= upperRange)
        {
            var responseMessage = GetResponseMessage(response);
            throw new HttpResponseException(response.StatusCode, responseMessage,
                $"The Web API request should be completed with success, not with error '{responseMessage}'");
        }
    }

    private static string GetResponseMessage(HttpResponseMessage response)
    {
        if (response == null)
            return null;

        var content = ReadContent(response);
        return $"{response.StatusCode}: {content ?? response.ReasonPhrase}";
    }

    private void LogResponse(HttpResponseMessage response, string content = null)
    {
        _log.AppendLine(response.RequestMessage.RequestUri.ToString());
        _log.AppendLine($"{response.StatusCode}: {response.ReasonPhrase}");
        content ??= ReadContent(response);
        if (content != null)
            _log.AppendLine(content);
        _log.AppendLine();
    }

    public void SaveLog(string outputFolder, string fileName)
    {
        var logFilePath = Path.Combine(outputFolder, fileName);
        Console.WriteLine($"Saving log to {logFilePath}");
        File.WriteAllText(logFilePath, _log.ToString());
    }
}
