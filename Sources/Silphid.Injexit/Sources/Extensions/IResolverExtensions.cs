﻿using System;

namespace Silphid.Injexit
{
    public static class IResolverExtensions
    {
        public static IResolver Using(this IResolver This, Action<IBinder> bind)
        {
            var overrideContainer = This.Create();
            bind(overrideContainer);
            return new CompositeResolver(overrideContainer, This);
        }

        public static IResolver Using(this IResolver This, IResolver overrideResolver) =>
            overrideResolver != null
                ? new CompositeResolver(overrideResolver, This)
                : This;
        
        public static IResolver UsingInstance<T>(this IResolver This, T instance) =>
            This.Using(x => x.BindInstance(instance));
        
        public static IResolver UsingInstances<T1, T2>(this IResolver This, T1 instance1, T2 instance2) =>
            This.Using(x =>
            {
                x.BindInstance(instance1);
                x.BindInstance(instance2);
            });
        
        public static IResolver UsingInstances<T1, T2, T3>(this IResolver This, T1 instance1, T2 instance2, T3 instance3) =>
            This.Using(x =>
            {
                x.BindInstance(instance1);
                x.BindInstance(instance2);
                x.BindInstance(instance3);
            });

        public static object Resolve(this IResolver This, Type abstractionType, string id = null) =>
            This.ResolveFactory(abstractionType, id).Invoke(This);

        public static T Resolve<T>(this IResolver This, string id = null) =>
            (T) This.Resolve(typeof(T), id);
    }
}