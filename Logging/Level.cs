using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging {
    public enum Level {
        Debug,
        Info,
        Warn,
        Error,
        Exception,
        Critical,
        Disabled = 9999,
    }
}
