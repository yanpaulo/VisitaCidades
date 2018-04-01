using System;
using VisitaCidades.Model;

namespace VisitaCidades.Win32
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Bootstrapper.Run(args);
        }
    }
#endif
}
