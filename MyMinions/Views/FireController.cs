//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="FireController.cs" company="sgmunn">
//    (c) sgmunn 2012  
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

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
    using MyMinions.Domain;

    // todo: notify when all animations are complete on menu items and remove from view
    
    public class FireController : UIBackButtonPanoramaViewController
    {
        private readonly MinionContext context;

        public FireController(MinionContext context) : base()
        {
            this.context = context;
            this.Title = "fire minions";

            this.AddController(new AllMinionsViewController(this.context, "all minions", true), 0);
            this.PreviewSize = 0;
        }
    }

}
