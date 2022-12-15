#nullable enable
using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;
using Component = UnityEngine.Component;

/**
 * NOTES:
 * Look into safer null checks using implicit null property
 * https://jacx.net/2015/11/20/dont-use-equals-null-on-unity-objects.html
 * Probably not necessary due to Generic type restriction
 */

namespace Nyr.UnityDev.Util
{
    public interface ISafeGetComponent
    {
        #region NullChecks

        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="component"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="MissingReferenceException"></exception>
        [UsedImplicitly]
        public static T IsNull<T>(Component source, T component) where T : Object => component
            ? component
            : throw new MissingReferenceException($"@{source.name}: Failed to find {typeof(T)}");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="component"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="MissingReferenceException"></exception>
        [UsedImplicitly]
        public static T IsNull<T>(T component) where T : Object => component
            ? component
            : throw new MissingReferenceException($"Failed to find {typeof(T)}");

        /// <summary>
        /// Check if a <see cref="UnityEngine.Object"/> is not Unity null. Unity null is converted to C# null.
        /// </summary>
        /// <param name="obj">Derived from <see cref="UnityEngine.Object"/></param>
        /// <typeparam name="T">Derived from <see cref="UnityEngine.Object"/></typeparam>
        /// <returns>Object of Type T or C# null</returns>
        [UsedImplicitly]
        public static T? IsNotNullCSharp<T>(T obj) where T : Object => obj ? obj : null;

        /// <summary>
        /// Check if a <see cref="UnityEngine.Object"/> is Unity null. Unity null is converted to C# null.
        /// </summary>
        /// <param name="obj">Derived from <see cref="UnityEngine.Object"/></param>
        /// <typeparam name="T">Derived from <see cref="UnityEngine.Object"/></typeparam>
        /// <returns>Object of Type T or C# null</returns>
        [UsedImplicitly]
        public static T? IsNullCSharp<T>(T obj) where T : Object => obj ? obj : null;

        #endregion

        #region GetComponent

        public T SafeGetComponent<T>() where T : Component =>
            ((Component)this).TryGetComponent<T>(out var component)
                ? component
                : throw new MissingComponentException(
                    $"@{(this as Component).name}: Failed to Get Component {typeof(T)}");

        public T? NullableGetComponent<T>() where T : Component =>
            ((Component)this).TryGetComponent<T>(out var component) ? component : null;

        public T? SafeNullableGetComponent<T>() where T : Component
        {
            var source = (this as Component);
            if (source == null) return null;
            return source.TryGetComponent<T>(out var component) ? component : null;
        }

        public void SafeGetComponent<T>(ref T target) where T : Component
        {
            if (target == null) return;
            target = SafeGetComponent<T>();
        }

        public void SafeSwapComponent<T>(ref T target) where T : Component => target = SafeGetComponent<T>();

        public void NullableGetComponent<T>(ref T? target) where T : Component => target = NullableGetComponent<T>();

        public void SafeNullableGetComponent<T>(ref T? target) where T : Component =>
            target = SafeNullableGetComponent<T>();

        #endregion

        #region GetComponentInChildren

        public T SafeGetComponentInChildren<T>() where T : Component =>
            IsNull((Component)this, ((Component)this).GetComponentInChildren<T>());

        public bool TryGetComponentInChildren<T>(out T component) where T : Component =>
            component = ((Component)this).GetComponentInChildren<T>();

        public void NullableGetComponentInChildren<T>(ref T? target) where T : Component =>
            target = NullableGetComponent<T>();

        #endregion
    }
}