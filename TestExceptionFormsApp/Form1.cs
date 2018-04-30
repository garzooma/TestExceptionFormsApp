using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestExceptionFormsApp
{
  public partial class Form1 : Form {
    public Form1() {
        InitializeComponent();

        Messenger.Default.Register<MessageCommunicator>(this, OnMessageReceived);

            wpfUserControl1.ExceptionEvent += WpfUserControl1_ExceptionEvent;

    }

        private void WpfUserControl1_ExceptionEvent(Exception excp)
        {
            //throw new Exception("Exception from wpf uc");
        }

        private void OnMessageReceived(MessageCommunicator msgComm)
    {
        if (msgComm.Message == "Throw")
        {
            throw new Exception("Exception from message");
        }
    }

    private void button1_Click(object sender, EventArgs e) {
      Console.WriteLine("Throw test exception");
      throw new Exception("Test Exception");
    }

    private void button2_Click(object sender, EventArgs e) {
      Thread thread = new Thread(() => {
        Console.WriteLine("Starting thread");
        throw new Exception("Thread exception");
      });

      try {
        thread.Start();
       }
       catch (Exception excp) {
        Console.Write("Caught thread exception");
      }
    }


    private void ThreadMethod(Object obj) {
      try {
        Console.WriteLine("Starting thread");
        throw new Exception("Thread exception");
      }
      catch (Exception excp) {
        Action<Exception> handlerAction = obj as Action<Exception>;
        handlerAction.Invoke(excp);
      }
    }

        private void button3_Click(object sender, EventArgs e)
        {
            ParameterizedThreadStart threadDelegate = new ParameterizedThreadStart(ThreadMethod);
            ExceptionHandler handler = new ExceptionHandler();
            Thread thread = new Thread(() => RealStart(handler));
            try
            {
                //thread.Start(handlerAction);
                thread.Start();
            }
            catch (Exception excp)
            {
                Console.Write("Caught thread exception");
            }

        }

        private static void RealStart(ExceptionHandler param1)
        {
            try { 
            throw new Exception("RealStart excp");
            }
            catch (Exception excp)
            {
                param1.Handle(excp);
            }
        }

        public class ExceptionHandler
        {
            public void Handle(Exception excp)
            {
                //throw new Exception("Handled");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            BackgroundWorker bw = new BackgroundWorker();
            bw.RunWorkerCompleted += Bw_RunWorkerCompleted;
            bw.DoWork += Bw_DoWork;
            bw.RunWorkerAsync(this);
        }

        private void Bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Bw_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bw = sender as BackgroundWorker;
            bw.CancelAsync();
            // e.Cancel = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Tick += tickMethod;
            timer.Interval = 5 * 1000;
            timer.Start();

        }

        private void tickMethod(object sender, EventArgs e)
        {
            bool isMainThread = Program.IsMainThread();
            System.Windows.Forms.Timer timer = sender as System.Windows.Forms.Timer;
            if (timer != null)
            {
                timer.Stop();
            }
            else
            {
                System.Windows.Threading.DispatcherTimer dtimer = sender as System.Windows.Threading.DispatcherTimer;
                dtimer.Stop();
            }
            throw new NotImplementedException();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
            timer.Dispatcher.ShutdownStarted += Dispatcher_ShutdownStarted;
            timer.Dispatcher.UnhandledException += Dispatcher_UnhandledException;
            timer.Tick += dispatchMethod;
            timer.Interval = new TimeSpan(0, 0, 0, 0, 5 * 1000);
            timer.Start();
        }

        private void Dispatcher_ShutdownStarted(object sender, EventArgs e)
        {
            throw new Exception("Timer Exception");
        }

        private void dispatchMethod(object sender, EventArgs e)
        {
            System.Windows.Threading.DispatcherTimer dtimer = sender as System.Windows.Threading.DispatcherTimer;
            dtimer.Stop();
            try
            {
                throw new NotImplementedException();
            }
            catch(Exception excp)
            {
                dtimer.Dispatcher.InvokeShutdown();
            }
        }

        private void Dispatcher_UnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            throw new NotImplementedException();
        }
    }

  public class MessageCommunicator
    {
        public string Message { get; set; }
    }
}
