using System;
using System.Collections.Generic;
using System.Text;

namespace Config.Lib.Models
{
    public class ConfigModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public string ApplicationName { get; set; }
        public bool IsActive { get; set; }
    }
}
