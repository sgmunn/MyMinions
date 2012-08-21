using System;
using MonoKit.UI;
using MonoKit.UI.Elements;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using MyMinions.Domain.Data;
using System.Collections.Generic;
using MonoKit.DataBinding;
using System.Linq;
using MonoKit.Domain;
using MonoKit.Reactive;
using MonoKit.Reactive.Disposables;
using MyMinions.Domain;
using MonoKit;
using MonoKit.Reactive.Linq;
using MonoKit.Data;

namespace MyMinions.UI
{
    public class SettingsViewController : TableViewController
    {
        private readonly IDomainContext context;

        private readonly IRepository<MinionContract> repository;

        private readonly CompositeDisposable lifetime;

        private readonly ICommandExecutor<MinionAggregate> commandExecutor;

        public SettingsViewController(IDomainContext context, IRepository<MinionContract> repository) : base(UITableViewStyle.Plain, new SettingsSource())
        {
            this.context = context;
            this.repository = repository;
            this.lifetime = new CompositeDisposable();
            this.commandExecutor = context.NewCommandExecutor<MinionAggregate>();

            this.NavigationItem.Title = "Settings";
            this.NavigationItem.RightBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Edit, this.Edit);
            this.NavigationItem.RightBarButtonItem.Enabled = false;
        }
        
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // todo: implement .Where so that we can subscribe to the events we want to know about
            var bus = this.context.EventBus;
            this.lifetime.Add(bus.ObserveOnMainThread().Subscribe(this.OnNextReadModel));

            if (this.Source.Count == 0)
            {
                new TableViewSection(this.Source) { AllowMoveItems = false };
            }

            this.LoadMinionsAsync();
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

            // hack: don't like this much, kinda ugly
            this.NavigationController.NavigationBarHidden = false;
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);

            // the view is being *re*moved from it's parent contoller
            // so we should clear the lifetime
            if (this.IsMovingFromParentViewController)
            {
                this.lifetime.Clear();
            }
        }

        private void LoadMinionsAsync()
        {
            this.NavigationItem.RightBarButtonItem.Enabled = false;

            // create a task to load the minions.  Observe the results of the task on
            // the main thread during which we'll load them into the table and 
            // enable the edit button
            var subscription = Observable.Start<IEnumerable<MinionContract>>(
                () =>
                {
                    return this.repository.GetAll().OrderBy(x => x.MinionName);
                })
                .ObserveOnMainThread().Subscribe((minions) =>
                    {
                        this.NavigationItem.RightBarButtonItem.Enabled = false;
                        this.LoadMinions(minions);
                    });

            // make sure to add to lifetime so that if we navigate away 
            this.lifetime.Add(subscription);
        }

        private void LoadMinions(IEnumerable<MinionContract> minions)
        {
            var section1 = ((TableViewSection)this.Source.SectionAt(0));
                
            section1.BeginUpdate();
            section1.Clear();
            foreach (var minion in minions)
            {
                section1.Add(new DisclosureElement(minion, new Binding("MinionName")) { Command = this.NavigateToMinion, Edit = this.DeleteMinion });
            }
                
            section1.EndUpdate();
            this.NavigationItem.RightBarButtonItem.Enabled = true;
        }

        private void OnNextReadModel(IDataModelEvent readModelChange)
        {
            if (readModelChange.Identity is MinionId)
            {
               // this.MinionUpdated((MinionDataContract)readModel);
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
                section1.Insert(0, new DisclosureElement(minion, new Binding("MinionName")) { Command = this.NavigateToMinion, Edit = this.DeleteMinion });
                this.NavigateToMinion(minion);
            }
        }

        private void NavigateToMinion(Element element)
        {
            this.NavigateToMinion((MinionContract)element.Data);
        }

        private void DeleteMinion(Element element)
        {
            var minion = (MinionContract)element.Data;
            this.commandExecutor.Execute(new DeleteCommand { AggregateId = minion.Identity, });
        }

        private void NavigateToMinion(MinionContract minion)
        {
            var editor = new RecipientEditViewController(this.commandExecutor);
            editor.Load(minion);

            this.NavigationController.PushViewController(editor, true);
        }

        private void Edit(object sender, EventArgs args)
        {
            this.NavigationItem.RightBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Done, this.DoneEdit);
            this.NavigationItem.LeftBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Add, this.Add);
            this.TableView.SetEditing(true, true);
        }
        
        private void DoneEdit(object sender, EventArgs args)
        {
            this.TableView.SetEditing(false, true);
            this.NavigationItem.RightBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Edit, this.Edit);
            this.NavigationItem.LeftBarButtonItem = null;
        }
        
        private void Add(object sender, EventArgs args)
        {
            var subscription = Observable.Start(
                () => 
                 {
                    var id = MinionId.NewId();
                    this.commandExecutor.Execute(new ChangeNameCommand { AggregateId = id, Name = "New Minion" });
                }).Subscribe();

            this.lifetime.Add(subscription);

            this.DoneEdit(null, null);
        }
    }
    
    public class SettingsSource : TableViewSource
    {
     public override UITableViewCellEditingStyle EditingStyleForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return UITableViewCellEditingStyle.Delete;
        }
    }
}

