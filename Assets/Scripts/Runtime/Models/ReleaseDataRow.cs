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
        [JsonIgnore]
        public float DeviationPercent => Plan == 0 ? 0 : (Fact - Plan) / (float) Plan * 100;
    }
}