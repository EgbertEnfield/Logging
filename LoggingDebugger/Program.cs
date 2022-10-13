using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logging;

namespace LoggingDebugger {
    internal class Program {
        static void Main(string[] args) {
            var formatter = new Formatter() {
                Format = "{asctime} [{level}] {message}",
                DateFormat = "yy-MM-dd hh:mm:ss",
            };
            var streamHandler = new StreamHandler() {
                MinLevel = Level.Debug,
            };
            var rotatingFileHandler = new RotatingFileHandler("C:/Users/EosinY/Desktop/loglog", ".log", 1, RotatingFileHandler.SizeUnit.B) {
                MinLevel = Level.Debug,
            };
            var logger = new Logger(formatter, streamHandler, rotatingFileHandler);

            logger.Warn("foooooooooooooooooooooooooooooooooooooooo!");
            logger.Warn("fuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuck yeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeaaaaaaaaaaaaaaaaaaaaaah!");
            logger.Warn("shhhhhhhhhhhhhhhhhhhhhhhhhhhhhiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiittt");

            Console.WriteLine($"{rotatingFileHandler.LogFilePath} {File.Exists(rotatingFileHandler.LogFilePath)}");
            Console.Read();
        }
    }
}