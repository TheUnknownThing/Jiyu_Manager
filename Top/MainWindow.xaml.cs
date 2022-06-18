using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows;
using System.Windows.Interop;
using System.Diagnostics;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Top
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int WM_SYSCOMMAND = 0x0112;
        private const int SC_MINIMIZE = 0xF020;
        public static bool isStartKill_toggled = false, isStartTop_toggled = false;
        public static bool isWindowLoaded = false;
        public static int sleeptime=10,topinterval=2;
        public static bool isStudnetmainExsisted = true;
        public DispatcherTimer timer;

        public MainWindow()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += new EventHandler(timer_Tick);
            isWindowLoaded = true;
        }

        private void ExcuteCommand()
        {
            Thread.Sleep(1000 * sleeptime);
            CommandExecutor.ExecuteKillCommand();
            isStudnetmainExsisted = false;
        }

        private void StartKill_Toggled(object sender, RoutedEventArgs e)
        {
            isStartKill_toggled = StartKill.IsOn;
            if (isStartKill_toggled)
            {
                Thread k = new Thread(new ThreadStart(ExcuteCommand));
                k.Start();                
            }
        }
        private void CloseTime_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (isWindowLoaded)
            {
                sleeptime = (int)CloseTime.Value;
                CloseTimeFound.Content = "关闭时间： " + sleeptime.ToString();
            }
            
        }
        private void timer_Tick(object sender,EventArgs e)
        {
            if (!isStudnetmainExsisted)
            {
                StartTop.IsOn = false;
                StartKill.IsOn = false;
                timer.Stop();
            }
            IntPtr WindowHandle = new WindowInteropHelper(this).Handle;
            SetTop.SetActiveWindow(WindowHandle);
            SetTop.SetWindowPos(WindowHandle, -1, 0, 0, 0, 0, 1 | 2);
        }

        private void fastopenjiyu_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(pos.Text);
        }

        private void fastopentsk_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("taskmgr");
        }

        private void fastminjiyu_Click(object sender, RoutedEventArgs e)
        {
            IntPtr JIYUhandle = SetTop.GetHandleByProcessName("studentmain.exe");
            if (JIYUhandle != IntPtr.Zero)
            {
                SetTop.PostMessage(JIYUhandle, WM_SYSCOMMAND, SC_MINIMIZE, 0);
                SetTop.SendMessage(JIYUhandle, WM_SYSCOMMAND, SC_MINIMIZE, 0);
            }
        }

        private void TopTime_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (isWindowLoaded)
            {
                topinterval = (int)TopTime.Value;
                TopTimeFound.Content = "置顶时间： " + topinterval.ToString();
            }
        }

        private void StartTop_Toggled(object sender, RoutedEventArgs e)
        {
            isStartTop_toggled = StartTop.IsOn;
            if (isStartTop_toggled)
            {
                timer.Interval = TimeSpan.FromSeconds((double)topinterval);
                timer.Start();
            }
            if (!isStartTop_toggled)
            {
                timer.Stop();
                StartTop.IsOn = false;
            }
        }
    }
}
