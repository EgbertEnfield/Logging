using System;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Logging {
    public class Formatter {
        public string Format { get; set; }
        public string DateFormat { get; set; }

        internal string BuildLogMessage(Level level, string message) {
            if (string.IsNullOrEmpty(this.Format)) {
                var err = LoggerError.Status.FormatNotDefined;
                throw new FormatException($"{err.GetStatusInfo()} Code:{err.GetStatusCode()}");
            }
            if (string.IsNullOrEmpty(this.DateFormat)) {
                var err = LoggerError.Status.DateFormatNotDefined;
                throw new FormatException($"{err.GetStatusInfo()} Code:{err.GetStatusCode()}");
            }

            string logMsg = this.Format;

            if (Regex.IsMatch(this.Format, @"{asctime}")) {
                logMsg = Regex.Replace(logMsg, @"{asctime}", DateTime.Now.ToString(this.DateFormat));
            }
            if (Regex.IsMatch(this.Format, @"{level}")) {
                logMsg = Regex.Replace(logMsg, @"{level}", level.ToString());
            }
            if (Regex.IsMatch(this.Format, @"{message}")) {
                logMsg = Regex.Replace(logMsg, @"{message}", message);
            }
            if (Regex.IsMatch(this.Format, @"{funcName}")) {
                logMsg = Regex.Replace(logMsg, @"{funcName}", MethodBase.GetCurrentMethod().Name);
            }
            return logMsg;
        }
    }
}
