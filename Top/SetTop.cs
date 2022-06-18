using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Top
{
    static class SetTop
    {
        public static Hashtable processWnd = null;
        static SetTop()
        {
            if (processWnd == null)
            {
                processWnd = new Hashtable();
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ProcessEntry32
        {
            public uint dwSize;
            public uint cntUsage;
            public uint th32ProcessID;
            public IntPtr th32DefaultHeapID;
            public uint th32ModuleID;
            public uint cntThreads;
            public uint th32ParentProcessID;
            public int pcPriClassBase;
            public uint dwFlags;


            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szExeFile;
        }
        [DllImport("user32.dll", EntryPoint = "SetActiveWindow")]
        public static extern IntPtr SetActiveWindow(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int Width, int Height, int flags);

        public delegate bool WNDENUMPROC(IntPtr hwnd, uint lParam);

        [DllImport("user32.dll")]
        public static extern long GetWindowThreadProcessId(IntPtr hWnd, ref uint lpdwProcessId);
        [DllImport("user32", EntryPoint = "IsWindow")]
        private static extern bool IsWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern int SetParent(IntPtr hWndChild, IntPtr hWndParent);

        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern IntPtr GetParent(IntPtr hWnd);
        [DllImport("kernel32.dll")]
        public static extern IntPtr CreateToolhelp32Snapshot(uint flags, uint processid);
        [DllImport("user32.dll", EntryPoint = "EnumWindows", SetLastError = true)]
        public static extern bool EnumWindows(WNDENUMPROC lpEnumFunc, uint lParam);

        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        public static extern IntPtr FindWindow(string IpClassName, string IpWindowName);
        [DllImport("kernel32.dll")]
        public static extern int CloseHandle(IntPtr handle);
        [DllImport("kernel32.dll")]
        public static extern int Process32First(IntPtr handle, ref ProcessEntry32 pe);
        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        [DllImport("kernel32.dll", EntryPoint = "SetLastError")]
        public static extern void SetLastError(uint dwErrCode);
        [DllImport("user32.dll", EntryPoint = "PostMessage")]
        public static extern int PostMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        [DllImport("kernel32.dll")]
        public static extern int Process32Next(IntPtr handle, ref ProcessEntry32 pe);
        public static IntPtr GetHandleByProcessName(string ProcessName)
        {
            List<ProcessEntry32> list = new List<ProcessEntry32>();
            IntPtr handle = CreateToolhelp32Snapshot(0x2, 0);
            IntPtr hh = IntPtr.Zero;
            if ((int)handle > 0)
            {
                ProcessEntry32 pe32 = new ProcessEntry32();
                pe32.dwSize = (uint)Marshal.SizeOf(pe32);
                int bMore = Process32First(handle, ref pe32);
                while (bMore == 1)
                {
                    IntPtr temp = Marshal.AllocHGlobal((int)pe32.dwSize);
                    Marshal.StructureToPtr(pe32, temp, true);
                    ProcessEntry32 pe = (ProcessEntry32)Marshal.PtrToStructure(temp, typeof(ProcessEntry32));
                    Marshal.FreeHGlobal(temp);
                    list.Add(pe);
                    if (pe.szExeFile == ProcessName)
                    {
                        bMore = 2;
                        hh = GetCurrentWindowHandle(pe.th32ProcessID);
                        break;
                    }
                    bMore = Process32Next(handle, ref pe32);
                }
            }
            return hh;
        }
        public static IntPtr GetCurrentWindowHandle(uint proid)
        {
            IntPtr ptrWnd = IntPtr.Zero;
            uint uiPid = proid;
            object objWnd = processWnd[uiPid];
            if (objWnd != null)
            {
                ptrWnd = (IntPtr)objWnd;
                if (ptrWnd != IntPtr.Zero && IsWindow(ptrWnd))  // 从缓存中获取句柄
                {
                    return ptrWnd;
                }
                else
                {
                    ptrWnd = IntPtr.Zero;
                }
            }
            bool bResult = EnumWindows(new WNDENUMPROC(EnumWindowsProc), uiPid);
            // 枚举窗口返回 false 并且没有错误号时表明获取成功
            if (!bResult && Marshal.GetLastWin32Error() == 0)
            {
                objWnd = processWnd[uiPid];
                if (objWnd != null)
                {
                    ptrWnd = (IntPtr)objWnd;
                }
            }
            return ptrWnd;
        }


        private static bool EnumWindowsProc(IntPtr hwnd, uint lParam)
        {
            uint uiPid = 0;
            if (GetParent(hwnd) == IntPtr.Zero)
            {
                GetWindowThreadProcessId(hwnd, ref uiPid);
                if (uiPid == lParam)    // 找到进程对应的主窗口句柄
                {
                    processWnd.Add(uiPid, hwnd);   // 把句柄缓存起来
                    SetLastError(0);    // 设置无错误
                    return false;   // 返回 false 以终止枚举窗口
                }
            }
            return true;
        }


    }
}
