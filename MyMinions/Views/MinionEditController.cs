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

    public class MinionEditController : TableViewController
    {
        private readonly MinionContext context;

        private MinionContract minion;
        
        public MinionEditController(MinionContext context, MinionContract minion) : base(UITableViewStyle.Grouped)
        {
            this.NavigationItem.Title = "edit";
            this.NavigationItem.LeftBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Cancel);
            this.NavigationItem.LeftBarButtonItem.Clicked += CancelClicked;
            this.NavigationItem.RightBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Done);
            this.NavigationItem.RightBarButtonItem.Clicked += SaveClicked;

            this.context = context;
            //this.commandExecutor = commandExecutor;
            this.Load(minion);
        }

        private void SaveClicked (object sender, EventArgs e)
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
            var editMinion = new MinionContract
            {
                Id = minion.Id,
                MinionName = minion.MinionName,
                WeeklyAllowance = minion.WeeklyAllowance,
            };

            this.minion = editMinion;

            var section1 = new TableViewSection(this.Source);
            
            section1.Header = "Minion Name";
            section1.BeginUpdate();
            section1.Add(new TextInputElement(this.minion, null, new Binding("MinionName")) { Placeholder = "Name", CanEdit = (_) => false });
            section1.EndUpdate();
            
            var section2 = new TableViewSection(this.Source);
            
            section2.Header = "Weekly Allowance";
            section2.BeginUpdate();
            section2.Add(new DecimalInputElement(this.minion, null, new Binding("WeeklyAllowance")) { CanEdit = (_) => true });
            section2.EndUpdate();
        }
        
        public override void ViewWillDisappear(bool animated)
        {
            // tell the view to end editing, resigns responders and triggers a property change for our minion
            //this.View.EndEditing(false);
            base.ViewWillDisappear(animated);
            //this.SaveMinionAsync();
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
}
