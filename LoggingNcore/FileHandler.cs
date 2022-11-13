using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggingNcore {
    /// <summary>
    /// 任意の特定ファイルにどんどん追記していくやつ
    /// </summary>
    public class FileHandler {
        protected string LogFileDirectory { get; set; }
        protected string LogFileExtension { get; set; }
        protected string LogFileName { get; set; }
        public string LogFilePath {
            get { return $"{LogFileDirectory}/{LogFileName}{LogFileExtension}"; }
        }

        public FileMode Mode { get; set; } = FileMode.Append;
        public Level MinLevel { get; set; } = Level.Disabled;

        public FileHandler(string fullPath) {
            LogFileDirectory = Path.GetDirectoryName(fullPath);
            LogFileExtension = Path.GetExtension(fullPath);
            LogFileName = Path.GetFileNameWithoutExtension(fullPath);
        }
        /// <summary>
        /// プロパティにデフォルトとして突っ込む用 使っても何もしない
        /// </summary>
        internal FileHandler() {
            MinLevel = Level.Disabled;
        }

        /// <summary>
        /// 特定のファイルにログを追記していく
        /// </summary>
        /// <param name="message">ログ内容</param>
        public void StreamFile(string message) {
            bool isAppend = Mode == FileMode.Append ? true : false;
            using (StreamWriter writer = new StreamWriter(LogFilePath, isAppend)) {
                if (!File.Exists(LogFilePath)) {
                    CreateLog(LogFilePath);
                }
                writer.WriteLine(message);
            }
        }

        public void CreateLog(string path) {
            using (File.Create(path)) { }
        }
    }
}
