//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="AppDelegate.cs" company="sgmunn">
//    (c) sgmunn 2012  
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------
namespace MyMinions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using MonoTouch.Foundation;
    using MonoTouch.UIKit;
    using System.Threading;
    using MyMinions.Views;
    using MyMinions.Domain.Data;
    using MyMinions.Domain;
    using MonoKit.Domain;

    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register ("AppDelegate")]
    public partial class AppDelegate : UIApplicationDelegate
    {
        private MinionContext context;
        private UIWindow window;
        private UINavigationController navController;
        //private MainViewController mainViewController;

        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            this.window = new UIWindow(UIScreen.MainScreen.Bounds);
            //this.navController = new UINavigationController();
            //this.navController.NavigationBarHidden = true;
            //this.window.RootViewController = this.navController;

            this.window.RootViewController = new HomeController();
            this.window.MakeKeyAndVisible();

            //var startupThread = new Thread(this.Startup);
            //startupThread.Start();

            return true;
        }

        private void Startup()
        {
            // lazy static constructor for the DB will not get executed until this point, out of the FinishedLoading
            // allowing the application to respond as quick as it can.
            this.context = new MinionContext(DB.Main, new ObservableDomainEventBus());

            this.InvokeOnMainThread(() =>
            {
//                this.mainViewController = new MainViewController(
//                    this.context, 
//                    new MinionRepository(MinionDB.Main),
//                    new TransactionRepository(MinionDB.Main)
//                    );
//                this.navController.PushViewController(this.mainViewController, false);

//                this.mainViewController.Load();
            });
        }
    }
}

