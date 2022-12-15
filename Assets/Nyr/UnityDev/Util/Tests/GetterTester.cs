using UnityEngine;
using static Nyr.UnityDev.Util.GetComponentSafe;

namespace Nyr.UnityDev.Util.Tests
{
    public class GetterTester : SafeComponentGetters
    {
        public Transform trans;
        public Collider coll;
        public Bounds bounds;

        [ContextMenu("GetTransform")]
        public void GetTransform()
        {
            // Inherited version
            coll = SafeNullableGetComponent<Collider>();
            bounds = (Bounds) coll?.bounds;

            // Standard unity way
            // trans = GetComponent<Transform>();
        }

        [ContextMenu("GetChildTransform")]
        public void GetChildTransform()
        {
            if (TryGetComponentInChildren<Collider>(out var transComp))
            {
                coll = transComp;
            }
        }

        [ContextMenu("ClearTransform")]
        public void ClearTransform()
        {
            trans = null;
        }
    }

    public class GetterTester2 : MonoBehaviour
    {
        public Transform trans;

        public void GetTransform()
        {
            // Extension method syntax
            trans = this.SafeGetComponent<Transform>();
        }
    }
}