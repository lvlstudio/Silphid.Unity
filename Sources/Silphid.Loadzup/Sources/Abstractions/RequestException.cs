﻿using System;
using System.Collections.Generic;
using System.Net;
using UniRx;

namespace Silphid.Loadzup
{
    public class RequestException : Exception
    {
        public string RawErrorMessage { get; }
        public bool HasResponse { get; }
        public string Text { get; }
        public HttpStatusCode StatusCode { get; }
        public Dictionary<string, string> ResponseHeaders { get; }

        public RequestException()
        {
        }

        public RequestException(HttpStatusCode statusCode)
        {
            RawErrorMessage = statusCode.ToString();
            Text = ((int) statusCode).ToString();
            StatusCode = statusCode;
        }

        public RequestException(WWWErrorException exception)
        {
            RawErrorMessage = exception.RawErrorMessage;
            HasResponse = exception.HasResponse;
            Text = exception.Text;
            StatusCode = exception.StatusCode;
            ResponseHeaders = exception.ResponseHeaders;
        }

        public override string ToString() => $"{RawErrorMessage} {Text}";
    }
}