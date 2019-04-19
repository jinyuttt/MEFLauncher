using LauncherCommon;
using System.Threading;

namespace MEFCoreLoader
{
    class Program
    {
        static readonly AutoResetEvent resetEvent = new AutoResetEvent(false);
        static void Main(string[] args)
        {
            ConsoleHepler.Hide("MEFLauncher");
            LauncherHelper.Run(resetEvent);

            resetEvent.WaitOne();
        }
    }
}
