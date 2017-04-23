using System;
using Acr.IO;
using Acr.UserDialogs;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Platform.Plugins;
using Sample.Core.ViewModels;


namespace Sample.Core {
    
    public class App : MvxApplication {

        public App() {
            this.RegisterAppStart<HomeViewModel>();
        }

        public override void Initialize()
        {

            Mvx.RegisterSingleton<IUserDialogs>(() => UserDialogs.Instance);
        }

        public override void LoadPlugins(IMvxPluginManager pluginManager) {
            base.LoadPlugins(pluginManager);
            pluginManager.EnsurePlatformAdaptionLoaded<Acr.MvvmCross.Plugins.SignaturePad.PluginLoader>();
        }
    }
}
