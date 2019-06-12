using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Patterns.Structure.Observer
{
    class Ob02_WeakEvent
    {
        public static void Test()
        {
            Console.WriteLine("--------");
            var button = new Button();
            var window = new Window(button);
            var windowRef = new WeakReference(window);
            button.Fire();

            Console.WriteLine("Setting window to null");
            window = null;

            FireGC();

            Console.WriteLine($"Is the window alive after GC: {windowRef.IsAlive}");
            button.Fire();

            Console.WriteLine("--------");
            var button2 = new Button();
            var window2 = new WindowWeakEvent(button2);
            var window2Ref = new WeakReference(window2);
            button2.Fire();

            Console.WriteLine("Setting window to null");
            window2 = null;

            FireGC();

            Console.WriteLine($"Is the window alive after GC: {window2Ref.IsAlive}");
            button2.Fire();
        }

        private static void FireGC()
        {
            Console.WriteLine("Starting GC");
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            Console.WriteLine("GC is done");
        }
    }

    public class Button
    {
        public event EventHandler Clicked;

        public void Fire()
        {
            Clicked?.Invoke(this, EventArgs.Empty);
        }
    }

    public class Window
    {
        public Window(Button button)
        {
            button.Clicked += ButtonOnClicked;
        }

        private void ButtonOnClicked(object sender, EventArgs eventArgs)
        {
            Console.WriteLine("Button clicked (Window handler)");
        }

        ~Window()
        {
            Console.WriteLine("Window finalized");
        }
    }

    public class WindowWeakEvent
    {
        public WindowWeakEvent(Button button)
        {
            WeakEventManager<Button, EventArgs>
                .AddHandler(button, "Clicked", ButtonOnClicked);
        }

        private void ButtonOnClicked(object sender, EventArgs eventArgs)
        {
            Console.WriteLine("Button clicked (Window handler)");
        }

        ~WindowWeakEvent()
        {
            Console.WriteLine("Window finalized");
        }
    }
}
