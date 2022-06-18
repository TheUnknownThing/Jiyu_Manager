using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;

namespace Jiyu_Manager.Jiyu_Manager_Class
{
    public class TopMostClass
    {
        #region SetTopMost
        public static int SW_SHOW = 5;

        public static int SW_NORMAL = 1;

        public static int SW_MAX = 3;

        public static int SW_HIDE = 0;

        public static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);

        public static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);

        public const uint SWP_NOMOVE = 0x0002;    //不调整窗体位置

        public const uint SWP_NOSIZE = 0x0001;    //不调整窗体大小

        [DllImport("user32.dll")]

        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll", EntryPoint = "ShowWindow")]

        public static extern bool ShowWindow(System.IntPtr hWnd, int nCmdShow);

        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("User32.dll", EntryPoint = "FindWindowEx")]
        private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpClassName, string lpWindowName);
        public static void TopMostMainWindow()
        {

            IntPtr MainWindowProcess = FindWindow(null, "Jiyu_Manager_MainWindow");

            if (MainWindowProcess != null)
            {

                SetWindowPos(MainWindowProcess, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE);

                //System.Windows.MessageBox.Show(MainWindowProcess.ToInt32().ToString() + " Success");
            }

            if (ProgramInf.isDevFeatureEnabled) //For debug usage
            {
                IntPtr SpyWindowProcess = FindWindow(null, "窗口查看器 V1.31 -[361度]-");
                if (SpyWindowProcess != null)
                {

                    SetWindowPos(SpyWindowProcess, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE);

                }
            }
        }
        #endregion
        #region SetNoFocus

        private const int WS_EX_NOACTIVATE = 0x08000000;

        private const int GWL_EXSTYLE = -20;

        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        private static extern IntPtr GetWindowLong32(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
        private static extern IntPtr GetWindowLong64(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        private static extern IntPtr SetWindowLong32(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
        private static extern IntPtr SetWindowLong64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);
        public static IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
        {
            return Environment.Is64BitProcess
                ? SetWindowLong64(hWnd, nIndex, dwNewLong)
                : SetWindowLong32(hWnd, nIndex, dwNewLong);
        }
        public static IntPtr GetWindowLong(IntPtr hWnd, int nIndex)
        {
            return Environment.Is64BitProcess
                ? GetWindowLong64(hWnd, nIndex)
                : GetWindowLong32(hWnd, nIndex);
        }
        public static void SetWindowNoFocus()
        {
            IntPtr handle = FindWindow(null, "Jiyu_Manager_MainWindow");
            var exstyle = GetWindowLong(handle, GWL_EXSTYLE);
            SetWindowLong(handle, GWL_EXSTYLE, new IntPtr(exstyle.ToInt32() | WS_EX_NOACTIVATE));
        }
        #endregion
    }
}
