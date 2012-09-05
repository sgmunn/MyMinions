//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="HomeController.cs" company="sgmunn">
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
    using MyMinions.Domain;
    using MyMinions.Domain.Data;
    using MonoKit.Domain;
    using MonoKit.Data.SQLite;

    // todo: notify when all animations are complete on menu items and remove from view

    public class HomeController : MinionPanoramaViewController
    {
        private readonly MinionContext context;

        public HomeController() : base()
        {
            this.context = this.InitializeContext();

            this.Title = "my minions";
            this.AddController(new AllMinionsViewController(this.context, "all minions", false), 0);
            this.PreviewSize = 0;
        }

        public override void LoadView()
        {
            base.LoadView();
        }

        protected override IEnumerable<MenuItem> GetMenuItems()
        {
            // todo: need specific images for menu
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
            var minion = new MinionContract { MinionName = "Minion", };
            var nav = new UINavigationController(new MinionEditController(this.context, null));
            
            this.PresentModalViewController(nav, true);
        }

        private void NavigateToFireController()
        {
            var minion = new MinionContract { MinionName = "Minion", };
            var nav = new UINavigationController(new AllMinionsViewController(this.context, "Fire Minions", true));

            this.PresentModalViewController(nav, true);
//            this.Present(new FireController(this.context));
            // todo: set menu for firing
        }

        private MinionContext InitializeContext()
        {
            // lazy static constructor for the DB will not get executed until this point, out of the FinishedLoading
            // allowing the application to respond as quick as it can.
            var minionContext = new MinionContext(DB.Main, new ObservableDomainEventBus());

            var minionRepo = new SqlRepository<MinionContract>(DB.Main);
            var allMinions = minionRepo.GetAll();

            if (!allMinions.Any())
            {
                // bootstrap us some
                var cmd = minionContext.NewCommandExecutor<MinionAggregate>();
                cmd.Execute(new ChangeNameCommand {AggregateId = MinionId.NewId(), Name = "Minion 1" });
                cmd.Execute(new ChangeNameCommand {AggregateId = MinionId.NewId(), Name = "Minion 2" });
                cmd.Execute(new ChangeNameCommand {AggregateId = MinionId.NewId(), Name = "Minion 3" });
            }

            return minionContext;
        }
    }
}
