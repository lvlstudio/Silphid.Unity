﻿using System;
using System.Collections.Generic;

namespace Silphid.Machina
{
    public class StateInfo : IStateConfig
    {
        public readonly Dictionary<Type, Func<object, bool>> Handlers = new Dictionary<Type, Func<object, bool>>();
        
        /// <summary>
        /// Adds an handler for given trigger type T, which should return whether it handled the trigger or not.
        /// </summary>
        public void On<T>(Func<T, bool> handler)
        {
            Handlers[typeof(T)] = x => handler((T) x);
        }

        /// <summary>
        /// Adds an handler for given trigger type T, which is assumed to always handle the trigger.
        /// </summary>
        public void On<T>(Action<T> handler)
        {
            Handlers[typeof(T)] = x =>
            {
                handler((T) x);
                return true;
            };
        }
    }
}