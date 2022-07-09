using System;
using System.Text;
using System.Runtime.InteropServices;
using EasyHook;
using System.Threading;
using System.Windows.Forms;

namespace Jiyu_Hooker
{
    [Serializable]
    public class HookParameter
    {
        public string Msg { get; set; }
        public int HostProcessId { get; set; }
    }

    public class Main : IEntryPoint
    {
        public static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);

        public const uint SWP_NOMOVE = 0x0002;    //不调整窗体位置

        public const uint SWP_NOSIZE = 0x0001;    //不调整窗体大小

        public LocalHook SetWindowPosHook = null;
        public LocalHook SetForegroundWindowHook = null;
        public LocalHook BringWindowToTopHook = null;
        public LocalHook MessageBoxAHook = null;
        public LocalHook MessageBoxWHook = null;

        public Main(RemoteHooking.IContext context,string channelName, HookParameter parameter)
        {
            MessageBox.Show(parameter.Msg, "Hooked");
        }

        public void Run(RemoteHooking.IContext context,string channelName, HookParameter parameter)
        {
            try
            {
                SetWindowPosHook = LocalHook.Create(
                    LocalHook.GetProcAddress("user32.dll", "SetWindowPos"),
                    new DSetWindowPos(SetWindowPos_Hooked),
                    this);
                SetWindowPosHook.ThreadACL.SetExclusiveACL(new int[1]);

                SetForegroundWindowHook = LocalHook.Create(
                    LocalHook.GetProcAddress("user32.dll", "SetForegroundWindow"),
                    new DSetForegroundWindow(SetForegroundWindow_Hooked),
                    this);
                SetForegroundWindowHook.ThreadACL.SetExclusiveACL(new int[1]);

                BringWindowToTopHook = LocalHook.Create(
                    LocalHook.GetProcAddress("user32.dll", "BringWindowToTop"),
                    new DBringWindowToTop(BringWindowToTop_Hooked),
                    this);
                BringWindowToTopHook.ThreadACL.SetExclusiveACL(new int[1]);

                MessageBoxAHook = LocalHook.Create(
                    LocalHook.GetProcAddress("user32.dll", "MessageBoxA"),
                    new DMessageBoxA(MessageBoxA_Hooked),
                    this);
                MessageBoxAHook.ThreadACL.SetExclusiveACL(new int[1]);

                MessageBoxWHook = LocalHook.Create(
                    LocalHook.GetProcAddress("user32.dll", "MessageBoxW"),
                    new DMessageBoxW(MessageBoxW_Hooked),
                    this);
                MessageBoxWHook.ThreadACL.SetExclusiveACL(new int[1]);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            try
            {
                while (true)
                {
                    Thread.Sleep(10);
                }
            }
            catch
            {

            }
        }

        #region SetWindowPosHook_Prep
        [DllImport("user32.dll", EntryPoint = "SetWindowPos", CharSet = CharSet.Auto)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)]
        delegate bool DSetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);
        static bool SetWindowPos_Hooked(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags)
        {
            return SetWindowPos(hWnd, HWND_NOTOPMOST, x, y, cx, cy, uFlags);
        }
        #endregion

        #region SetForegroundWindow_Prep
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)]
        delegate bool DSetForegroundWindow(IntPtr hWnd);
        static bool SetForegroundWindow_Hooked(IntPtr hWnd)
        {
            return true;
        }
        #endregion

        #region BringWindowToTop_Prep
        [DllImport("user32.dll")]
        private static extern bool BringWindowToTop(IntPtr hWnd);
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)]
        delegate bool DBringWindowToTop(IntPtr hWnd);
        static bool BringWindowToTop_Hooked(IntPtr hWnd)
        {
            return true;
        }
        #endregion

        #region MessageBoxA_Prep
        [DllImport("user32.dll", EntryPoint = "MessageBoxA", CharSet = CharSet.Ansi)]
        public static extern IntPtr MessageBoxA(int hWnd, string text, string caption, uint type);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        delegate IntPtr DMessageBoxA(int hWnd, string text, string caption, uint type);

        static IntPtr MessageBoxA_Hooked(int hWnd, string text, string caption, uint type)
        {
            return MessageBoxA(hWnd, "已注入-" + text, "已注入-" + caption, type);
        }
        #endregion

        #region MessageBoxW

        [DllImport("user32.dll", EntryPoint = "MessageBoxW", CharSet = CharSet.Unicode)]
        public static extern IntPtr MessageBoxW(int hWnd, string text, string caption, uint type);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        delegate IntPtr DMessageBoxW(int hWnd, string text, string caption, uint type);

        static IntPtr MessageBoxW_Hooked(int hWnd, string text, string caption, uint type)
        {
            return MessageBoxW(hWnd, "已注入-" + text, "已注入-" + caption, type);
        }

        #endregion
    }
}
