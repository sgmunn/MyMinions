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
using MonoKit.Domain.Commands;
using MonoKit.Domain.Events;

namespace MyMinions.UI
{
    // todo: support delete of aggregate
    public class SettingsViewController : TableViewController
    {
        private readonly IDomainContext context;

        private readonly IMinionRepository repository;

        private readonly CompositeDisposable lifetime;

        private MinionDataContract editingMinion;

        public SettingsViewController(IDomainContext context, IMinionRepository repository) : base(MonoTouch.UIKit.UITableViewStyle.Plain, new SettingsSource())
        {
            this.context = context;
            this.repository = repository;
            this.lifetime = new CompositeDisposable();

            this.NavigationItem.Title = "Settings";
            this.NavigationItem.RightBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Edit, this.Edit);
        }
        
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.lifetime.Add(this.context.EventBus.Subscribe(this.OnNextEvent));
            
            if (this.Source.Count == 0)
            {
                new TableViewSection(this.Source) { AllowMoveItems = false };
            }

            this.LoadMinions();
        }
        
        public override void ViewDidUnload()
        {
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

            if (this.editingMinion != null)
            {
                this.SaveMinion(this.editingMinion);
            }

            this.editingMinion = null;
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);

            if (this.IsMovingFromParentViewController)
            {
                this.lifetime.Clear();
            }
        }

        private void LoadMinions()
        {
            // todo: async
            // todo: merge existing items instead of reload
            var section1 = ((TableViewSection)this.Source.SectionAt(0));
                
            section1.BeginUpdate();
            section1.Clear();
            foreach (var minion in this.repository.GetAll().OrderBy(x => x.MinionName))
            {
                section1.Add(new DisclosureElement(minion, new Binding("MinionName")) { Command = this.NavigateToMinion });
            }
                
            section1.EndUpdate();
        }

        private void OnNextEvent(IEvent @event)
        {
            Console.WriteLine("event bus {0}", @event);
            // we have a choice, we can reload the whole list, just the one aggregate or update the UI directly
            // what we do depends on what we're displaying here.  I'll show the three as an example

            // 1st option -- ideally this would merge our already loaded items
            // this.LoadMinions();

            var section1 = ((TableViewSection)this.Source.SectionAt(0));

            // 2nd option, update or replace the element that was changed or added
            if (@event is CreatedEvent)
            {
                var minion = this.repository.GetById(@event.AggregateId);
                section1.Insert(0, new DisclosureElement(minion, new Binding("MinionName")) { Command = this.NavigateToMinion });
            }
            else
            {
                // 3rd option - update table directly.  works best if the data being loaded knows how to update bindings
                if (@event is NameChangedEvent)
                {
                    var element = section1.First(x => ((MinionDataContract)x.Data).Id == @event.AggregateId);
                    var minion = (MinionDataContract)element.Data;

                    minion.MinionName = ((NameChangedEvent)@event).Name;

                    // update bindings because our data contract doesn't support INotifyPropertyChanged
                    var bindings = element.GetBindingExpressions("Text");
                    foreach (var binding in bindings)
                    {
                        binding.UpdateTarget();
                    }
                }
            }
        }
        
        private void NavigateToMinion(Element element)
        {
            this.NavigateToMinion((MinionDataContract)element.Data);
        }

        private void NavigateToMinion(MinionDataContract minion)
        {
            this.editingMinion = minion;

            // if this was a specific controller class we could intercept viewDidDisappear to trigger saves
            // in this case we'll remember which one we're editing and do the save on the view did appear
            var controller = new TableViewController(UITableViewStyle.Grouped);
            controller.NavigationItem.Title = minion.MinionName ?? "New Minion";

            var section1 = new TableViewSection(controller.Source);
            
            section1.Header = " ";
            section1.BeginUpdate();
            section1.Add(new TextInputElement(minion, null, new Binding("MinionName")) { Placeholder = "Name" });
            section1.EndUpdate();
            
            this.NavigationController.PushViewController(controller, true);
        }

        private void SaveMinion(MinionDataContract minion)
        {
            this.context.NewCommandExecutor<Minion>().Execute(new ChangeNameCommand { AggregateId = minion.Id, Name = minion.MinionName, });
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
            // todo: async
            var id = Guid.NewGuid();
            this.context.NewCommandExecutor<Minion>().Execute(new CreateCommand { AggregateId = id, });
            // do this after command execution
            this.NavigateToMinion(this.repository.GetById(id));

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

