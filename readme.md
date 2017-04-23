# Announcement

This is a fork from https://github.com/aritchie/acrmvvmcross that has a working ios sample and MVVMCross upgraded to 4.3

# Signature Pad for iOS and Android

Call for a signature pad dialog in 1 line of xplat code from a view model!

	signatureService.RequestSignature(result => {
		if (result.Cancelled)
			return;

		// use the image stream to write to file or serialize the draw points
		// result.Stream or result.Points
	});


	signatureService.LoadSignature(drawPoints);

##Configuration

	signatureService.DefaultConfiguration.ClearText = "Why clear?";

	or pass overridden configuration to each method:

	signatureService.RequestSignature(callback, new SignaturePadConfiguration {
		SaveText = "Signed!",
		CancelText = "No way!",
		PromptText = "Right here"
	});

### Features


* Draw signature
* Save as image
* Save as points

* Add background image to signature pad

## Supported Platforms

* Xamarin (iOS Unified/Android)
* Portable Class Libraries (Profile 259)

## Setup

#### iOS
Creat a SignaturePadPluginBootstrap class inside Bootstrap folder

    public class SignaturePadPluginBootstrap
        : MvxLoaderPluginBootstrapAction<Acr.MvvmCross.Plugins.SignaturePad.PluginLoader, Acr.MvvmCross.Plugins.SignaturePad.Touch.Plugin>
    {
    }

### MvvmCross
In your App class, override the LoadPlugins method to include

     public override void LoadPlugins(IMvxPluginManager pluginManager) {
            base.LoadPlugins(pluginManager);
            pluginManager.EnsurePlatformAdaptionLoaded<Acr.MvvmCross.Plugins.SignaturePad.PluginLoader>();
        }
