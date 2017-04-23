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