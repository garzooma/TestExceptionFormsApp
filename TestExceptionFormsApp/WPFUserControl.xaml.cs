using GalaSoft.MvvmLight.Messaging;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TestExceptionFormsApp
{
    /// <summary>
    /// Interaction logic for WPFUserControl.xaml
    /// </summary>
    public partial class WPFUserControl : UserControl
    {
        public WPFUserControl()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bool main = Program.IsMainThread();
            throw new Exception("Exception in WPF UC");
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //this.Dispatcher.UnhandledException += Dispatcher_UnhandledException;
        }

        private void Dispatcher_UnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            bool main = Program.IsMainThread();
            e.Handled = true;
            Messenger.Default.Send<MessageCommunicator>(new MessageCommunicator() { Message = "xThrow" });
            //throw new Exception("Handler exception");
            ExceptionEvent(new Exception("Not thrown"));
        }

        public delegate void ExceptionEventHandler(Exception excp);

        public event ExceptionEventHandler ExceptionEvent;

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
           this.Dispatcher.UnhandledException += Dispatcher_UnhandledException;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.UnhandledException -= Dispatcher_UnhandledException;
        }
    }
}
