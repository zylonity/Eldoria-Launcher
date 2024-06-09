using Modrinth.Models.Enums.Project;
using Modrinth.Models.Enums.Version;
using Modrinth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EldoriaLauncher.MrPack
{
    public class ModIndex
    {
        public Dependencies dependencies { get; set; }
        public File[] files { get; set; }
        public string versionId { get; set; }
    }
}
