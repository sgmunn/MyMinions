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
    using MonoKit;
    using MonoKit.DataBinding;
    using MonoKit.Data;
    using MonoKit.Reactive.Disposables;
    using MonoKit.Reactive;

    // todo: notify when all animations are complete on menu items and remove from view

    public class AllMinionsViewController : TableViewController
    {
        private readonly MinionContext context;

        private readonly CompositeDisposable lifetime;

        public AllMinionsViewController(MinionContext context) : base(UITableViewStyle.Plain)
        {
            this.context = context;
            this.lifetime = new CompositeDisposable();

            this.Title = "all minions";
            var s = new TableViewSection(this.Source);
        }

        public override void LoadView()
        {
            base.LoadView();

            this.View.Layer.CornerRadius = 10;
        }
        
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            
            var bus = this.context.EventBus;
            this.lifetime.Add(bus.ObserveOnMainThread().Subscribe(this.OnNextReadModel));

            var repo = DB.Main.NewMinionRepository();
            var allMinions = repo.GetAll();
            
            var section1 = ((TableViewSection)this.Source.SectionAt(0));
            
            foreach (var minion in allMinions)
            {
                section1.Add(new StringElement(minion, new Binding("MinionName")) { Command = this.NavigateToMinion, });
            }
        }
        
        public override void ViewDidUnload()
        {
            // this can happen if the view gets unloaded with a memory warning and we'll end up with
            // multiple subscriptions to the domain model when we register in view did load
            this.lifetime.Clear();

            base.ViewDidUnload();

            var section1 = ((TableViewSection)this.Source.SectionAt(0));
            if (section1.Count > 0)
            {
                section1.Clear();
            }
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

        private void OnNextReadModel(IDataModelEvent readModel)
        {
            var dataModel = readModel as DataModelChange;
            if (dataModel != null && dataModel.Item is MinionContract)
            {
                this.MinionUpdated((MinionContract)dataModel.Item);
            }
        }

        
        private void MinionUpdated(MinionContract minion)
        {
            if (minion.Deleted)
            {
                return;
            }
            
            var section1 = ((TableViewSection)this.Source.SectionAt(0));
            
            // could be added, changed or deleted
            var element = section1.FirstOrDefault(x => ((MinionContract)x.Data).Id == minion.Id);
            
            if (element != null)
            {
                element.Data = minion;
            }
            else
            {
                // the minion is a new one
                // also Version == 1
                section1.Insert(0, new StringElement(minion, new Binding("MinionName")) { Command = this.NavigateToMinion, });
                //this.NavigateToMinion(minion);
            }
        }

    }
}
