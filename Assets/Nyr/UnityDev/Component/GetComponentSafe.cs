#nullable enable
using System;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Nyr.UnityDev.Component
{
    [UsedImplicitly]
    public static class GetComponentSafe
    {
        #region NullChecks
        public static T IsNotNull<T>(MonoBehaviour source, T component) where T : Object => component != null
            ? component
            : throw new MissingReferenceException($"@{source.gameObject.name}: Failed to find {typeof(T)}");

        public static T IsNotNull<T>(MonoBehaviour source, T component, Exception e) where T : Object =>
            component != null
                ? component
                : throw e;

        public static T IsNotNull<T>(MonoBehaviour source, T component, string message) where T : Object =>
            component != null
                ? component
                : throw new MissingReferenceException($"@{source.gameObject.name}: {message}");

        public static T IsNotNull<T>(MonoBehaviour source, T component, string message, object exceptionSource)
            where T : Object =>
            component != null
                ? component
                : throw new MissingReferenceException($"@{exceptionSource}: {message}");

        public static T IsNull<T>(MonoBehaviour source, T component) where T : Object => component == null
            ? throw new MissingReferenceException($"@{source.gameObject.name}: Failed to find {typeof(T)}")
            : component;

        public static T IsNull<T>(MonoBehaviour source, T component, Exception e) where T : Object => component == null
            ? throw e
            : component;

        public static T IsNull<T>(MonoBehaviour source, T component, string message) where T : Object =>
            component == null
                ? throw new MissingReferenceException($"@{source.gameObject.name}: {source}")
                : component;

        public static T IsNull<T>(MonoBehaviour source, T component, string message, object exceptionSource)
            where T : Object =>
            component == null
                ? throw new MissingReferenceException($"@{exceptionSource}: {source}")
                : component;

        public static T? IsNotNullCSharp<T>(T component) where T : Object => component != null ? component : null;

        public static T? IsNullCSharp<T>(T component) where T : Object => component == null ? null : component;
        #endregion

        #region ComponentTyped
        public static T SafeGetComponent<T>(this MonoBehaviour source) where T : Object =>
            source.TryGetComponent<T>(out var component)
                ? component
                : throw new MissingReferenceException($"@{source.gameObject.name}: Failed to find {typeof(T)}");
        
        public static T SafeGetComponent<T>(this MonoBehaviour source, Exception e) where T : Object =>
            source.TryGetComponent<T>(out var component)
                ? component
                : throw e;

        public static T SafeGetComponent<T>(this MonoBehaviour source, string message) where T : Object =>
            source.TryGetComponent<T>(out var component)
                ? component
                : throw new MissingReferenceException($"@{source.gameObject.name}: {message}");

        public static T SafeGetComponent<T>(this MonoBehaviour source, string message, object exceptionSource)
            where T : Object =>
            source.TryGetComponent<T>(out var component)
                ? component
                : throw new MissingReferenceException($"@{exceptionSource}: {message}");

        public static T? SafeGetComponentNullable<T>(this MonoBehaviour source) where T : Object =>
            source.TryGetComponent<T>(out var component)
                ? component
                : null;

        public static T SafeGetComponentInChildren<T>(this MonoBehaviour source) where T : Object =>
            IsNotNull(source, source.GetComponentInChildren<T>());

        public static T SafeGetComponentInChildren<T>(this MonoBehaviour source, Exception e) where T : Object =>
            IsNotNull(source, source.GetComponentInChildren<T>(), e);

        public static T SafeGetComponentInChildren<T>(this MonoBehaviour source, string message) where T : Object =>
            IsNotNull(source, source.GetComponentInChildren<T>(), message);

        public static T SafeGetComponentInChildren<T>(this MonoBehaviour source, string message, object exceptionSource)
            where T : Object =>
            IsNotNull(source, source.GetComponentInChildren<T>(), message, source);

        public static T? GetComponentInChildrenNullable<T>(this MonoBehaviour source) where T : Object =>
            IsNotNullCSharp(source.GetComponentInChildren<T>());

        public static T SafeGetComponentInParent<T>(this MonoBehaviour source) where T : Object =>
            IsNotNull(source, source.GetComponentInParent<T>());

        public static T SafeGetComponentInParent<T>(this MonoBehaviour source, Exception e) where T : Object =>
            IsNotNull(source, source.GetComponentInParent<T>(), e);

        public static T SafeGetComponentInParent<T>(this MonoBehaviour source, string message) where T : Object =>
            IsNotNull(source, source.GetComponentInParent<T>(), message);

        public static T SafeGetComponentInParent<T>(this MonoBehaviour source, string message, object exceptionSource)
            where T : Object =>
            IsNotNull(source, source.GetComponentInParent<T>(), message, exceptionSource);

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
        public static void SafeGetComponent<T>(this MonoBehaviour source, ref T target) where T : Object
        {
            if (target != null) return;
            target = SafeGetComponent<T>(source);
        }
        
        public static void SafeGetComponentNullable<T>(this MonoBehaviour source, ref T? target) where T : Object
        {
            if (target != null) return;
            target = SafeGetComponentNullable<T>(source);
        }

        public static void SafeGetComponentInChildren<T>(this MonoBehaviour source, ref T target) where T : Object
        {
            if (target != null) return;
            target = IsNotNull(source, source.GetComponentInChildren<T>());
        }

        public static void SafeGetComponentInChildrenNullable<T>(this MonoBehaviour source, ref T? target) where T : Object
        {
            if (target != null) return;
            target = IsNotNullCSharp(source.GetComponentInChildren<T>());
        }

        public static void SafeGetComponentInParent<T>(this MonoBehaviour source, ref T target) where T : Object
        {
            if (target != null) return;
            target = IsNotNull(source, source.GetComponentInParent<T>());
        }

        public static void SafeGetComponentInParentNullable<T>(this MonoBehaviour source, ref T? target) where T : Object
        {
            if (target != null) return;
            target = IsNotNullCSharp<T>(source.GetComponentInParent<T>());
        }
        #endregion
    }
}