using MvvmCross.Platform;
using MvvmCross.Platform.Plugins;
using System;


namespace Acr.MvvmCross.Plugins.SignaturePad {
    
    public class PluginLoader : IMvxPluginLoader {
        public static readonly PluginLoader Instance = new PluginLoader();

        public void EnsureLoaded() {
            var manager = Mvx.Resolve<IMvxPluginManager>();
            manager.EnsurePlatformAdaptionLoaded<PluginLoader>();
        }
    }
}
