using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EldoriaLauncher.MrPack
{
    public class File
    {

        public string[] downloads { get; set; }
        public int fileSize { get; set; }

        public Hashes hashes { get; set; }
        public string path { get; set; }
    }
}
