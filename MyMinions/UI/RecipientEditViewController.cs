//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file=".cs" company="sgmunn">
//    (c) sgmunn 2012  
// 
//    Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
//    documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
//    the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
//    to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
//    The above copyright notice and this permission notice shall be included in all copies or substantial portions of 
//    the Software.
//  
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO 
//    THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//    CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS 
//    IN THE SOFTWARE.
//  </copyright>
//  -------------------------------------------------------------------------------------------------------------------- 
using MonoKit.Reactive.Linq;

namespace MyMinions.UI
{
    using System;
    using MyMinions.Domain.Data;
    using MonoKit.UI;
    using MonoKit.UI.Elements;
    using MonoKit.Domain;
    using MonoKit.DataBinding;
    using MonoTouch.UIKit;
    using MyMinions.Domain;
    using MonoKit;

    public class RecipientEditViewController : TableViewController
    {
        private readonly ICommandExecutor<Minion> commandExecutor;

        private MinionContract minion;

        public RecipientEditViewController(ICommandExecutor<Minion> commandExecutor) : base(UITableViewStyle.Grouped)
        {
            this.commandExecutor = commandExecutor;
        }

        public void Load(MinionContract minion)
        {
            this.minion = minion;

            this.NavigationItem.Title = minion.MinionName ?? "New Minion";

            var section1 = new TableViewSection(this.Source);
            
            section1.Header = "Minion Name";
            section1.BeginUpdate();
            section1.Add(new TextInputElement(minion, null, new Binding("MinionName")) { Placeholder = "Name", CanEdit = (_) => false });
            section1.EndUpdate();

            var section2 = new TableViewSection(this.Source);
            
            section2.Header = "Weekly Allowance";
            section2.BeginUpdate();
            section2.Add(new DecimalInputElement(minion, null, new Binding("WeeklyAllowance")) { CanEdit = (_) => true });
            section2.EndUpdate();
        }

        public override void ViewWillDisappear(bool animated)
        {
            // tell the view to end editing, resigns responders and triggers a property change for our minion
            this.View.EndEditing(false);
            base.ViewWillDisappear(animated);
            this.SaveMinionAsync();
        }
        
        private void SaveMinionAsync()
        {
            var subscription = Observable.Start(
                () => 
                 {
                    this.commandExecutor.Execute(
                        new MonoKit.Domain.IAggregateCommand [] {
                            new ChangeNameCommand { AggregateId = minion.Identity, Name = this.minion.MinionName, },
                            new ChangeWeeklyAllowanceCommand { AggregateId = minion.Identity, Allowance = this.minion.WeeklyAllowance, }
                        });
                }).Subscribe();
        }
    }
}

