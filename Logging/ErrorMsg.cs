using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging {
    internal static class LoggerError {
        public enum Status {         
            FormatNotDefined,
            DateFormatNotDefined,
            [StatusInfo("0xE381AAE38293E381A7E38284E998AAE7A59EE996A2E4BF82E784A1E38184E38284E3828D21")]
            NullReferenceException
        }
    }

    public class StatusCodeAttribute: Attribute {
        public int StatusCode { get; internal set; }

        public StatusCodeAttribute(int code) {
            StatusCode = code;
        }
    }

    public class StatusInfoAttribute : Attribute {
        public string StatusInfo { get; internal set; }

        public StatusInfoAttribute(string value) {
            this.StatusInfo = value;
        }
    }



    public static class CommonAttribute {
        /// <summary>
        /// Get detail error message.
        /// </summary>
        /// <param name="value">The enum element which want to get message.</param>
        /// <returns>If message specified, it returns the message. If not, it is the element name. null is error.</returns>
        public static string GetStatusInfo(this Enum value) {
            Type type = value.GetType();
            FieldInfo fieldInfo = type.GetField(value.ToString());

            if (fieldInfo == null) return null;

            StatusInfoAttribute[] attribs = fieldInfo.GetCustomAttributes(typeof(StatusInfoAttribute), false) as StatusInfoAttribute[];
            string msg = attribs.Length > 0 ? attribs[0].StatusInfo : value.ToString();
            return msg;
        }

        /// <summary>
        /// Get error status code
        /// </summary>
        /// <param name="value">The enum element which want to get code</param>
        /// <returns>If code defined, it returns the code. If not, it is default enum value adding 1. -1 is error</returns>
        public static int GetStatusCode(this Enum value) {
            Type type = value.GetType();
            FieldInfo fieldInfo = type.GetField(value.ToString());
            
            if (fieldInfo == null) return -1;
            StatusCodeAttribute[] attributes = fieldInfo.GetCustomAttributes(typeof(StatusCodeAttribute), false) as StatusCodeAttribute[];
            int code = attributes.Length > 0 ? attributes[0].StatusCode : (int)LoggerError.Status.FormatNotDefined + 1;
            return code;
        }
    }
}
