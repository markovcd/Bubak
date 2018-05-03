using System.Runtime.CompilerServices;

namespace Bubak.Shared.Misc
{
    public interface ILogger
    {
        void Log(string message, [CallerMemberName] string callerName = null);
    }
}
