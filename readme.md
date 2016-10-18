# This fork has a working ios sample and MVVMCross updated to 4.3

# Announcement

Fork of aritchies great library, he has split out all but one of the old MvvmCross plugins, and the development of that plugin contiues here. There is no Nuget package, yet. 

The code has been upgraded to MvvmCross 4.



#Signature Pad for iOS and Android (Windows Phone 8 support coming soon)

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

## Unsupported Platforms

* Universal Windows Platform (Win10/UWP) )
* Windows Phone 8/8.1 - It is here, but parts of unimplemented.  NO FEATURE REQUESTS OR SUPPORT - YOU WANT IT, SUBMIT A TESTED PULL REQUEST!


## Setup

TBD

#### iOS and Windows

    TDB

#### Android Initialization (In your main activity)

    TDB

### MvvmCross

    TDB



## FAQ

TDB
