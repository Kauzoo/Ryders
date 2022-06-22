namespace Ryders.Core
{
    public static class Formula
    {
        private const float SpeedToRidersSpeedRatio = 216.0f;

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

        /// <summary>
        /// Returns conversions ratio from in game units to Speedometer Units. This conversion ratio is identical to
        /// the original game (216.0f). In Ryders usually the Speedometer units will be used for everything except
        /// the final position update, unless specifically mentioned otherwise.
        /// Speedometer units are always bigger than in game units.
        /// </summary>
        /// <returns></returns>
        public static float GetSpeedToRidersSpeedRatio() => SpeedToRidersSpeedRatio;
    }
}