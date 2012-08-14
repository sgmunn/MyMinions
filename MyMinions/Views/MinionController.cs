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

    public class MinionController : UIBackButtonPanoramaViewController
    {
        public MinionController() : base()
        {
            this.Title = "Scarlett";
            this.AddController(new TodayDeedsViewController(), 0);

            // todo: have an event to show navigated to in order to hook this up later to improve animation speed
            this.AddController(new MinionCashController(), 0);
            this.AddController(new MinionDeedsController(), 0);
        }

        public override void LoadView()
        {
            base.LoadView();

            this.BackgroundView.BackgroundColor = UIColor.Clear;
        }
        
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            if (this.Menu != null)
            {
                //this.Menu.StartPoint = new PointF(30, this.View.Bounds.Height - 30);
                //this.Menu.FarRadius = 250;
                //this.Menu.EndRadius = 220;
                //this.Menu.NearRadius = 210;
                //this.Menu.CloseRotation = 0f;
                //this.Menu.ExpandRotation = 0f;
                this.Menu.RadiusStep = 50;
                //this.Menu.Mode = LayoutMode.Horizontal;
                //this.ContentView.BringSubviewToFront(this.Menu);
            }
        }

        protected override IEnumerable<MenuItem> GetMenuItems()
        {
            var storyMenuItemImage = UIImage.FromFile("Images/bg-menuitem.png");
            var storyMenuItemImagePressed = UIImage.FromFile("Images/bg-menuitem-highlighted.png");
    
            var starImage = UIImage.FromFile("Images/icon-star.png");

            var starMenuItem1 = new MenuItem(storyMenuItemImage, storyMenuItemImagePressed, starImage);
            var starMenuItem2 = new MenuItem(storyMenuItemImage, storyMenuItemImagePressed, starImage);
            var starMenuItem3 = new MenuItem(storyMenuItemImage, storyMenuItemImagePressed, starImage);
            var starMenuItem4 = new MenuItem(storyMenuItemImage, storyMenuItemImagePressed, starImage);

            return new [] { starMenuItem1, starMenuItem2, starMenuItem3, starMenuItem4 };
        }

        protected override void HandleMenuItemSelected (object sender, MenuItemSelectedEventArgs e)
        {
            switch (e.Selected)
            {
                case 0: 
                    this.NavigateToEarnController();
                    break;
                case 1: 
                    this.NavigateToSpendController();
                    break;
                case 2: 
                    this.NavigateToReviewController();
                    break;
                case 3: 
                    this.NavigateToEditController();
                    break;
            }
        }

        private void NavigateToEarnController()
        {
            this.Present(new EarnController());
        }

        private void NavigateToSpendController()
        {
            this.Present(new SpendController());
        }

        private void NavigateToReviewController()
        {
            this.Present(new ReviewController());
        }

        private void NavigateToEditController()
        {
            this.Present(new EditController());
        }

    }

}
