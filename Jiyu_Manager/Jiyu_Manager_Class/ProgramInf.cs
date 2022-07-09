using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Jiyu_Manager.Jiyu_Manager_Class
{
    public static class ProgramInf
    {
        public static string SoftwareVersion = "Version 0.2 Dev";
        public static bool isDevFeatureEnabled = true;
        public static int VersionReleaseData = 20220706;
        public static IntPtr ProgramProcessID ;

        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        public static void GetProgramProcessID()
        {
            IntPtr MainWindowProcess = FindWindow(null, "Jiyu_Manager_MainWindow");
            ProgramProcessID = MainWindowProcess;
            if (ProgramProcessID == (IntPtr)0)
            {
                DebugOutputClass.DebugOutputText("Get Mainwindow ProcessID Failed!");
            }
        }
    }
}
