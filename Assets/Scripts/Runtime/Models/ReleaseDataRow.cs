using Newtonsoft.Json;

namespace Runtime.Models
{
    /// <summary>
    /// Model of row in <see cref="ReleaseData"/> for release.
    /// </summary>
    public sealed class ReleaseDataRow
    {
        /// <summary>
        /// Checkup date.
        /// </summary>
        [JsonProperty("date")]
        public string Date { get; set; }

        /// <summary>
        /// Plan number of tasks at the date.
        /// </summary>
        [JsonProperty("plan")]
        public int Plan { get; set; }

        /// <summary>
        /// Fact number of tasks at the date.
        /// </summary>
        [JsonProperty("actual")]
        public int Fact { get; set; }

        /// <summary>
        /// Percentage of deviation of actual from plan.
        /// </summary>
        public float GetDeviationPercent(int totalPlannedTasks)
        {
            if (totalPlannedTasks == 0)
                return 0;


            var planPercentage = (1f * Plan / totalPlannedTasks) * 100f;
            var factPercentage = (1f * Fact / totalPlannedTasks) * 100f;
            return factPercentage - planPercentage;
        }
    }
}