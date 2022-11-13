using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace LoggingNcore {
    /// <summary>
    /// サイズ，または日付ごとにログローテーションするやつ  なんかカオス化してて草()
    /// </summary>
    public class RotatingFileHandler : FileHandler {
        public enum SizeUnit {
            B = 1,
            KB = 10^3,
            MB = 10^6,
            GB = 10^9
        }
        private enum RotateBy {
            Date,
            Size
        }

        private RotateBy rotateMode;
        private static int rotateSpan;
        public static int RotateSpan { get; set; }
        private static float rotateSize;
        public static int RotateSize { get; set; }
        private static string defaultFileName = DateTime.Today.ToString("yyMMdd");

        public new Level MinLevel { get; set; } = Level.Disabled;


        public new string LogFilePath {
            get { return $"{LogFileDirectory}/{LogFileName}{LogFileExtension}"; } }

        /// <summary>
        /// 日付ベースでローテートさせる
        /// </summary>
        /// <param name="logDir">ログディレクトリ</param>
        /// <param name="logExt">ログの拡張子</param>
        /// <param name="rotSpan">最新のログが作られてからローテートするまでの日数</param>
        public RotatingFileHandler(string logDir, string logExt, int rotSpan) : base($"{logDir}/{defaultFileName}{logExt}") {
            rotateMode = RotateBy.Date;
            rotateSpan = rotSpan;
            SetCurrentLogFileName();
        }

        /// <summary>
        /// サイズベースでローテートさせる
        /// </summary>
        /// <param name="logDir">ログディレクトリ</param>
        /// <param name="logExt">ログの拡張子</param>
        /// <param name="rotSize">ローテーションのサイズ閾値</param>
        /// <param name="unit">単位</param>
        public RotatingFileHandler(string logDir, string logExt, float rotSize, SizeUnit unit) : base($"{logDir}/{defaultFileName}{logExt}") {
            rotateMode = RotateBy.Size;
            rotateSize = rotSize * (float)unit;
            SetCurrentLogFileName();
        }

        /// <summary>
        /// プロパティにデフォルトとして突っ込む用 使わない
        /// </summary>
        internal RotatingFileHandler() {
            MinLevel = Level.Disabled;
        }

        public void RotateLog(string message) {
            void RotateBySize() {
                bool isExists = IsExistLog();
                if (new FileInfo(LogFilePath).Length > rotateSize) {
                    string oldName = Path.GetFileNameWithoutExtension(LogFilePath);
                    if (oldName == defaultFileName) {
                        string[] logs = Directory.GetFiles(LogFileDirectory, $"{oldName}-*{LogFileExtension}");
                        string oldPath = $"{LogFileDirectory}/{oldName}{LogFileExtension}";
                        string newPath = $"{LogFileDirectory}/{oldName}-{logs.Length + 1}{LogFileExtension}";
                        File.Move(oldPath, newPath);
                    }
                    LogFileName = defaultFileName;
                    CreateLog(LogFilePath);
                }
            }
            void RotateByDate() {
                bool isExists = IsExistLog();
                string[] files = Directory.GetFiles(LogFileDirectory, "*" + LogFileExtension);
                string[] sorted = files.OrderBy(f => File.GetCreationTime(f)).ToArray();
                if (sorted.Length > 1) {
                    string oldName = Path.GetFileNameWithoutExtension(sorted[sorted.Length - 2]);
                    string oldPath = $"{LogFileDirectory}/{oldName}{LogFileExtension}";
                    string newFullPath = $"{LogFileDirectory}/{oldName}-{sorted.Length - 1}{LogFileExtension}";
                    File.Move(oldPath, newFullPath);
                }
            }
            bool IsExistLog() {
                bool isExists = File.Exists(LogFilePath);
                if (!isExists) {
                    CreateLog(LogFilePath);
                }
                return isExists;
            }

            switch (rotateMode) {
                case RotateBy.Date:
                    RotateByDate();
                    break;
                case RotateBy.Size:
                    RotateBySize();
                    break;
            }
            StreamFile(message);
        }

        public void SetCurrentLogFileName() {
            string GetCurrentLogFileNameByDate() {
                string[] files = Directory.GetFiles(LogFileDirectory, "*" + LogFileExtension);
                var latest = "";
                if (files.Length > 0) {
                    var sorted = files.OrderBy(f => File.GetCreationTime(f)).ToArray();

                    DateTime logDate = File.GetCreationTime(sorted[sorted.Length - 1]);
                    DateTime rotateDate = logDate.AddDays(rotateSpan);

                    latest = rotateDate <= DateTime.Today ? defaultFileName : sorted[sorted.Length - 1];
                }
                else {
                    latest = defaultFileName;
                }

                return Path.GetFileNameWithoutExtension(latest);                
            }
            string GetCurrentLogFileNameBySize() {
                string latest = "";
                string[] files = Directory.GetFiles(LogFileDirectory, "*" + LogFileExtension);
                if (files.Length > 0) {
                    string[] archived = Directory.GetFiles(LogFileDirectory, "*-*" + LogFileExtension);
                    string[] merged = files.Union(archived).ToArray();
                    string[] sorted = files.OrderBy(f => File.GetCreationTime(f)).ToArray();
                    latest = sorted[sorted.Length - 1];
                    if (Path.GetFileNameWithoutExtension(latest) != defaultFileName) {
                        latest = defaultFileName;
                    }
                }
                else {
                    latest = defaultFileName;
                }
                return Path.GetFileNameWithoutExtension(latest);
            }

            // Function main
            switch (rotateMode) {
                case RotateBy.Date:
                    LogFileName = GetCurrentLogFileNameByDate();
                    break;
                case RotateBy.Size:
                    LogFileName = GetCurrentLogFileNameBySize();
                    break;
            }
        }

        public void LoadSettingsRotHandler() {

        }
    }
}
