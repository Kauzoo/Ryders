using UnityEngine;
using Object = UnityEngine.Object;
using Component = UnityEngine.Component;

namespace Nyr.UnityDev.Util
{
    public interface ISafeDestroy
    {
        public static void SafeDestroy(ref Object obj, float t = 0.0f)
        {
            Object.Destroy(obj, t);
            obj = null;
        }
    }
}