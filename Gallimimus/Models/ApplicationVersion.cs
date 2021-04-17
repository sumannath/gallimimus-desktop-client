using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gallimimus.Models
{
    class ApplicationVersion
    {
        [JsonProperty("platform")]
        public string Platform { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("releaseDate")]
        public DateTime ReleaseDate { get; set; }

        [JsonProperty("architechture")]
        public string Architechture { get; set; }

        [JsonProperty("downloadLink")]
        public string DownloadLink { get; set; }

        [JsonProperty("silentInstallArgs")]
        public string SilentInstallArgs { get; set; }
    }
}
