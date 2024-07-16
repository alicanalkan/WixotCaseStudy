using UnityEngine;

namespace Wixot.UI.Utils
{
    public static class UIUtils
    {
        /// <summary>
        /// Gets a component of the given type attached to the GameObject. If that type of component does not exist, it adds one.
        /// </summary>
        /// <remarks>
        /// This method is useful when you don't know if a GameObject has a specific type of component,
        /// </remarks>
        /// <typeparam name="T"></typeparam>
        /// <param name="gameObject"></param>
        public static T GetOrAdd<T> (this GameObject gameObject) where T : Component
        {
            T component = gameObject.GetComponent<T>();
            if (!component) component = gameObject.AddComponent<T>();
            
            return component;
        }
    }
}