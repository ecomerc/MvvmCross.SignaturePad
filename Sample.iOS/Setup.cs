using Acr.IO;
using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Platform;
using MvvmCross.Platform;
using UIKit;
using Sample.Core;


namespace Sample.iOS {
    
    public class Setup : MvxIosSetup
    {

        public Setup(MvxApplicationDelegate appDelegate, UIWindow window) : base(appDelegate, window) {}


        protected override IMvxApplication CreateApp() {

            Mvx.RegisterSingleton<IFileViewer>(Acr.IO.FileViewer.Instance);
            Mvx.RegisterSingleton<IFileSystem>(Acr.IO.FileSystem.Instance);

            return new App();
        }
    }
}
