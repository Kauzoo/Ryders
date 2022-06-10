using UnityEngine;

namespace Ryders.Core.Player.DefaultBehaviour.Telemetry
{
    public static class TelemetryHelper
    {
        // TODO Expand this
        public static string GetVector3Formated(Vector3 vec)
        {
            return "X: " + vec.x + " Y: " + vec.y + " Z: " + vec.z;
        }
    }
}