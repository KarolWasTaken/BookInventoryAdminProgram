using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookInventoryAdminProgram
{

    public class Settings
    {
        public string ConnectionString { get; set; }
        public Dictionary<string, bool> HeaderVisibilitiesSerialised { get; set; }
    }
}
