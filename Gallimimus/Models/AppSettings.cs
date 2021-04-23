using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gallimimus.Models
{
    class AppSettings
    {
        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
