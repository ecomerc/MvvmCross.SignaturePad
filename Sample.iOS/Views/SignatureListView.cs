using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.iOS.Views;
using UIKit;
using Sample.Core.ViewModels;


namespace Sample.iOS.Views {

    [Foundation.Register("SignatureListView")]
    public class SignatureListView : MvxTableViewController {

        public override void ViewDidLoad() {
            base.ViewDidLoad();
            this.Title = "MVX Signatures";

            var btnAdd = new UIBarButtonItem(UIBarButtonSystemItem.Add);
            this.NavigationItem.RightBarButtonItems = new [] { btnAdd };

            var src = new MvxStandardTableViewSource(this.TableView, "TitleText Name");

            var set = this.CreateBindingSet<SignatureListView, SignatureListViewModel>();
            set.Bind(src).To(x => x.List);
            set.Bind(src).For(x => x.SelectionChangedCommand).To(x => x.View);
            set.Bind(btnAdd).To(x => x.Create);
            set.Apply();

            this.TableView.Source = src;
            this.TableView.ReloadData();
        }
    }
}