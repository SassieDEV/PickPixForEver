using AppKit;
using System;
using System.IO;
using Foundation;
using Xamarin.Forms;
using Xamarin.Forms.Platform.MacOS;

namespace PickPixForEver.MacOS
{
    [Register("AppDelegate")]
    public class AppDelegate : FormsApplicationDelegate
    {
        NSWindow window;

        public override NSWindow MainWindow
        {
            get
            {
                return window;
            }
        }

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
            SQLitePCL.Batteries.Init();
            var parentFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            var dbPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/PickPixForEver";
            if (!Directory.Exists(dbPath))
            {
                Directory.CreateDirectory(dbPath);
            }
            dbPath = Path.Combine(dbPath, "PickPixForever.db");

            Forms.Init();
            LoadApplication(new App(dbPath));
            base.DidFinishLaunching(notification);
        }

        public override void WillTerminate(NSNotification notification)
        {
            // Insert code here to tear down your application
        }
    }
}
