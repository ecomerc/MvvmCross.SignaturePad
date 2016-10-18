using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.iOS.Views;
using Sample.Core.ViewModels;


namespace Sample.iOS.Views {
    
    [Foundation.Register("MainView")]
    public class HomeView : MvxTableViewController {

        public override void ViewDidLoad() {
            base.ViewDidLoad();
            this.Title = "ACR MvvmCross Plugins";
            var src = new MvxStandardTableViewSource(this.TableView, "TitleText Title");

            var set = this.CreateBindingSet<HomeView, HomeViewModel>();
            set.Bind(src).To(x => x.Menu);
            set.Bind(src).For(x => x.SelectionChangedCommand).To(x => x.View);
            set.Apply();
            
            this.TableView.Source = src;
            this.TableView.ReloadData();
        }
    }
}