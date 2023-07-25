using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteralLifeChurch.ArchiveManagerApi.DI.Forwarders
{
    internal class EnvironmentForwarder
    {
        public string GetEnvironmentVariable(string name)
        {
            return Environment.GetEnvironmentVariable(name);
        }  
    }
}
