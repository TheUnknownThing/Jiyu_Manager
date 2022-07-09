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
using ModernWpf.Controls;
using ModernWpf.Controls.Primitives;

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
            ShowDevDialog();   
        }
        private async Task ShowDevDialog()
        {
            ContentDialog DevDialog = new ContentDialog
            {
                Title = "Welcome",
                Content = "Hi. This software is currently under development, so the features are UNstable."+"\nUse it AT YOUR OWN RISK. \nAlso, you can find the Github link in the \"About\" Menu, hope you can join my development!",
                CloseButtonText = "Okay"
            };

            ContentDialogResult result = await DevDialog.ShowAsync();
        }
        private void SetTimer()
        {
            aTimer = new System.Threading.Timer(TimerEvent, null, Timeout.Infinite, Timeout.Infinite);
        }
        private void TimerEvent(Object state)
        {
            //开始置顶
            aTimer.Change(750, Timeout.Infinite);
            TopMostClass.TopMostMainWindow();
            TopMostClass.SetWindowNoFocus();
            TopMostClass.setForegroundWindow();
        }
        private async void StartDetectionToggle_Toggled(object sender, RoutedEventArgs e)
        {
            if (ProgramInf.isDevFeatureEnabled && ShowDebug.IsOn == true)
            {
                DebugOutput k = new DebugOutput();
                k.Show();
            }
            StartTop.IsOn = true;
            await RemoveGACAssemblyAsync();
            await JiyuManagerHookClass.InstallHookInternal();
        }
        
        private async static Task<bool> RegGACAssemblyAsync()
        {
            //千万别用，在Easyhook 2.7版本中，不需要Register GAC，详见 https://stackoverflow.com/questions/31762879/easyhook-the-given-32-bit-library-does-not-exist-user-library-does-not-export
            JiyuManagerHookClass.RegHookerGACAssembly();
            JiyuManagerHookClass.RegEasyhookGACAssembly();
            return true;
        }
        private async static Task<bool> RemoveGACAssemblyAsync()
        {
            var dllName = "Jiyu_Hooker.dll";
            var dllPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dllName);
            new System.EnterpriseServices.Internal.Publish().GacRemove(dllPath);
            dllName = "Easyhook.dll";
            dllPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dllName);
            new System.EnterpriseServices.Internal.Publish().GacRemove(dllPath);
            dllName = "EasyLoad32.dll";
            dllPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dllName);
            new System.EnterpriseServices.Internal.Publish().GacRemove(dllPath);
            dllName = "EasyLoad64.dll";
            dllPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dllName);
            new System.EnterpriseServices.Internal.Publish().GacRemove(dllPath);
            return true;
        }
        private void AvoidQuizToggle_Toggled(object sender, RoutedEventArgs e)
        {

        }

        private void StartTop_Toggled(object sender, RoutedEventArgs e)
        {
            ProgramInf.GetProgramProcessID();
            switch (StartTop.IsOn)
            {
                case true:
                    aTimer.Change(0, Timeout.Infinite);
                    break;
                case false:
                    aTimer.Change(Timeout.Infinite, Timeout.Infinite);
                    break;
            }

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


        private void DebugOptionMenu_Click(object sender, RoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
        }

        private void DangerZoneMenu_Click(object sender, RoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
        }

        private void KillJiyu_Click(object sender, RoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
        }
    }
}
