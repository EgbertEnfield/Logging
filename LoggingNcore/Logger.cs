using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggingNcore {
    public class Logger {
        private Formatter formatter;
        private FileHandler fileHandler = new FileHandler();
        private StreamHandler streamHandler = new StreamHandler();
        private RotatingFileHandler rotatingFileHandler = new RotatingFileHandler();

        /// <summary>
        /// ロガーを設定する
        /// </summary>
        /// <param name="formatter">ログのフォーマット</param>
        /// <param name="handlers">使いたい機能のインスタンスを入れる</param>
        public Logger(Formatter formatter, params object[] handlers) {
            this.formatter = formatter;
            for (int i = 0; i < handlers.Length; i++) {
                if (handlers[i].GetType().Name == typeof(FileHandler).Name) {
                    fileHandler = (FileHandler)handlers[i];
                }
                else if (handlers[i].GetType().Name == typeof(StreamHandler).Name) {
                    streamHandler = (StreamHandler)handlers[i];
                }
                else if (handlers[i].GetType().Name == typeof(RotatingFileHandler).Name) {
                    rotatingFileHandler = (RotatingFileHandler)handlers[i];
                }
            }
        }

        public void Critical(string message, ConsoleColor color = ConsoleColor.White) => LoggerCommon(Level.Critical, message, color);

        public void Except(string message, Exception ex, ConsoleColor color = ConsoleColor.White) {
            message = $"{message}\n  {ex.GetType().Name}: {ex.Message} at {ex.Source}\n  {ex.StackTrace}";
            LoggerCommon(Level.Except, message, color);
        }

        public void Error(string message, ConsoleColor color = ConsoleColor.White) => LoggerCommon(Level.Error, message, color);

        public void Warn(string message, ConsoleColor color = ConsoleColor.White) => LoggerCommon(Level.Warn, message, color);

        public void Info(string message, ConsoleColor color = ConsoleColor.White) => LoggerCommon(Level.Info, message, color);

        public void Debug(string message, ConsoleColor color = ConsoleColor.White) => LoggerCommon(Level.Debug, message, color);

        private void LoggerCommon(Level level, string message, ConsoleColor color) {
            string logMessage = this.formatter.BuildLogMessage(level, message);
            if ((int)streamHandler.MinLevel <= (int)level) {
                streamHandler.StreamConsole(logMessage, color);
            }
            if ((int)fileHandler.MinLevel <= (int)level) {
                fileHandler.StreamFile(logMessage);
            }
            if ((int)rotatingFileHandler.MinLevel <= (int)level) {
                rotatingFileHandler.RotateLog(logMessage);
            }
        }
    }
}
