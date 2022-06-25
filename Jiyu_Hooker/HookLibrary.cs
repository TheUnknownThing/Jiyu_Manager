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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

        }

        #region SetWindowPosHook_Prep
        [DllImport("user32.dll", EntryPoint = "SetWindowPos", CharSet = CharSet.Auto)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Auto)]
        delegate bool DSetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);
        static bool SetWindowPos_Hooked(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags)
        {
            return SetWindowPos(hWnd, HWND_NOTOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE);
        }
        #endregion

    }
}
