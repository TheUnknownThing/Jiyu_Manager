using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jiyu_Manager.Jiyu_Manager_Class
{
    class DebugOutputClass
    {
        private static string GetCurrentTime()
        {
            return DateTime.Now.ToString();
        }
        public delegate void MyDelegate(string Item1);
        public static void DebugOutputText(string output)
        {
            MyDelegate myDelegate = new MyDelegate(DebugOutput.debugOutput.UpdateTextBox);
            myDelegate(GetCurrentTime()+" "+output+"\n");
        }
    }
}
