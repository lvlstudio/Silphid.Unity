﻿using System;

namespace Silphid.Loadzup.Bundles
{
    public interface IBundle
    {
        IObservable<T> LoadAsset<T>(string assetName);
        void Unload();
    }
}