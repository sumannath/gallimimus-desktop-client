using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gallimimus.Models
{
    class ApplicationList
    {
        [JsonProperty("applicationCount")]
        public int ApplicationCount { get; set; }

        [JsonProperty("applications")]
        public List<ApplicationInstall> Applications { get; set; }
    }
}
