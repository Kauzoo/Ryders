namespace Ryders.Core
{
    public static class Formula
    {
        public const float SpeedToRidersSpeedRatio = 216.0f;

        /// <summary>
        /// Converts from the Speedometer units used by default in Ryders to Sonic Riders original in game units
        /// </summary>
        /// <param name="speed">The speed to convert to "in game units"</param>
        /// <returns></returns>
        public static float SpeedToRidersSpeed(float speed) => speed / SpeedToRidersSpeedRatio;

        /// <summary>
        /// Converts from Sonic Riders original in game units to Ryders units (Speedometer)
        /// </summary>
        /// <param name="speed">The speed to convert to Ryders units (Speedometer)</param>
        /// <returns></returns>
        public static float RidersSpeedToSpeed(float speed) => speed * SpeedToRidersSpeedRatio;
    }
}