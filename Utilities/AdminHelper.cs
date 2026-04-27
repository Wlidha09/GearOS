using System;
using System.Diagnostics;
using System.Security.Principal;

namespace GearOS.Utilities
{
    public static class AdminHelper
    {
        public static bool IsRunningAsAdmin()
        {
            try
            {
                using (var identity = WindowsIdentity.GetCurrent())
                {
                    var principal = new WindowsPrincipal(identity);
                    return principal.IsInRole(WindowsBuiltInRole.Administrator);
                }
            }
            catch
            {
                return false;
            }
        }

        public static void RestartAsAdmin()
        {
            var exeName = Process.GetCurrentProcess().MainModule.FileName;
            var startInfo = new ProcessStartInfo(exeName)
            {
                UseShellExecute = true,
                Verb = "runas"
            };

            try
            {
                Process.Start(startInfo);
            }
            catch
            {
                // user cancelled UAC
            }
        }
    }
}