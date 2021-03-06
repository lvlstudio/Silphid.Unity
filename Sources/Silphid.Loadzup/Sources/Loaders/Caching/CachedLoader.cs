﻿using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Silphid.Loadzup.Caching
{
    public class CachedLoader : ILoader
    {
        private readonly Dictionary<Uri, Subject<object>> _currentlyLoading = new Dictionary<Uri, Subject<object>>();
        private readonly ILoader _innerLoader;
        private readonly Dictionary<Uri, object> _cache = new Dictionary<Uri, object>();

        public CachedLoader(ILoader innerLoader)
        {
            _innerLoader = innerLoader;
        }

        public bool Supports<T>(Uri uri) =>
            _innerLoader.Supports<T>(uri);

        public IObservable<T> Load<T>(Uri uri, Options options = null) =>
            LoadInternal<T>(uri, options).Select(GetInstance);

        private IObservable<T> LoadInternal<T>(Uri uri, Options options)
        {
            lock (this)
            {
                object obj;
                if (_cache.TryGetValue(uri, out obj))
                    return Observable.Return((T) obj);

                Subject<object> sub;
                if (_currentlyLoading.TryGetValue(uri, out sub))
                    return sub.OfType<object, T>();

                _currentlyLoading[uri] = new Subject<object>();
            }

            return _innerLoader
                .Load<T>(uri, options)
                .Do(x =>
                {
                    Subject<object> sub;

                    lock (this)
                    {
                        sub = _currentlyLoading[uri];
                        _cache[uri] = x;
                        _currentlyLoading.Remove(uri);
                    }

                    sub.OnNext(x);
                    sub.OnCompleted();
                })
                .DoOnError(x =>
                {
                    Subject<object> sub;

                    lock (this)
                    {
                        sub = _currentlyLoading[uri];
                        _currentlyLoading.Remove(uri);
                    }

                    sub.OnError(x);
                });
        }

        private T GetInstance<T>(T obj) =>
            obj is GameObject
                ? (T) (object) Object.Instantiate((GameObject) (object) obj)
                : obj;

        public void ClearCache()
        {
            lock (this)
            {
                _cache.Clear();
            }
        }

        protected void Remove(Uri uri)
        {
            lock (this)
            {
                _cache.Remove(uri);
            }
        }
    }
}