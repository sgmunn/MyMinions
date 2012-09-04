//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="AllMinionsViewController.cs" company="sgmunn">
//    (c) sgmunn 2012  
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------
using MonoKit.Reactive.Linq;

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
    using MonoTouch.Foundation;

    // todo: notify when all animations are complete on menu items and remove from view
    
    public class SettingsSource : TableViewSource
    {
        public override UITableViewCellEditingStyle EditingStyleForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return UITableViewCellEditingStyle.Delete;
        }
    }

    public class AllMinionsViewController : TableViewController
    {
        private readonly MinionContext context;
        
        private readonly CompositeDisposable lifetime;
        
        private Element navigateToMinion;

        private bool allowDelete;
        
        public AllMinionsViewController(MinionContext context, string title, bool allowDelete) 
            : base(UITableViewStyle.Plain, new SettingsSource())
        {
            this.context = context;
            this.lifetime = new CompositeDisposable();

            this.NavigationItem.RightBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Done);
            this.NavigationItem.RightBarButtonItem.Clicked += DoneClicked;

            this.allowDelete = allowDelete;
            this.Title = title;
            var s = new TableViewSection(this.Source);

         //   this.TableView.SetEditing(true, true);
        }

        protected CompositeDisposable Lifetime
        {
            get
            {
                return this.lifetime;
            }
        }
        
        public override void LoadView()
        {
            base.LoadView();

            if (!this.allowDelete)
            {
                this.View.Layer.CornerRadius = 10;
            }
        }
        
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            
            var bus = this.context.EventBus;
            this.lifetime.Add(bus.ObserveOnMainThread().Subscribe(this.OnNextReadModel));
            
            var repo = DB.Main.NewMinionRepository();
            var allMinions = repo.GetAll().OrderBy(x => x.MinionName);
            
            var section1 = ((TableViewSection)this.Source.SectionAt(0));
            
            foreach (var minion in allMinions)
            {
                section1.Add(this.LoadMinion(minion));
            }
        }

        protected Element LoadMinion(MinionContract minion)
        {
            if (this.allowDelete)
            {
                return new StringElement(minion, new Binding("MinionName")) 
                { 
                    Edit = this.DeleteMinion,
                };
            }
            else
            {
                return new StringElement(minion, new Binding("MinionName")) 
                { 
                    Command = this.NavigateToMinion,
                    CanEdit = x => false,
                };
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
        
        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            
            if (this.navigateToMinion != null)
            {
                this.PerformSelector(new MonoTouch.ObjCRuntime.Selector("navigateToNewMinion"), this, 0);
            }
        }
        
        [Export("navigateToNewMinion")]
        protected void NavigateToNewMinion()
        {
            this.NavigateToMinion(this.navigateToMinion);
        }
        
        private void DoneClicked (object sender, EventArgs e)
        {
            this.DismissModalViewControllerAnimated(true);
        }

        private void NavigateToMinion(Element element)
        {
            this.navigateToMinion = null;
            
            var minion = (MinionContract)element.Data;
            var p = this.ParentViewController as UIPanoramaViewController;
            p.Present(new MinionController(this.context, minion));
        }
        
        private void DeleteMinion(Element element)
        {
            var minion = (MinionContract)element.Data;
            var subscription = Observable.Start(
                () => 
                {
                var cmd = this.context.NewCommandExecutor<MinionAggregate>();
                
                cmd.Execute(
                    new MonoKit.Domain.IAggregateCommand [] {
                    new DeleteCommand { AggregateId = minion.Identity, },
                });
            }).Subscribe();
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
            var section1 = ((TableViewSection)this.Source.SectionAt(0));
            
            // could be added, changed or deleted
            var element = section1.FirstOrDefault(x => ((MinionContract)x.Data).Id == minion.Id);

            // because we're using a snapshot based aggregate over an event sourced one
            // we will get a read model event when we delete the minion.
            // in the table where the minion was deleted it will already have been deleted
            // but in other tables it will exist and needs to be removed.
            if (minion.Deleted)
            {
                if (element != null)
                {
                    section1.Remove(element);
                }

                return;
            }

            // now update or insert the minion
            if (element != null)
            {
                // todo: rearrange based on minion name
                element.Data = minion;
            }
            else
            {
                // todo: better merging of new minions
                // the minion is a new one
                // also Version == 1
                var minionElement = this.LoadMinion(minion);
                section1.Insert(0, minionElement);
                this.navigateToMinion = minionElement;
            }
        }
    }
}
