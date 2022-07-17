using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace LCUSharp.Utility
{
    /// <summary>
    /// Manages the operations relating to the league client's process.
    /// </summary>
    internal class LeagueProcessHandler
    {
        private const string ProcessName = "LeagueClientUx";

        /// <summary>
        /// Triggers when the league client is closed.
        /// </summary>
        public event EventHandler Closed;

        private Process _process;
        /// <summary>
        /// The league client's process.
        /// </summary>
        public Process Process
        {
            get { return _process; }
            private set
            {
                if (Process != null)
                {
                    Process.Exited -= OnProcessExited;
                }

                _process = value;
                Process.EnableRaisingEvents = true;
                Process.Exited += OnProcessExited;

                ExecutablePath = Path.GetDirectoryName(Process.MainModule.FileName);
            }
        }

        /// <summary>
        /// The league client's executable path.
        /// </summary>
        public string ExecutablePath { get; private set; }

        /// <summary>
        /// Waits for the league client's process to start.
        /// </summary>
        /// <returns>True if the process was found successfully, otherwise false.</returns>
        public async Task<bool> WaitForProcessAsync()
        {
            while (true)
            {
                var newProcess = TryGetProcess();
                if (newProcess == null)
                {
                    await Task.Delay(100).ConfigureAwait(false);
                    continue;
                }

                Process = newProcess;
                return true;
            }
        }

        private Process TryGetProcess()
        {
            var processes = Process.GetProcessesByName(ProcessName);
            if (processes.Length <= 0)
            {
                return null;
            }

            var newProcess = processes[0];
            if (newProcess == null || newProcess.MainModule == null)
            {
                return null;
            }

            return newProcess;
        }

        /// <summary>
        /// Called when the league client is closed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnProcessExited(object sender, EventArgs e)
        {
            Closed?.Invoke(sender, e);
        }
    }
}
