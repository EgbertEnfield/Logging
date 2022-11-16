using System;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Logging.NetCore {
    /// <summary>
    /// ログフォーマットを決めるやつ  必ず使う
    /// </summary>
    public class Formatter {
        public string Format { get; set; }
        public string DateFormat { get; set; }

        // フォーマットの置換フラグ
        // ログ時にstring.Contains()が無くなることで軽量化する可能性が微レ存
        private int formatFlag = 0;

        /// <summary>
        /// Format設定に使うフォーマット指定子たち
        /// </summary>
        public enum FormatRequired {
            TimeStanp = 0x1,
            LogLevel = 0x2,
            LogValue = 0x4,
            Message = 0x8,
            Count = 0x10,
        }

        internal bool CheckFormat() {
            int flag = 0;
            int errFlag = 0;
            string format = this.Format;

            foreach(FormatRequired val in Enum.GetValues(typeof(FormatRequired))) {
                formatFlag |= format.Contains(Enum.GetName(typeof(FormatRequired), val)) ? (int)val : 0;
            }

            // フラグの値はEnumからとってきてるので大丈夫なはず
            if (errFlag != flag) {
                var err = LoggerError.Status.FormatNotDefined;
                Console.Error.WriteLine(err.GetStatusInfo());
                return false;
            }
            else {
                formatFlag = flag;
                return true;
            }
        }

        internal string BuildLogMessage(Level level, string message) {
            if (string.IsNullOrEmpty(this.DateFormat)) {
                var err = LoggerError.Status.DateFormatNotDefined;
                throw new FormatException(err.GetStatusInfo());
            }  

            string logMsg = this.Format;

            foreach (FormatRequired val in Enum.GetValues(typeof(FormatRequired))) {
                if((formatFlag & (int)val) > 0) {
                    switch (val) {
                    }
                }
            }

            if ((formatFlag & (int)FormatRequired.TimeStanp) > 0) {
                logMsg = logMsg.Replace("{asctime}", DateTime.Now.ToString(this.DateFormat));
            }
            if ((formatFlag & (int)FormatRequired.LogLevel) > 0) {
                logMsg = logMsg.Replace("{level}", level.ToString());
            }
            if ((formatFlag & (int)FormatRequired.Message) > 0) {
                logMsg = logMsg.Replace("{message}", message);
            }

            return logMsg;
        }
    }

    /// <summary>
    /// FormatterのFormat指定用
    /// </summary>
    public static class FormatComps {
        public static string TimeStamp {
            get { return Formatter.FormatRequired.TimeStanp.ToString(); }
        }
        public static string LogLevel {
            get { return Formatter.FormatRequired.LogValue.ToString(); }
        }
        public static string Message {
            get { return Formatter.FormatRequired.Message.ToString(); }
        }
        public static string LogValue {
            get { return Formatter.FormatRequired.LogValue.ToString(); }
        }
        public static string Count {
            get { return Formatter.FormatRequired.Count.ToString(); }
        }
    }
}
