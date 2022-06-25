using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Jiyu_Manager.Jiyu_Manager_Class;

namespace Jiyu_Manager
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private System.Threading.Timer aTimer;
        public MainWindow()
        {
            InitializeComponent();
            SetTimer();
        }
        private void SetTimer()
        {
            aTimer = new System.Threading.Timer(TimerEvent, null, Timeout.Infinite, Timeout.Infinite);
        }
        private void TimerEvent(Object state)
        {
            //开始置顶
            aTimer.Change(1000, Timeout.Infinite);
            TopMostClass.TopMostMainWindow();
        }
        private async void StartDetectionToggle_Toggled(object sender, RoutedEventArgs e)
        {
            StartTop.IsOn = true;
            await RegGACAssemblyAsync();
            JiyuManagerHookClass.InstallHookInternal();
        }
        
        private async static Task<bool> RegGACAssemblyAsync()
        {
            // 在Easyhook 2.7版本中，不需要Register GAC，详见 https://stackoverflow.com/questions/31762879/easyhook-the-given-32-bit-library-does-not-exist-user-library-does-not-export
            try
            {
                JiyuManagerHookClass.RegHookerGACAssembly();
                JiyuManagerHookClass.RegEasyhookGACAssembly();
            }
            catch (Exception ex)
            {
                MessageBox.Show("RegGACAssembly " + ex.ToString());
            }
            return true;
        }

        private void AvoidQuizToggle_Toggled(object sender, RoutedEventArgs e)
        {

        }

        private void StartTop_Toggled(object sender, RoutedEventArgs e)
        {
            switch (StartTop.IsOn)
            {
                case true:
                    aTimer.Change(0, Timeout.Infinite);
                    break;
                case false:
                    aTimer.Change(Timeout.Infinite, Timeout.Infinite);
                    break;
            }
            TopMostClass.SetWindowNoFocus();

        }

        private void TaskkillJiyu_Click(object sender, RoutedEventArgs e)
        {
            RunCommand.RunCmd("taskkill /im studentmain.exe /f");
        }

        private void NTSDkillJiyu_Click(object sender, RoutedEventArgs e)
        {

        }

        private void OpenAboutMenu_Click(object sender, RoutedEventArgs e)
        {
            About K = new About();
            K.Show();
        }
    }
}
