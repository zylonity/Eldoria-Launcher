using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EldoriaLauncher.MrPack
{
    public class Dependencies
    {
        [JsonPropertyName("fabric-loader")]
        public string fabric_loader { get; set; }

        public string minecraft { get; set; }

    }
}
