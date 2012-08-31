//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="MinionController.cs" company="sgmunn">
//    (c) sgmunn 2012  
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace MyMinions.Views
{
    using System;
    using MonoTouch.UIKit;
    using MonoKit.UI.AwesomeMenu;
    using System.Collections.Generic;
    using System.Linq;
    using MyMinions.Domain.Data;
    using MyMinions.Domain;
    using MonoKit;
    using MonoKit.Reactive.Disposables;
    using MonoKit.Data;
    using MonoKit.Reactive;

    // todo: notify when all animations are complete on menu items and remove from view

    public class MinionController : UIBackButtonPanoramaViewController
    {
        private readonly MinionContext context;

        private readonly MinionContract minion;

        private readonly CompositeDisposable lifetime;

        public MinionController(MinionContext context, MinionContract minion) : base()
        {
            this.context = context;
            this.minion = minion;
            this.lifetime = new CompositeDisposable();

            this.Load(minion);

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

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var bus = this.context.EventBus;
            this.lifetime.Add(bus.ObserveOnMainThread().Subscribe(this.OnNextReadModel));
        }
        
        public override void ViewDidUnload()
        {
            // this can happen if the view gets unloaded with a memory warning and we'll end up with
            // multiple subscriptions to the domain model when we register in view did load
            this.lifetime.Clear();
            base.ViewDidUnload();
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
            // todo: need images for menu
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
            var nav = new UINavigationController(new MinionEditController(this.context, this.minion));

            this.PresentModalViewController(nav, true);
        }

        private void Load(MinionContract minion)
        {
            this.Title = minion.MinionName;
        }

        private void OnNextReadModel(IDataModelEvent readModel)
        {
            var dataModel = readModel as DataModelChange;
            if (dataModel != null && dataModel.Item is MinionContract)
            {
                this.Load((MinionContract)dataModel.Item);
                this.LayoutContent();
            }
        }
    }
}
