using System;
using System.Collections.Generic;
using Wixot.AssetManagement;
using Wixot.AssetManagement.PoolManagement;
using Wixot.UI;

namespace Wixot.Engine
{
    public static class Singletons
    {
        public static readonly Dictionary<Type, string> TypeNameMap = new Dictionary<Type, string>
        {
            {typeof(AssetManager),"Asset Manager"},
            {typeof(PoolManager),"Pool Manager"},
            {typeof(GameManager), "Game Manager"},
            {typeof(UIManager), "UI Manager"}
        };
    }
}
