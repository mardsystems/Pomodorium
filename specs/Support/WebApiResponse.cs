using Pomodorium.Util;
using System.Net;

namespace Pomodorium.Support;

public class WebApiResponse
{
    public HttpStatusCode StatusCode { get; set; }

    public string ResponseMessage { get; set; }

    public void ShouldStatusBe(HttpStatusCode expectedStatusCode, string message = null)
    {
        if (StatusCode != expectedStatusCode)
        {
            if (message != null)
            {
                message = ", because " + message;
            }

            message = $"The Web API request expected to respond with {expectedStatusCode} ({(int)expectedStatusCode}), but responded with {StatusCode} ({(int)StatusCode}): '{ResponseMessage}'{message}.";

            throw new HttpResponseException(StatusCode, ResponseMessage, message);
        }
    }
}

public class WebApiResponse<TData> : WebApiResponse
{
    public TData ResponseData { get; set; }
}
