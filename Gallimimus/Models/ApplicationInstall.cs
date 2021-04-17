using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gallimimus.Models
{
    class ApplicationInstall
    {
        [JsonProperty("applicationId")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("developer")]
        public string Developer { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("website")]
        public string Website { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("versionCount")]
        public int VersionCount { get; set; }

        [JsonProperty("versions")]
        public List<ApplicationVersion> Versions { get; set; }

        public string Status { get; set; }
    }
}
