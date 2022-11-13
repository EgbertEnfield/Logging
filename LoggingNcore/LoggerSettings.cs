using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging {
    internal static class SettingsGetters {
        internal static void GetSettings() {
            string fileName = Directory.GetCurrentDirectory() + "Settings.json";
            using(var sr = new StreamReader(fileName)) {
                string rawJson = sr.ReadToEnd();

            }
        }
    }

    internal class Settings {
#pragma warning disable CS8618 // null 非許容のフィールドには、コンストラクターの終了時に null 以外の値が入っていなければなりません。Null 許容として宣言することをご検討ください。
        internal Formatter Formatter { get; set; }                        
#pragma warning restore CS8618 // null 非許容のフィールドには、コンストラクターの終了時に null 以外の値が入っていなければなりません。Null 許容として宣言することをご検討ください。
    }
}
