using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WebFoxSitePackage
{
    static class ProcessExtend
    {
        // [StructLayout(LayoutKind.Sequential)]
        private struct ProcessBasicInformation
        {
            public int ExitStatus;
            public int PebBaseAddress;
            public int AffinityMask;
            public int BasePriority;
            public uint UniqueProcessId;
            public uint InheritedFromUniqueProcessId;
        }

        [DllImport("ntdll.dll")]
        static extern int NtQueryInformationProcess(
           IntPtr hProcess,
           int processInformationClass /* 0 */,
           ref ProcessBasicInformation processBasicInformation,
           uint processInformationLength,
           out uint returnLength
        );

        /// <summary>
        /// 遍历杀进程
        /// </summary>
        /// <param name="parent"></param>
        public static void KillProcessTree(this Process parent)
        {
            var processes = Process.GetProcesses();
            foreach(var p in processes)
            {
                var pbi = new ProcessBasicInformation();
                try
                {
                    uint bytesWritten;
                    if(NtQueryInformationProcess(p.Handle, 0, ref pbi, (uint)Marshal.SizeOf(pbi), out bytesWritten) == 0) // == 0 is OK
                        if(pbi.InheritedFromUniqueProcessId == parent.Id)
                            using(var newParent = Process.GetProcessById((int)pbi.UniqueProcessId))
                                newParent.KillProcessTree();
                }
                catch { }
            }
            parent.Kill();
        }
    }
}
