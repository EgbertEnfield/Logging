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
        internal Formatter Formatter { get; set; }                        
    }
}
