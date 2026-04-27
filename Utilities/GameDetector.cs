using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Threading;
using GearOS.Models;

namespace GearOS.Utilities
{
    public class GameDetector
    {
        private DispatcherTimer _timer;
        private string _lastActiveProcess = "";
        private List<DeviceProfile> _availableProfiles;

        public event Action<DeviceProfile> ProfileActivated;

        public GameDetector(List<DeviceProfile> profiles)
        {
            _availableProfiles = profiles;
            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _timer.Tick += (s, e) => CheckForegroundProcess();
        }

        public void Start() => _timer.Start();

        private void CheckForegroundProcess()
        {
            IntPtr handle = GetForegroundWindow();
            GetWindowThreadProcessId(handle, out uint processId);

            try
            {
                var process = Process.GetProcessById((int)processId);
                string currentProcess = process.ProcessName;

                if (currentProcess != _lastActiveProcess)
                {
                    _lastActiveProcess = currentProcess;
                    var profile = _availableProfiles?.FirstOrDefault(p =>
                        p.TargetProcessName.Equals(currentProcess, StringComparison.OrdinalIgnoreCase));

                    if (profile != null) ProfileActivated?.Invoke(profile);
                }
            }
            catch { }
        }

        [DllImport("user32.dll")] private static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")] private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
    }
}