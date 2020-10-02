using System;
using System.Collections.Generic;
using System.Text;

namespace PaperAPI.Entities
{
    public class AvailableMCVersions
    {
        public string project { get; set; }
        public List<string> versions { get; set; }
    }
}
