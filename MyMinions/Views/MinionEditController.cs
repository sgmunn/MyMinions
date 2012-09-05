//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="MinionEditController.cs" company="sgmunn">
//    (c) sgmunn 2012  
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace MyMinions.Views
{
    using System;
    using MyMinions.Domain.Data;
    using MonoTouch.UIKit;
    using MyMinions.Domain;
    using MonoKit.UI;
    using MonoKit.UI.Elements;
    using MonoKit.DataBinding;
    using MonoKit;
    using MonoKit.Reactive.Linq;
    using MonoKit.Reactive.Disposables;
    using System.Collections.Generic;
    using MonoKit.Reactive;
    using MonoKit.Data;

    public class MinionEditController : TableViewController
    {
        private readonly MinionContext context;
        
        private readonly CompositeDisposable lifetime;

        private MinionContract minion;
        
        public MinionEditController(MinionContext context, MinionContract minion) : base(UITableViewStyle.Grouped)
        {
            this.lifetime = new CompositeDisposable();

            this.NavigationItem.Title = minion != null ? "Edit Details" : "Hire Minion";
            this.NavigationItem.LeftBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Cancel);
            this.NavigationItem.LeftBarButtonItem.Clicked += CancelClicked;
            this.NavigationItem.RightBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Done);
            this.NavigationItem.RightBarButtonItem.Clicked += DoneClicked;

            this.context = context;
            //this.commandExecutor = commandExecutor;
            this.Load(minion);
        }
        
        public override void ViewDidUnload()
        {
            this.lifetime.Clear();
            base.ViewDidUnload();
        }

        private void DoneClicked (object sender, EventArgs e)
        {
            this.View.EndEditing(false);
            this.SaveMinionAsync();

            this.DismissModalViewControllerAnimated(true);
        }

        private void CancelClicked (object sender, EventArgs e)
        {
            this.DismissModalViewControllerAnimated(true);
        }

        private void Load(MinionContract minion)
        {
            // bit of a hack, ideally I'd have IEditableObject
            var isNew = minion == null;
            this.minion = new MinionContract
            {
                Id = !isNew ? minion.Id : Guid.NewGuid(),
                MinionName = !isNew ? minion.MinionName : "New Minion",
                WeeklyAllowance = !isNew ? minion.WeeklyAllowance : 0,
            };

            var section1 = new TableViewSection(this.Source);
            
            section1.Header = "Name";
            section1.Add(new TextInputElement(this.minion, null, new Binding("MinionName")) { Placeholder = "Name", });
            
            var section2 = new TableViewSection(this.Source);
            
            section2.Header = "Allowance";
            section2.Add(new DecimalInputElement(this.minion, null, new Binding("WeeklyAllowance")));

            if (!isNew)
            {
                var section3 = new TableViewSection(this.Source);
                
                section3.Header = "Deeds";
                section3.Add(new DisclosureElement("Deeds to Perform") { Command = this.EditDeeds });
            }
        }

        private void EditDeeds(Element element)
        {
            this.NavigationController.PushViewController(new ScheduledDeedsController(this.context, this.minion.Identity), true);
        }

        private void LoadDeeds(TableViewSection section, IEnumerable<ScheduledDeedContract> deeds)
        {
            foreach (var deed in deeds)
            {
                section.Add(new StringElement(deed, new Binding("Description")));
            }
        }

        private void SaveMinionAsync()
        {
            var subscription = Observable.Start(
                () => 
                {
                var cmd = this.context.NewCommandExecutor<MinionAggregate>();

                cmd.Execute(
                    new MonoKit.Domain.IAggregateCommand [] {
                    new ChangeNameCommand { AggregateId = this.minion.Identity, Name = this.minion.MinionName, },
                    new ChangeWeeklyAllowanceCommand { AggregateId = this.minion.Identity, Allowance = this.minion.WeeklyAllowance, }
                });
            }).Subscribe();
        }
    }

    public class ScheduledDeedsController : TableViewController
    {
        private readonly MinionContext context;
        
        private readonly CompositeDisposable lifetime;
        
        private IUniqueIdentity minionId;
        
        public ScheduledDeedsController(MinionContext context, IUniqueIdentity minionId) : base(UITableViewStyle.Grouped)
        {
            this.lifetime = new CompositeDisposable();
            
            this.NavigationItem.Title = "Scheduled Deeds";
            this.NavigationItem.RightBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Add);
            this.NavigationItem.RightBarButtonItem.Clicked += AddClicked;
            
            this.context = context;
            this.minionId = minionId;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            var section1 = new TableViewSection(this.Source);
            this.LoadScheduledDeedsAsync(this.minionId.Id);
        }
        
        public override void ViewDidUnload()
        {
            this.lifetime.Clear();
            base.ViewDidUnload();
        }
        
        private void AddClicked (object sender, EventArgs e)
        {
            this.LoadDeeds(new[] { new ScheduledDeedContract{ DeedId = Guid.NewGuid(), Description = "Make Bed", }});
        }
        
        private void LoadScheduledDeedsAsync(Guid id)
        {
            var repo = new ScheduledDeedRepository(this.context.Connection);
            
            var subscription = Observable.Start<IEnumerable<ScheduledDeedContract>>(
                () =>
                {
                return repo.GetAllForMinion(id);
            })
                .ObserveOnMainThread().Subscribe((deeds) =>
                                                 {
                    this.LoadDeeds(deeds);
                });
            
            // make sure to add to lifetime so that if we navigate away 
            this.lifetime.Add(subscription);
        }
        
        private void LoadDeeds(IEnumerable<ScheduledDeedContract> deeds)
        {
            var section = ((TableViewSection)this.Source.SectionAt(0));
            foreach (var deed in deeds)
            {
                section.Add(new StringElement(deed, new Binding("Description")));
            }
        }
        
        private void SaveMinionAsync()
        {
//            var subscription = Observable.Start(
//                () => 
//                {
//                var cmd = this.context.NewCommandExecutor<MinionAggregate>();
//                
//                cmd.Execute(
//                    new MonoKit.Domain.IAggregateCommand [] {
//                    new ChangeNameCommand { AggregateId = this.minion.Identity, Name = this.minion.MinionName, },
//                    new ChangeWeeklyAllowanceCommand { AggregateId = this.minion.Identity, Allowance = this.minion.WeeklyAllowance, }
//                });
//            }).Subscribe();
        }
    }
}

