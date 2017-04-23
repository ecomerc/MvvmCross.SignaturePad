using Foundation;
using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Platform;
using MvvmCross.Platform;
using UIKit;


namespace Sample.iOS {

    [Foundation.Register("AppDelegate")]
    public class AppDelegate : MvxApplicationDelegate {

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions) {
            var window = new UIWindow(UIScreen.MainScreen.Bounds);
            window.MakeKeyAndVisible();

            var setup = new Setup(this, window);
            setup.Initialize();

            var startup = Mvx.Resolve<IMvxAppStart>();
            startup.Start();

            #if DEBUG
            //Cirrious.MvvmCross.Binding.MvxBindingTrace.TraceBindingLevel = Cirrious.CrossCore.Platform.MvxTraceLevel.Diagnostic;
            #endif

            return true;
        }
    }
}