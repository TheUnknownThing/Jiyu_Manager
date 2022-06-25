using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Jiyu_Manager.Jiyu_Manager_Class
{
    class RunCommand
    {
        private static string CmdPath = @"C:\Windows\System32\cmd.exe";
        public static string RunCmd(string cmd)
        {
            cmd = cmd.Trim().TrimEnd('&') + "&exit";//说明：不管命令是否成功均执行exit命令，否则当调用ReadToEnd()方法时，会处于假死状态
            using (Process p = new Process())
            {
                p.StartInfo.FileName = CmdPath;
                p.StartInfo.UseShellExecute = false; //是否使用操作系统shell启动
                p.StartInfo.RedirectStandardInput = true; //接受来自调用程序的输入信息
                p.StartInfo.RedirectStandardOutput = true; //由调用程序获取输出信息
                p.StartInfo.RedirectStandardError = true; //重定向标准错误输出
                p.StartInfo.CreateNoWindow = true; //不显示程序窗口
                p.Start();//启动程序

                p.StandardInput.WriteLine(cmd);
                p.StandardInput.AutoFlush = true;
                

                string output = p.StandardOutput.ReadToEnd();
                p.WaitForExit();
                p.Close();

                return output;
            }
        }
    }
}
