//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file=".cs" company="sgmunn">
//    (c) sgmunn 2012  
//
//    Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
//    documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
//    the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
//    to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
//    The above copyright notice and this permission notice shall be included in all copies or substantial portions of 
//    the Software.
//
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO 
//    THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//    CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS 
//    IN THE SOFTWARE.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------
//

using MonoKit.UI.Elements;
using MonoKit.UI.AwesomeMenu;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;

namespace MyMinions.Views
{
    using System;
    using MonoKit.Metro;
    using MonoKit.UI;
    using MonoTouch.UIKit;

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
