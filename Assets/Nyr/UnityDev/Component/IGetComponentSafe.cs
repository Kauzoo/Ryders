#nullable enable
using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Nyr.UnityDev.Component
{
    public interface IGetComponentSafe
    {
        public T? GetComponent<T>() where T : Object
        {
            var source = (this as MonoBehaviour) ??
                         throw new InvalidCastException($"{this} could not be cast to MonoBehaviour");
            return source.TryGetComponent(out T component) ? component : null;
        }

        public T? GetComponentInParent<T>() where T : Object
        {
            var ret = (this as MonoBehaviour)?.GetComponentInParent<T>();
            return ret != null ? ret : null;
        }

        public T? GetComponentInChildren<T>() where T : Object
        {
            var ret = (this as MonoBehaviour)?.GetComponentInChildren<T>();
            return ret != null ? ret : null;
        }

        public T[]? GetComponentsInParent<T>() where T : Object
        {
            var ret = (this as MonoBehaviour)?.GetComponentsInParent<T>();
            return ret != null ? ret : null;
        }
        
        public T[]? GetComponentsInChildren<T>() where T : Object
        {
            var ret = (this as MonoBehaviour)?.GetComponentsInChildren<T>();
            return ret != null ? ret : null;
        }
    }
}