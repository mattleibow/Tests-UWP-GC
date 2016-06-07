using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Garbage
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private static LifetimeTester global;
        private LifetimeTester instance;

        public MainPage()
        {
            this.InitializeComponent();

            DoWork();
        }

        private void DoWork()
        {
            global = new LifetimeTester("GLOBAL", Log);
            instance = new LifetimeTester("CLASS ", Log);
            var localWithNothing = new LifetimeTester("LOCAL ", Log);
            var localWithReference = new LifetimeTester("REF   ", Log);

            SleepAndCollect("after construction");

            CallMethod(localWithNothing);

            SleepAndCollect("after localWithNothing");

            CallMethod(localWithReference);

            SleepAndCollect("after localWithReference");

            var testing = localWithReference.ToString();

            SleepAndCollect("reference: " + testing);

            SleepAndCollect("end");
        }

        private void CallMethod(LifetimeTester value)
        {
            var label = value.Label;
            SleepAndCollect($"    after start CallMethod({label})");

            value.Reference();

            NativeCode(value.Label, new WeakReference(value));

            SleepAndCollect($"    after end CallMethod({label})");
        }

        private void NativeCode(string label, WeakReference weak)
        {
            SleepAndCollect($"        starting NativeCode({label}) [StillAlive={weak.Target != null}]");
            SleepAndCollect($"        NativeCode({label}) processing [StillAlive={weak.Target != null}]");
        }

        private void SleepAndCollect(string message)
        {
            Task.Delay(1).Wait();
            GC.Collect();
            Log("*GC   ", message);
        }

        private void Log(string label, string message)
        {
            var thread = GetCurrentThreadId();
            Dispatcher.RunAsync(CoreDispatcherPriority.High, () =>
            {
                textBox.Text += $"[{thread}] {label}: {message}{Environment.NewLine}";
            });
        }

        [DllImport("Kernel32.dll")]
        public static extern uint GetCurrentThreadId();
    }

    public class LifetimeTester : IDisposable
    {
        private bool disposedValue = false;
        private Action<string, string> logger;
        public string Label { get; private set; }

        public LifetimeTester(string label, Action<string, string> logger)
        {
            this.logger = logger;
            Label = label;

            logger(Label, "    new LifetimeTester()");
        }

        protected virtual void Dispose(bool disposing)
        {
            logger(Label, "    Dispose(bool)");

            if (!disposedValue)
            {
                if (disposing)
                {
                }

                disposedValue = true;
            }
        }

        ~LifetimeTester()
        {
            logger(Label, "    ~LifetimeTester()");

            Dispose(false);
        }

        public void Dispose()
        {
            logger(Label, "    Dispose()");

            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Reference()
        {
            logger(Label, "    Reference()");
        }
    }
}
