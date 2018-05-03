using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Bubak.Shared.Misc
{
    public class DebugLogger : ILogger
    {
        public void Log(string message, [CallerMemberName] string callerName = null)
        {
            var s = callerName == null
                ? $"{DateTime.Now}: {message}"
                : $"{DateTime.Now}: {callerName} >>> {message}";

            Debug.Print(s);
        }

        public void LogException(Exception exception, [CallerMemberName] string callerName = null)
        {
            Log($"{exception.GetType()} - {exception.Message}{Environment.NewLine}{exception.StackTrace}");
        }
    }
}
