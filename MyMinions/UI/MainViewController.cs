using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Collections.Generic;
using MonoKit.UI.PagedViews;
using MonoKit;
using MyMinions.Domain.Data;
using MonoKit.Domain;
using System.Linq;
using MonoKit.Reactive.Disposables;
using MonoKit.Reactive;
using MyMinions.Domain;

namespace MyMinions.UI
{
    public partial class MainViewController : UIViewController, IScrollingPageViewDelegate
    {
        private readonly IDomainContext context;
        private readonly IMinionRepository repository;
        private readonly CompositeDisposable lifetime;

        private ScrollingPageView pagedView;
        private List<MinionDataContract> minions;
        private int currentPage;

        public MainViewController(IDomainContext context, IMinionRepository repository) : base ("MainViewController", null)
        {
            this.context = context;
            this.repository = repository;
            this.lifetime = new CompositeDisposable();

            // TODO: observe deleted events and name changes
           // this.lifetime.Add(this.context.EventBus.Subscribe<IEvent>(this.OnNextEvent));
        }
                
        public void Load()
        {
            // initial bootstrap method, we've not loaded any data previously
            this.minions = this.repository.GetAll().OrderBy(x => x.MinionName).ToList();
            this.LoadMinions(0);
        }

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();
            
            // Release any cached data, images, etc that aren't in use.
        }
        
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            
            // Perform any additional setup after loading the view, typically from a nib.
            // we could have been in here previously, if we were unloaded from a memory warning 
            
            // todo: we need to anchor to bounds for when the in-call status bar appears
            this.pagedView = new ScrollingPageView(this.contentView.Bounds, false);
            this.pagedView.AutoresizingMask = UIViewAutoresizing.All;
            this.pagedView.Delegate = this;
            this.pagedView.Alpha = 0f;
            this.contentView.AddSubview(this.pagedView);
            
            this.LoadMinions(this.currentPage);
            
            UIView.BeginAnimations("loadFadeIn");
            UIView.SetAnimationDuration(0.15f);
            this.pagedView.Alpha = 1f;
            UIView.CommitAnimations();
        }
        
        public override void ViewDidUnload()
        {
            base.ViewDidUnload();
            
            // Clear any references to subviews of the main view in order to
            // allow the Garbage Collector to collect them sooner.
            //
            // e.g. myOutlet.Dispose (); myOutlet = null;
            
            this.pagedView.RemoveFromSuperview();
            this.pagedView.Dispose();
            this.pagedView = null;
            
            this.ReleaseDesignerOutlets();
        }
        
        public override void ViewWillAppear(bool animated)
        {
            this.NavigationController.NavigationBarHidden = true;

            base.ViewWillAppear(animated);
            
            if (this.minions != null && this.minions.Count > 0) 
            {
            //this.minions = this.repository.GetAll().OrderBy(x => x.MinionName).ToList();
            //this.pagedView.ReloadPages();
            
                if (this.pagedView != null)
                    this.pagedView.ScrollToPage(this.currentPage);
                
                // refresh current minion
                
                //this.IScrollingPageViewDelegate.UpdatePage(0, )
            }
        }

        public override bool ShouldAutorotateToInterfaceOrientation(UIInterfaceOrientation toInterfaceOrientation)
        {
            // Return true for supported orientations
            return (toInterfaceOrientation == UIInterfaceOrientation.Portrait);
        }

        partial void settingsButtonClick(NSObject sender)
        {
            var settings = new SettingsViewController(this.context, this.repository);
            this.NavigationController.PushViewController(settings, true);
        }

        private void OnNextEvent(IEvent @event)
        {
            // I think I'd prefer to have notification of repository changes instead, perhaps event the thing
            // that actually changed so I don't have to reload it
            if (@event.AggregateTypeId == Minion.AggregateTypeId)
            {
                if (@event is NameChangedEvent)
                {

                }
            }
        }
                  
        private void LoadMinions(int pageIndex)
        {
            if (this.pagedView != null)
            {
                this.pagedView.ReloadPages();
                this.pagedView.ScrollToPage(pageIndex);
            }
        }
      
        #region IScrollingPageViewDelegate implementation
        string IScrollingPageViewDelegate.GetPageTypeKey(int index)
        {
            return string.Empty;
        }

        UIView IScrollingPageViewDelegate.CreateView(string pageTypeKey, RectangleF frame)
        {
            var view = this.LoadViewFromNib<MinionView>();
            view.Frame = frame;
            view.Controller = this;
            return view;
        }

        void IScrollingPageViewDelegate.UpdatePage(int index, UIView view)
        {
            var minionView = (MinionView)view; 
            minionView.Minion = this.minions[index];  
        }

        void IScrollingPageViewDelegate.PageIndexChanged(int index)
        {
            this.currentPage = index;
        }

        int IScrollingPageViewDelegate.PageCount
        {
            get
            {
                return this.minions == null ? 0 : this.minions.Count;
            }
        }
        #endregion

    }
}

