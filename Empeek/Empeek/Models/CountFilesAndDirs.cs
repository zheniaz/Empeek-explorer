using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Empeek.Models
{
    public class CountFilesAndDirs
    {
        public int Less10Mb { get; set; }
        public int More10Less50Mb { get; set; }
        public int More50Less100Mb { get; set; }
        public int More100Mb { get; set; }
        public string CurrentPath { get; set; }
    }
}