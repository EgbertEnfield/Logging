using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Logging.NetCore {
    internal class LoggingJsonHandler {
        internal void LoadJson(string path) {
            using (var sr = new StreamReader(path)) {
                string rawJson = sr.ReadToEnd();
                
            }            
        }
    }
}
