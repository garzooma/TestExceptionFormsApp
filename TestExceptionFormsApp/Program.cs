using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestExceptionFormsApp
{
  static class Program
  {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
      Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
      Application.ThreadException += GUIErrorHandler;

      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run(new Form1());
    }

    private static void GUIErrorHandler(object sender, ThreadExceptionEventArgs e)
    {
      MessageBox.Show(String.Format("Exception: {0}", e.Exception));
    }
  }
}
