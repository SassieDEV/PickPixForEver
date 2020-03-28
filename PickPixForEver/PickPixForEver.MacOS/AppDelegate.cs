using AppKit;
using Foundation;
using Xamarin.Forms;
using Xamarin.Forms.Platform.MacOS;

namespace PickPixForEver.MacOS
{
    [Register("AppDelegate")]
    public class AppDelegate : FormsApplicationDelegate
    {
        NSWindow window;

        public override NSWindow MainWindow => window;

        public AppDelegate()
        {
            var style = NSWindowStyle.Closable | NSWindowStyle.Resizable | NSWindowStyle.Titled;
            var rect = new CoreGraphics.CGRect(100, 100, 1024, 768);

            window = new NSWindow(rect, style, NSBackingStore.Buffered, false);
            window.Title = "PickPixForEver";
            window.TitleVisibility = NSWindowTitleVisibility.Hidden;
        }

        public override void DidFinishLaunching(NSNotification notification)
        {
            Forms.Init();

            //SQLitePCL.Batteries.Init();
            //var libPath = Path.Combine(
            //    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            //    , "..", "Library", "data");
            //if (!Directory.Exists(libPath))
            //{
            //    Directory.CreateDirectory(libPath);
            //}
            //var dbPath = Path.Combine(libPath, "PickPixForever.db");

            LoadApplication(new App(""));
            base.DidFinishLaunching(notification);
        }

        public override void WillTerminate(NSNotification notification)
        {
            // Insert code here to tear down your application
        }
    }
}
