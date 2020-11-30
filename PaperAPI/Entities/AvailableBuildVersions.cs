using System;
using System.Collections.Generic;
using System.Text;

namespace PaperAPI.Entities
{
    public class AvailableBuildVersions
    {
        public string project { get; set; }
        public string version { get; set; }
        public Builds builds { get; set; }

        public class Builds
        {
            public int latest { get; set; }
            public List<int> all { get; set; }
        }
    }
}
