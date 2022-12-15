#nullable enable
using System;
using UnityEngine;
using Component = UnityEngine.Component;

namespace Nyr.UnityDev.Util
{
    public class SafeGetComponentComponent : Component, ISafeGetComponent
    {
        protected T? SafeGetComponent<T>() where T : Component
        {
            return ((ISafeGetComponent)this).SafeGetComponent<T>();
        }
    }

    public class SafeComponentGetters : MonoBehaviour, ISafeGetComponent
    {
        protected T SafeGetComponent<T>() where T : Component => ((ISafeGetComponent)this).SafeGetComponent<T>();

        protected T? NullableGetComponent<T>() where T : Component =>
            ((ISafeGetComponent)this).NullableGetComponent<T>();

        protected T? SafeNullableGetComponent<T>() where T : Component =>
            ((ISafeGetComponent)this).SafeNullableGetComponent<T>();

        protected bool TryGetComponentInChildren<T>(out T component) where T : Component =>
            ((ISafeGetComponent)this).TryGetComponentInChildren(out component);
    }
}