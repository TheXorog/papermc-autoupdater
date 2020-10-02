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
            public string latest { get; set; }
            public List<string> all { get; set; }
        }
    }
}
