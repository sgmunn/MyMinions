//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="AppDelegate.cs" company="sgmunn">
//    (c) sgmunn 2012  
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------
namespace MyMinions
{
    using System;
    using MonoTouch.Foundation;
    using MonoTouch.UIKit;
    using MyMinions.Views;
    using MyMinions.Domain;

    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register ("AppDelegate")]
    public partial class AppDelegate : UIApplicationDelegate
    {
        private MinionContext context;
        private UIWindow window;
        private UINavigationController navController;

        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            this.window = new UIWindow(UIScreen.MainScreen.Bounds);

            this.window.RootViewController = new HomeController();
            this.window.MakeKeyAndVisible();

            return true;
        }
    }
}

