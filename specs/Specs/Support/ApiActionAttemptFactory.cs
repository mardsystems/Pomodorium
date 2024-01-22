﻿using System.Net;

namespace Pomodorium.Support;

public static class ApiActionAttemptFactory
{
    public static ActionAttempt<TInput, TResult> CreateWithStatusCheck<TInput, TResult>(
        this ActionAttemptFactory factory,
        string name,
        Func<TInput, WebApiResponse<TResult>> action,
        HttpStatusCode expectedStatusCode = HttpStatusCode.OK)
    {
        return factory.Create<TInput, TResult>(
            name,
            input =>
            {
                var response = action(input);

                response.ShouldStatusBe(expectedStatusCode);

                return response.ResponseData;
            });
    }
}