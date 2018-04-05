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
    }

    private void button1_Click(object sender, EventArgs e) {
      Console.WriteLine("Throw test exception");
      throw new Exception("Test Exception");
    }

    private void button2_Click(object sender, EventArgs e) {
      ParameterizedThreadStart threadDelegate = new ParameterizedThreadStart(ThreadMethod);
      Thread thread = new Thread(() => {
        Console.WriteLine("Starting thread");
        throw new Exception("Thread exception");
      });
      Action<Exception> handlerAction = exception => {
        Console.WriteLine(String.Format("Handling Exception: {0}", exception));
        throw exception;
      };
      try {
        thread.Start(handlerAction);
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
  }
}
