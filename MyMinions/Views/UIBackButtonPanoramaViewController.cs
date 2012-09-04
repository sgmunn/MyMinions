//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="UIBackButtonPanoramaViewController.cs" company="sgmunn">
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

    // todo: notify when all animations are complete on menu items and remove from view
    
    public class UIBackButtonPanoramaViewController : MinionPanoramaViewController
    {
        public UIBackButtonPanoramaViewController() : base()
        {
            this.AnimateTitle = false;
        }

        public UIButton BackView { get; private set; }

        public override void LoadView()
        {
            base.LoadView();

            this.BackView = new UIButton(UIButtonType.Custom);
            this.View.AddSubview(this.BackView);

            this.BackView.BackgroundColor = UIColor.Green;

            this.BackView.TouchUpInside += (sender, e) => {this.NavigateBack(); };

            this.BackgroundView.BackgroundColor = UIColor.Clear;
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            var size = this.TitleView.Bounds.Height - (this.Margin * 2);
            this.BackView.Frame = new System.Drawing.RectangleF(this.Margin, this.Margin, size, size);

            this.LayoutContent();
        }

        public override void Present(UIViewController controller)
        {
            base.Present(controller);

            UIView.Animate(0.4f, 0, UIViewAnimationOptions.CurveEaseInOut | UIViewAnimationOptions.BeginFromCurrentState, () =>
            {
                this.BackView.Alpha = 0f;
            
            }, () =>
            {
            });
        }

        public override void Dismiss()
        {
            base.Dismiss();

            UIView.Animate(0.4f, 0, UIViewAnimationOptions.CurveEaseInOut | UIViewAnimationOptions.BeginFromCurrentState, () =>
            {
                this.BackView.Alpha = 1f;
            
            }, () =>
            {
            });
        }

        protected override float CalculateTitleOffset(float offset)
        {
            return this.BackView.Frame.Width + (this.Margin * 2);
        }

        private void NavigateBack()
        {
            var p = this.ParentViewController as UIPanoramaViewController;
            p.Dismiss();
        }
    }

}
