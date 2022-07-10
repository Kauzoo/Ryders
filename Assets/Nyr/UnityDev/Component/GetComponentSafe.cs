#nullable enable
using System;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Nyr.UnityDev.Component
{
    [UsedImplicitly]
    public class GetComponentSafe
    {
        // TODO Add object support
        // TODO Add Log only support
        private readonly Exception? _standardException;
        private readonly string? _standardMessage;
        private readonly object? _standardMessageSource;
            
        public GetComponentSafe(Exception e) => _standardException = e;

        public GetComponentSafe(string? message) => _standardMessage = message;

        public GetComponentSafe(string? message, object? messageSource)
        {
            _standardMessage = message;
            _standardMessageSource = messageSource;
        }
        
        public static T IsNotNull<T>(T component, MonoBehaviour source) where T : Object => component != null
            ? component
            : throw new MissingReferenceException($"@{source.gameObject.name}: Failed to find {typeof(T)}");

        public static T IsNotNull<T>(T component, MonoBehaviour source, Exception e) where T : Object =>
            component != null
                ? component
                : throw e;

        public static T IsNotNull<T>(T component, MonoBehaviour source, string message) where T : Object =>
            component != null
                ? component
                : throw new MissingReferenceException($"@{source.gameObject.name}: {message}");

        public static T IsNotNull<T>(T component, MonoBehaviour source, string message, object exceptionSource)
            where T : Object =>
            component != null
                ? component
                : throw new MissingReferenceException($"@{exceptionSource}: {message}");

        public static T IsNull<T>(T component, MonoBehaviour source) where T : Object => component == null
            ? throw new MissingReferenceException($"@{source.gameObject.name}: Failed to find {typeof(T)}")
            : component;

        public static T IsNull<T>(T component, MonoBehaviour source, Exception e) where T : Object => component == null
            ? throw e
            : component;

        public static T IsNull<T>(T component, MonoBehaviour source, string message) where T : Object =>
            component == null
                ? throw new MissingReferenceException($"@{source.gameObject.name}: {source}")
                : component;

        public static T IsNull<T>(T component, MonoBehaviour source, string message, object exceptionSource)
            where T : Object =>
            component == null
                ? throw new MissingReferenceException($"@{exceptionSource}: {source}")
                : component;

        public static T? IsNotNullCSharp<T>(T component) where T : Object => component != null ? component : null;

        public static T? IsNullCSharp<T>(T component) where T : Object => component == null ? null : component;

        public static T GetComponent<T>(MonoBehaviour source) where T : Object =>
            source.TryGetComponent<T>(out var component)
                ? component
                : throw new MissingReferenceException($"@{source.gameObject.name}: Failed to find {typeof(T)}");
        
        public static T GetComponent<T>(MonoBehaviour source, Exception e) where T : Object =>
            source.TryGetComponent<T>(out var component)
                ? component
                : throw e;

        public static T GetComponent<T>(MonoBehaviour source, string message) where T : Object =>
            source.TryGetComponent<T>(out var component)
                ? component
                : throw new MissingReferenceException($"@{source.gameObject.name}: {message}");

        public static T GetComponent<T>(MonoBehaviour source, string message, object exceptionSource)
            where T : Object =>
            source.TryGetComponent<T>(out var component)
                ? component
                : throw new MissingReferenceException($"@{exceptionSource}: {message}");

        public static T? GetComponentNullable<T>(MonoBehaviour source) where T : Object =>
            source.TryGetComponent<T>(out var component)
                ? component
                : null;

        public static T GetComponentInChildren<T>(MonoBehaviour source) where T : Object =>
            IsNotNull(source.GetComponentInChildren<T>(), source);

        public static T GetComponentInChildren<T>(MonoBehaviour source, Exception e) where T : Object =>
            IsNotNull(source.GetComponentInChildren<T>(), source, e);

        public static T GetComponentInChildren<T>(MonoBehaviour source, string message) where T : Object =>
            IsNotNull(source.GetComponentInChildren<T>(), source, message);

        public static T GetComponentInChildren<T>(MonoBehaviour source, string message, object exceptionSource)
            where T : Object =>
            IsNotNull(source.GetComponentInChildren<T>(), source, message, source);

        public static T? GetComponentInChildrenNullable<T>(MonoBehaviour source) where T : Object =>
            IsNotNullCSharp(source.GetComponentInChildren<T>());

        public static T GetComponentInParent<T>(MonoBehaviour source) where T : Object =>
            IsNotNull(source.GetComponentInParent<T>(), source);

        public static T GetComponentInParent<T>(MonoBehaviour source, Exception e) where T : Object =>
            IsNotNull(source.GetComponentInParent<T>(), source, e);

        public static T GetComponentInParent<T>(MonoBehaviour source, string message) where T : Object =>
            IsNotNull(source.GetComponentInParent<T>(), source, message);

        public static T GetComponentInParent<T>(MonoBehaviour source, string message, object exceptionSource)
            where T : Object =>
            IsNotNull(source.GetComponentInParent<T>(), source, message, exceptionSource);

        public static T? GetComponentInParentNullable<T>(MonoBehaviour source) where T : Object =>
            IsNotNullCSharp(source.GetComponentInParent<T>());

        /// <summary>
        /// Replaces target with a retrieved Component if possible 
        /// </summary>
        /// <param name="source">MonoBehaviour from which the Component should be retrieved</param>
        /// <param name="target">Component to be assigned</param>
        /// <typeparam name="T"></typeparam>
        public static void SafeGetComponent<T>(MonoBehaviour source, ref T target) where T : Object
        {
            if (target != null) return;
            target = GetComponent<T>(source);
        }
        
        public static void GetComponentNullable<T>(MonoBehaviour source, ref T? target) where T : Object
        {
            if (target != null) return;
            target = GetComponentNullable<T>(source);
        }

        public static void GetComponentInChildren<T>(MonoBehaviour source, ref T target) where T : Object
        {
            if (target != null) return;
            target = IsNotNull(source.GetComponentInChildren<T>(), source);
        }

        public static void GetComponentInChildrenNullable<T>(MonoBehaviour source, ref T? target) where T : Object
        {
            if (target != null) return;
            target = IsNotNullCSharp(source.GetComponentInChildren<T>());
        }

        public static void GetComponentInParent<T>(MonoBehaviour source, ref T target) where T : Object
        {
            if (target != null) return;
            target = IsNotNull(source.GetComponentInParent<T>(), source);
        }

        public static void GetComponentInParentNullable<T>(MonoBehaviour source, ref T? target) where T : Object
        {
            if (target != null) return;
            target = IsNotNullCSharp<T>(source.GetComponentInParent<T>());
        }
    }
}