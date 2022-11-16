using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging.NetCore {
    public class Logger {
        private Formatter? formatter;
        private FileHandler? fileHandler = null;
        private StreamHandler? streamHandler = null;
        private RotatingFileHandler? rotatingFileHandler = null;

        /// <summary>
        /// ロガーを設定する
        /// </summary>
        /// <param name="formatter">ログのフォーマット</param>
        /// <param name="handlers">使いたい機能のインスタンスを入れる</param>
        public Logger(Formatter formatter, params object[] handlers) {
            this.formatter = formatter.CheckFormat() ? formatter : null;

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

            if (this.formatter is null) {
                var err = LoggerError.Status.FormatNotDefined;
                throw new FormatException(err.GetStatusInfo());
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
            string logMessage = this.formatter!.BuildLogMessage(level, message);
            
            // Handlerがnullだったら実行されない (多分)
            streamHandler?.StreamConsole(logMessage, color);
            fileHandler?.StreamFile(logMessage);
            rotatingFileHandler?.RotateLog(logMessage);
        }
    }
}
