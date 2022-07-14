using UnityEngine;

namespace Ryders.Core.Player.DefaultBehaviour
{
    public abstract partial class PlayerBehaviour
    {
        [System.Serializable]
        public class PbDebug
        {
            [Header("Toggles")] public bool printTransformTelemetry;
            public bool printRigidbodyTelemetry;
            public bool printMovementTelemetry;
            [Header("TelemetryText")] public TMPro.TextMeshProUGUI playerTransformTelemetry;
            public TMPro.TextMeshProUGUI playerRigidbodyTelemetry;
            public TMPro.TextMeshProUGUI playerMovementTelemetry;
            [HideInInspector] public Canvas debugCanvas;
        }
    }
}