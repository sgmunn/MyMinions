//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="MinionPanoramaViewController.cs" company="sgmunn">
//    (c) sgmunn 2012  
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace MyMinions.Views
{
    using System;
    using MonoKit.Metro;
    using MonoTouch.UIKit;
    using MonoKit.UI.AwesomeMenu;
    using System.Drawing;
    using System.Collections.Generic;
    using System.Linq;

    // todo: notify when all animations are complete on menu items and remove from view

    public class MinionPanoramaViewController : UIPanoramaViewController
    {
        public MinionPanoramaViewController()
        {
            this.TextColor = UIColor.FromRGB(229, 126, 34);
            this.AnimateTitle = false;
//            this.ShowTitle = false;
//            this.ShowHeaders = false;
//            this.ShadowEnabled = false;
            //this.BottomMargin = 60;
//            this.Margin = 0;

        }

        public Menu Menu { get; private set; }

        public override void LoadView()
        {
            base.LoadView();

            this.BackgroundView.BackgroundColor = UIColor.GroupTableViewBackgroundColor;

            var menuItems = this.GetMenuItems().ToList();

            if (menuItems.Any())
            {

            this.Menu = new Menu(this.View.Bounds, menuItems);
            this.ContentView.AddSubview(this.Menu);
            this.Menu.MenuItemSelected += HandleMenuItemSelected;
                this.Menu.Alpha = 0.7f;
            }
        }

        protected virtual IEnumerable<MenuItem> GetMenuItems()
        {
            yield break;
        }

        protected virtual void HandleMenuItemSelected (object sender, MenuItemSelectedEventArgs e)
        {
            // todo: delay navigate to coincide with animation of menu a little
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            if (this.Menu != null)
            {
                this.Menu.StartPoint = new PointF(30, this.View.Bounds.Height - 30);
                this.Menu.FarRadius = 250;
                this.Menu.EndRadius = 220;
                this.Menu.NearRadius = 210;
                this.Menu.CloseRotation = 0f;
                this.Menu.ExpandRotation = 0f;
                this.Menu.RadiusStep = 70;
                this.Menu.Mode = LayoutMode.Horizontal;
                this.ContentView.BringSubviewToFront(this.Menu);
            }
        }
    }
}
