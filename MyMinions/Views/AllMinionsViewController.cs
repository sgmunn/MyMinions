//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="AllMinionsViewController.cs" company="sgmunn">
//    (c) sgmunn 2012  
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace MyMinions.Views
{
    using System;
    using MonoKit.Metro;
    using MonoKit.UI;
    using MonoTouch.UIKit;
    using MonoKit.UI.Elements;
    using System.Linq;
    using MyMinions.Domain;
    using MyMinions.Domain.Data;
    using MonoKit.DataBinding;

    // todo: notify when all animations are complete on menu items and remove from view

    public class AllMinionsViewController : TableViewController
    {
        private readonly MinionContext context;

        public AllMinionsViewController(MinionContext context) : base(UITableViewStyle.Plain)
        {
            this.context = context;

            this.Title = "all minions";
        }

        public override void LoadView()
        {
            base.LoadView();

            var repo = DB.Main.NewMinionRepository();
            var allMinions = repo.GetAll();

            var section1 = new TableViewSection(this.Source);

            foreach (var minion in allMinions)
            {
                section1.Add(new StringElement(minion, new Binding("MinionName")) { Command = this.NavigateToMinion, });
            }

            this.View.Layer.CornerRadius = 10;
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

        }

        private void NavigateToMinion(Element element)
        {
            var minion = (MinionContract)element.Data;
            var p = this.ParentViewController as UIPanoramaViewController;
            p.Present(new MinionController(this.context, minion));
        }
    }
}
