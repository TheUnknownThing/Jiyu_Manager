using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime.InteropServices;
using EasyHook;
using System.IO;
using System.Threading;

namespace Jiyu_Manager.Jiyu_Manager_Class
{
    public class JiyuManagerHookClass
    {
        public delegate int HookProc(int nCode, IntPtr wParam, IntPtr lParam);

        #region DLLImport
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern bool UnhookWindowsHookEx(int idHook);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int CallNextHookEx(int idHook, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("User32.dll", EntryPoint = "FindWindow", CharSet = CharSet.Auto)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        #endregion

        public static bool RegGACAssembly()
        {
            var dllName = "EasyHook.dll";
            var dllPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dllName);
            if (!RuntimeEnvironment.FromGlobalAccessCache(Assembly.LoadFrom(dllPath)))
            {
                new System.EnterpriseServices.Internal.Publish().GacInstall(dllPath);
                Thread.Sleep(100);
            }
            dllName = "JiyuHooker.dll";
            dllPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dllName);
            new System.EnterpriseServices.Internal.Publish().GacRemove(dllPath);
            if (!RuntimeEnvironment.FromGlobalAccessCache(Assembly.LoadFrom(dllPath)))
            {
                new System.EnterpriseServices.Internal.Publish().GacInstall(dllPath);
                Thread.Sleep(100);
            }
            return true;
        }
        public class HookParameter
        {
            public string Msg { get; set; }
            public int HostProcessId { get; set; }
        }

        #region InstallHook
        private static int GetGuangboProcessID()
        {
            IntPtr GuangboProcess = FindWindow(null, "屏幕广播");

            if (GuangboProcess != null)
            {
                return GuangboProcess.ToInt32();
            }
            else
            {
                return 0;
            }
        }
        public static bool InstallHookInternal()
        {
            int GuangboProcessID = GetGuangboProcessID();
            if (GuangboProcessID != 0)
            {
                try
                {
                    var parameter = new HookParameter
                    {
                        Msg = "已经成功注入目标进程",
                        HostProcessId = RemoteHooking.GetCurrentProcessId()
                    };
                    RemoteHooking.Inject(
                                        GuangboProcessID,
                                        InjectionOptions.Default,
                                        typeof(HookParameter).Assembly.Location,
                                        typeof(HookParameter).Assembly.Location,
                                        string.Empty,
                                        parameter
                                    );
                    System.Windows.MessageBox.Show(parameter.Msg);
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("注入失败" + ex.ToString());
                    return false;
                }
                return true;
            }
            else
            {
                System.Windows.MessageBox.Show("未找到进程");
                return false;
            }
        }
        #endregion

    }
}
