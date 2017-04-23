using MvvmCross.Platform;
using MvvmCross.Platform.Plugins;
using System;


namespace EcoMerc.MvvmCross.Plugins.SignaturePad.Touch {

    public class Plugin : IMvxPlugin {

        public void Load() {
            Mvx.RegisterSingleton<ISignatureService>(new TouchSignatureService());
        }
    }
}