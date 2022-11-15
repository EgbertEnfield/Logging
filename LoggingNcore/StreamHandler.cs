using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging.NetCore{
    /// <summary>
    /// ログ内容をコンソールに表示させるやつ
    /// </summary>
    public class StreamHandler {
        public enum StreamType {
            StdIO,
            StdErr
        }
        public bool IsEnable { get; set; }
        public Level MinLevel { get; set; } = Level.Disabled;

        public void StreamConsole(string message, ConsoleColor color, StreamType type = StreamType.StdIO) {
            Console.ForegroundColor = color;
            if (type == StreamType.StdIO) {
                Console.WriteLine(message);
            }
            else if (type == StreamType.StdErr) {
                Console.Error.WriteLine(message);
            }
            Console.ResetColor();
        }
    }
}
