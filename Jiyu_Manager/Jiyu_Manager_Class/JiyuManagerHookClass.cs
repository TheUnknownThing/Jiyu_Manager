using Jiyu_Hooker;
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
using System.Diagnostics;

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

        [DllImport("user32.dll", EntryPoint = "FindWindow", CharSet = CharSet.Auto)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowThreadProcessId(IntPtr hwnd, out int ID);
        #endregion



        #region InstallHook
        private static int GetGuangboProcessID()
        {
            
            IntPtr GuangboProcess = FindWindow(null, "屏幕广播");

            if (GuangboProcess != null)
            {
                int calcID = 0;    //进程ID
                int calcTD = 0;    //线程ID
                calcTD = GetWindowThreadProcessId(GuangboProcess, out calcID);
                Process myProcess = Process.GetProcessById(calcID);
                return calcID;
            }
            else
            {
                return 0;
            }
            

            /*
            var p = Process.GetProcessesByName("studentmain");
            if (p.Length > 0)
            {
                return p.FirstOrDefault().Id;
            }
            else
            {
                System.Windows.MessageBox.Show("Not Found");
                return 0;
            }
            */
        }
        public static bool RegHookerGACAssembly()
        {
            var dllName = "Jiyu_Hooker.dll";
            var dllPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dllName);
            new System.EnterpriseServices.Internal.Publish().GacRemove(dllPath);
            /*
            if (!RuntimeEnvironment.FromGlobalAccessCache(Assembly.LoadFrom(dllPath)))
            {
                new System.EnterpriseServices.Internal.Publish().GacInstall(dllPath);
                Thread.Sleep(100);
            }
            System.Windows.MessageBox.Show("Hooker Success");
            */
            return true;
        }
        public static bool RegEasyhookGACAssembly()
        {
            var dllName = "EasyHook.dll";
            var dllPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dllName);
            new System.EnterpriseServices.Internal.Publish().GacRemove(dllPath);
            /*
            new System.EnterpriseServices.Internal.Publish().GacInstall(dllPath);
            System.Windows.MessageBox.Show("Easyhook Success");
            */
            return true;
        }
        public static bool InstallHookInternal()
        {
            System.Windows.MessageBox.Show("Start");
            int GuangboProcessID = 0;
            try
            {
                GuangboProcessID = GetGuangboProcessID();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString());
            }
            System.Windows.MessageBox.Show(typeof(HookParameter).Assembly.Location);
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
