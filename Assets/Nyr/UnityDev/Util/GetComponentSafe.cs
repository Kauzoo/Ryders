#nullable enable
using System;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Nyr.UnityDev.Util
{
    [UsedImplicitly]
    public static class GetComponentSafe
    {
        #region NullChecks
        [UsedImplicitly]
        public static T IsNotNull<T>(MonoBehaviour source, T component) where T : Object => component != null
            ? component
            : throw new MissingReferenceException($"@{source.gameObject.name}: Failed to find {typeof(T)}");

        [UsedImplicitly]
        public static T IsNotNull<T>(MonoBehaviour source, T component, Exception e) where T : Object =>
            component != null
                ? component
                : throw e;

        [UsedImplicitly]
        public static T IsNotNull<T>(MonoBehaviour source, T component, string message) where T : Object =>
            component != null
                ? component
                : throw new MissingReferenceException($"@{source.gameObject.name}: {message}");

        [UsedImplicitly]
        public static T IsNotNull<T>(MonoBehaviour source, T component, string message, object exceptionSource)
            where T : Object =>
            component != null
                ? component
                : throw new MissingReferenceException($"@{exceptionSource}: {message}");

        [UsedImplicitly]
        public static T IsNull<T>(MonoBehaviour source, T component) where T : Object => component == null
            ? throw new MissingReferenceException($"@{source.gameObject.name}: Failed to find {typeof(T)}")
            : component;

        [UsedImplicitly]
        public static T IsNull<T>(MonoBehaviour source, T component, Exception e) where T : Object => component == null
            ? throw e
            : component;

        [UsedImplicitly]
        public static T IsNull<T>(MonoBehaviour source, T component, string message) where T : Object =>
            component == null
                ? throw new MissingReferenceException($"@{source.gameObject.name}: {source}")
                : component;

        [UsedImplicitly]
        public static T IsNull<T>(MonoBehaviour source, T component, string message, object exceptionSource)
            where T : Object =>
            component == null
                ? throw new MissingReferenceException($"@{exceptionSource}: {source}")
                : component;

        [UsedImplicitly]
        public static T? IsNotNullCSharp<T>(T component) where T : Object => component != null ? component : null;

        [UsedImplicitly]
        public static T? IsNullCSharp<T>(T component) where T : Object => component == null ? null : component;
        #endregion

        #region ComponentTyped
        [UsedImplicitly]
        public static T SafeGetComponent<T>(this MonoBehaviour source) where T : Object =>
            source.TryGetComponent<T>(out var component)
                ? component
                : throw new MissingReferenceException($"@{source.gameObject.name}: Failed to find {typeof(T)}");
        
        [UsedImplicitly]
        public static T SafeGetComponent<T>(this MonoBehaviour source, Exception e) where T : Object =>
            source.TryGetComponent<T>(out var component)
                ? component
                : throw e;

        [UsedImplicitly]
        public static T SafeGetComponent<T>(this MonoBehaviour source, string message) where T : Object =>
            source.TryGetComponent<T>(out var component)
                ? component
                : throw new MissingReferenceException($"@{source.gameObject.name}: {message}");

        [UsedImplicitly]
        public static T SafeGetComponent<T>(this MonoBehaviour source, string message, object exceptionSource)
            where T : Object =>
            source.TryGetComponent<T>(out var component)
                ? component
                : throw new MissingReferenceException($"@{exceptionSource}: {message}");

        [UsedImplicitly]
        public static T? SafeGetComponentNullable<T>(this MonoBehaviour source) where T : Object =>
            source.TryGetComponent<T>(out var component)
                ? component
                : null;

        [UsedImplicitly]
        public static T SafeGetComponentInChildren<T>(this MonoBehaviour source) where T : Object =>
            IsNotNull(source, source.GetComponentInChildren<T>());

        [UsedImplicitly]
        public static T SafeGetComponentInChildren<T>(this MonoBehaviour source, Exception e) where T : Object =>
            IsNotNull(source, source.GetComponentInChildren<T>(), e);

        [UsedImplicitly]
        public static T SafeGetComponentInChildren<T>(this MonoBehaviour source, string message) where T : Object =>
            IsNotNull(source, source.GetComponentInChildren<T>(), message);

        [UsedImplicitly]
        public static T SafeGetComponentInChildren<T>(this MonoBehaviour source, string message, object exceptionSource)
            where T : Object =>
            IsNotNull(source, source.GetComponentInChildren<T>(), message, source);

        [UsedImplicitly]
        public static T? GetComponentInChildrenNullable<T>(this MonoBehaviour source) where T : Object =>
            IsNotNullCSharp(source.GetComponentInChildren<T>());

        [UsedImplicitly]
        public static T SafeGetComponentInParent<T>(this MonoBehaviour source) where T : Object =>
            IsNotNull(source, source.GetComponentInParent<T>());

        [UsedImplicitly]
        public static T SafeGetComponentInParent<T>(this MonoBehaviour source, Exception e) where T : Object =>
            IsNotNull(source, source.GetComponentInParent<T>(), e);

        [UsedImplicitly]
        public static T SafeGetComponentInParent<T>(this MonoBehaviour source, string message) where T : Object =>
            IsNotNull(source, source.GetComponentInParent<T>(), message);

        [UsedImplicitly]
        public static T SafeGetComponentInParent<T>(this MonoBehaviour source, string message, object exceptionSource)
            where T : Object =>
            IsNotNull(source, source.GetComponentInParent<T>(), message, exceptionSource);

        [UsedImplicitly]
        public static T? SafeGetComponentInParentNullable<T>(this MonoBehaviour source) where T : Object =>
            IsNotNullCSharp(source.GetComponentInParent<T>());
        #endregion

        #region Void
        /// <summary>
        /// Replaces target with a retrieved Component if possible 
        /// </summary>
        /// <param name="source">MonoBehaviour from which the Component should be retrieved</param>
        /// <param name="target">Component to be assigned</param>
        /// <typeparam name="T"></typeparam>
        [UsedImplicitly]
        public static void SafeGetComponent<T>(this MonoBehaviour source, ref T target) where T : Object
        {
            if (target != null) return;
            target = SafeGetComponent<T>(source);
        }
        
        [UsedImplicitly]
        public static void SafeGetComponentNullable<T>(this MonoBehaviour source, ref T? target) where T : Object
        {
            if (target != null) return;
            target = SafeGetComponentNullable<T>(source);
        }

        [UsedImplicitly]
        public static void SafeGetComponentInChildren<T>(this MonoBehaviour source, ref T target) where T : Object
        {
            if (target != null) return;
            target = IsNotNull(source, source.GetComponentInChildren<T>());
        }

        [UsedImplicitly]
        public static void SafeGetComponentInChildrenNullable<T>(this MonoBehaviour source, ref T? target) where T : Object
        {
            if (target != null) return;
            target = IsNotNullCSharp(source.GetComponentInChildren<T>());
        }

        [UsedImplicitly]
        public static void SafeGetComponentInParent<T>(this MonoBehaviour source, ref T target) where T : Object
        {
            if (target != null) return;
            target = IsNotNull(source, source.GetComponentInParent<T>());
        }

        [UsedImplicitly]
        public static void SafeGetComponentInParentNullable<T>(this MonoBehaviour source, ref T? target) where T : Object
        {
            if (target != null) return;
            target = IsNotNullCSharp<T>(source.GetComponentInParent<T>());
        }
        #endregion

        [UsedImplicitly]
        public static T2 SafeGetNullable<T1, T2>(this T1 item) where T1 : MonoBehaviour where T2 : Object =>
        item.TryGetComponent<T2>(out var component)
                ? component
                : throw new MissingReferenceException($"@{item.gameObject.name}: Failed to find {typeof(T2)}");
    }
}