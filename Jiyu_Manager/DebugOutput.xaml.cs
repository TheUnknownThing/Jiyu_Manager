using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Jiyu_Manager
{
    /// <summary>
    /// DebugOutput.xaml 的交互逻辑
    /// </summary>
    public partial class DebugOutput : Window
    {
        public static DebugOutput debugOutput = null;
        public DebugOutput()
        {
            InitializeComponent();
            debugOutput = this;
        }
        public void UpdateTextBox(string newData)
        { this.DebugOutputTextbox.Text += newData; }
    }
}
