namespace Bubak.Shared.Misc
{
    public interface ILogger
    {
        void Log(string message, string callerName = null);
    }
}
