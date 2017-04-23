using System.Collections.Generic;
using MvvmCross.Core.ViewModels;


namespace Sample.Core.ViewModels {
    
    public class HomeViewModel : MvxViewModel {

        public IList<MenuItemViewModel> Menu { get; private set; }
        public MvxCommand<MenuItemViewModel> View {
            get {
                return new MvxCommand<MenuItemViewModel>(item => item.Command.Execute());
            }
        }


        public HomeViewModel() {
            this.Menu = new List<MenuItemViewModel> {
				new MenuItemViewModel(
					"Signature List",
					() => ShowViewModel<SignatureListViewModel>()
				)
            };
        }
    }
}
