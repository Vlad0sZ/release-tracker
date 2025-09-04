using System;
using Newtonsoft.Json;
using Runtime.Interfaces.IO;

namespace Runtime.Models
{
    /// <summary>
    /// Model for release information.
    /// </summary>
    public sealed class ReleaseInfo : IPathable
    {
        /// <summary>
        /// Id for release date
        /// </summary>
        [JsonProperty("guid")]
        public string Id { get; set; } = Guid.NewGuid().ToString()[..8];

        /// <summary>
        /// Name of release.
        /// </summary>
        ///
        [JsonProperty("name")]
        public string Name { get; set; }


        /// <summary>
        /// Start release at this date.
        /// </summary>
        [JsonProperty("start_date")]
        public string StartDate { get; set; }

        /// <summary>
        /// Planned release end date.
        /// </summary>
        [JsonProperty("end_date")]
        public string EndDate { get; set; }


        /// <summary>
        /// Number of tasks in release.
        /// </summary>
        [JsonProperty("tasks_count")]
        public int TotalTasks { get; set; }


        /// <summary>
        /// Number of day, when need to check tasks.
        /// </summary>
        [JsonProperty("check_day")]
        public int CheckIn { get; set; }

        /// <summary>
        /// Table data.
        /// </summary>
        [JsonProperty("table")]
        public ReleaseDataRow[] Table { get; set; }

        public override string ToString() =>
            Newtonsoft.Json.JsonConvert.SerializeObject(this);

        [JsonIgnore] public string Extension => "json";

        [JsonIgnore] public string FileName => $"r-{Id}";
    }
}