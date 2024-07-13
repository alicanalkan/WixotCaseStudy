using System;
using System.Collections.Generic;
using Wixot.AssetManagement;
using Wixot.AssetManagement.PoolManagement;

namespace Wixot.Engine
{
    public static class Singletons
    {
        public static readonly Dictionary<Type, string> TypeNameMap = new Dictionary<Type, string>
        {
            {typeof(AssetManager),"Asset Manager"},
            {typeof(PoolManager),"Pool Manager"},
        };
    }
}
