using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging {
    public class StreamHandler {
        public bool IsEnable { get; set; }
        public Level MinLevel { get; set; } = Level.Disabled;

        public void StreamConsole(string message, ConsoleColor color) {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
