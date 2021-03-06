﻿using System;
using UniRx;
using UnityEngine;

namespace Silphid.Loadzup.Http
{
    public class HttpPoster : IPoster
    {
        private readonly IRequester _requester;
        private readonly IConverter _converter;

        public HttpPoster(IRequester requester, IConverter converter)
        {
            _requester = requester;
            _converter = converter;
        }

        public IObservable<T> Post<T>(Uri uri, WWWForm form, Options options = null) =>
            _requester
                .Post(uri, form, options)
                .ContinueWith(x => typeof(T) != typeof(Response)
                    ? _converter.Convert<T>(x.Bytes, options?.ContentType ?? x.ContentType, x.Encoding)
                    : Observable.Return((T) (object) x));
    }
}