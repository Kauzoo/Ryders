using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Nyr.UnityDev
{
    public static class GetComponentSafe
    {
        public static T GetComponent<T>(MonoBehaviour source) where T : Object
        {
            return source.TryGetComponent<T>(out var component)
                ? component
                : throw new MissingReferenceException($"@{source.gameObject.name}: Failed to find {typeof(T)}");
        }

        /// <summary>
        /// Replaces target with a retrieved Component if possible 
        /// </summary>
        /// <param name="source">MonoBehaviour from which the Component should be retrieved</param>
        /// <param name="target">Component to be assigned</param>
        /// <typeparam name="T"></typeparam>
        public static void GetComponent<T>(MonoBehaviour source, ref T target) where T : Object
        {
            if (target == null)
                target = GetComponent<T>(source);
        }
    }
}