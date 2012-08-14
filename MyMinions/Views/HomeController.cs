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

    public class HomeController : MinionPanoramaViewController
    {

        public HomeController() : base()
        {
            this.Title = "my minions";
            this.AddController(new AllMinionsViewController(), 0);
            this.PreviewSize = 0;
        }

        public override void LoadView()
        {
            base.LoadView();
        }
        
        protected override IEnumerable<MenuItem> GetMenuItems()
        {
            var storyMenuItemImage = UIImage.FromFile("Images/bg-menuitem.png");
            var storyMenuItemImagePressed = UIImage.FromFile("Images/bg-menuitem-highlighted.png");
    
            var starImage = UIImage.FromFile("Images/icon-star.png");

            var starMenuItem1 = new MenuItem(storyMenuItemImage, storyMenuItemImagePressed, starImage);
            var starMenuItem2 = new MenuItem(storyMenuItemImage, storyMenuItemImagePressed, starImage);
            var starMenuItem3 = new MenuItem(storyMenuItemImage, storyMenuItemImagePressed, starImage);

            return new [] { starMenuItem1, starMenuItem2, starMenuItem3 };
        }

        protected override void HandleMenuItemSelected (object sender, MenuItemSelectedEventArgs e)
        {
            switch (e.Selected)
            {
                case 0: 
                    this.NavigateToHireController();
                    break;
                case 1: 
                    this.NavigateToFireController();
                    break;
            }
        }

        private void NavigateToHireController()
        {
            this.Present(new HireController());
            // todo: set menu for hiring
        }

        private void NavigateToFireController()
        {
            this.Present(new FireController());
            // todo: set menu for firing
        }

    }

}
