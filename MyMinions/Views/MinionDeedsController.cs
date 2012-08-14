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
    
    public class MinionDeedsController : TableViewController
    {
        public MinionDeedsController() : base(UITableViewStyle.Plain)
        {
            this.Title = "deeds";
        }

        public override void LoadView()
        {
            base.LoadView();

            var section1 = new TableViewSection(this.Source);
            section1.Add(new StringElement("deed 1") { Command = this.NavigateToMinion });
            section1.Add(new StringElement("deed 2") { Command = this.NavigateToMinion });
            section1.Add(new StringElement("deed 3") { Command = this.NavigateToMinion });
        }

        private void NavigateToMinion(Element element)
        {
            //var p = this.ParentViewController.ParentViewController as UIPanoramaViewController;
            //p.Present(new MinionController());
            //p.Dismiss();
        }
    }

}
