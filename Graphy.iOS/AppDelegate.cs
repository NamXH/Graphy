using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Graphy.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : UIApplicationDelegate
    {
        // class-level declarations
        UIWindow m_window;

        // This method is invoked when the application has loaded and is ready to run. In this
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            // create a new window instance based on the screen size
            m_window = new UIWindow(UIScreen.MainScreen.Bounds);

            var rootNavigationController = new UINavigationController();
            var mainScreen = new MainScreen();
            rootNavigationController.PushViewController(mainScreen, false);
            mainScreen.CreateTabs();

            // If you have defined a root view controller, set it here:
            m_window.RootViewController = rootNavigationController;

            // make the window visible
            m_window.MakeKeyAndVisible();

            return true;
        }
    }
}

