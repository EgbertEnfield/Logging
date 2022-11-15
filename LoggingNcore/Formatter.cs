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
        /// 0: false, 1~: true でフォーマットの必須を指定
        /// </summary>
        public enum FormatRequired {
            TimeStanp = 0x1,
            LogLevel = 0x2,
            Message = 0x4
        }

        public Formatter(string format, string dateFormat) {
            this.Format = format;
            this.DateFormat = dateFormat;

            bool isProper = IsProperFormat();
            Console.WriteLine($"Format result: {isProper}");
        }

        private bool IsProperFormat() {
            int flag = 0;
            int errFlag = 0;
            string format = this.Format;

            // 必須項目のチェック用例示フラグ
            errFlag |= (int)FormatRequired.TimeStanp;
            errFlag |= (int)FormatRequired.LogLevel;
            errFlag |= (int)FormatRequired.Message;

            // 必須項目のチェック用フラグ
            flag |= format.Contains(FormatComps.TimeStamp) ? (int)FormatRequired.TimeStanp : 0;
            flag |= format.Contains(FormatComps.LogLevel) ? (int)FormatRequired.LogLevel : 0;
            flag |= format.Contains(FormatComps.Message) ? (int)FormatRequired.Message : 0;

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
            get { return "{asctime}"; }
        }
        public static string LogLevel {
            get { return "{level}"; }
        }
        public static string Message {
            get { return "{message}"; }
        }
    }
}
