using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityPack.IO {
    public class WindowProcess {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file_full_name"></param>
        /// <returns></returns>
        public static bool callBackProcess(string file_full_name) {
            try {
                Process.Start(file_full_name);
                return true;
            }
            catch {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="process_name"></param>
        /// <returns></returns>
        public static bool killAllProcessByName(string process_name) {
            try {
                var agent_transport_processes = Process.GetProcesses().Where(pr => pr.ProcessName.ToLower().Contains(process_name.ToLower()));
                if (agent_transport_processes.ToList().Count == 0) return true;
                foreach (var process in agent_transport_processes) {
                    process.Kill();
                }
                return true;
            }
            catch {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="process_name"></param>
        /// <returns></returns>
        public static bool isProcessRunning(string process_name) {
            try {
                var agent_transport_processes = Process.GetProcesses().Where(pr => pr.ProcessName.ToLower().Contains(process_name.ToLower()));
                return agent_transport_processes.ToList().Count > 0;
            }
            catch {
                return false;
            }
        }

        public static int processRunningCounter(string process_name) {
            try {
                var agent_transport_processes = Process.GetProcesses().Where(pr => pr.ProcessName.ToLower().Contains(process_name.ToLower()));
                return agent_transport_processes.ToList().Count;
            }
            catch {
                return 0;
            }
        }

    }
}
