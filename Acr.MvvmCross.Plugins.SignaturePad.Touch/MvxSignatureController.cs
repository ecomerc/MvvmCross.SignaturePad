using System;
using System.IO;
using System.Linq;
using System.Drawing;
using UIKit;
using Foundation;
using MvvmCross.Plugins.Color.iOS;
using Acr.MvvmCross.Plugins.SignaturePad.Touch.Extensions;

namespace Acr.MvvmCross.Plugins.SignaturePad.Touch {

    public class MvxSignatureController : UIViewController {

        private MvxSignatureView view;
        private readonly Action<SignatureResult> onResult;
        private readonly SignaturePadConfiguration config;


        public MvxSignatureController(SignaturePadConfiguration config, Action<SignatureResult> onResult) {
            this.config = config;
            this.onResult = onResult;
        }


        public override void LoadView() {
            base.LoadView();

            this.view = new MvxSignatureView();
            this.View = this.view;
        }


        public override void ViewDidLoad() {
            base.ViewDidLoad();

            this.view.BackgroundColor = this.config.BackgroundColor.ToNativeColor();
            this.view.Signature.BackgroundColor = this.config.SignatureBackgroundColor.ToNativeColor();
            if (!string.IsNullOrWhiteSpace(this.config.BackgroundImage))
                this.view.Signature.BackgroundImageView.Image = UIImage.FromFile(this.config.BackgroundImage);
            this.view.Signature.BackgroundImageView.Alpha = this.config.BackgroundImageAlpha;

            this.view.Signature.BackgroundImageView.Frame = this.view.Signature.Bounds;
            this.view.Signature.BackgroundImageView.AutoresizingMask = UIViewAutoresizing.All;

            if (this.config.BackgroundImageSize == SignaturePadBackgroundSize.Fill)
                this.view.Signature.BackgroundImageView.ContentMode = UIViewContentMode.ScaleAspectFit;

            if (this.config.BackgroundImageSize == SignaturePadBackgroundSize.Stretch)
                this.view.Signature.BackgroundImageView.ContentMode = UIViewContentMode.ScaleToFill;

            this.view.Signature.Caption.TextColor = this.config.CaptionTextColor.ToNativeColor();
            this.view.Signature.Caption.Text = this.config.CaptionText;
            this.view.Signature.ClearLabel.SetTitle(this.config.ClearText, UIControlState.Normal);
            this.view.Signature.ClearLabel.SetTitleColor(this.config.ClearTextColor.ToNativeColor(), UIControlState.Normal);
            this.view.Signature.SignatureLineColor = this.config.SignatureLineColor.ToNativeColor();
            this.view.Signature.SignaturePrompt.Text = this.config.PromptText;
            this.view.Signature.SignaturePrompt.TextColor = this.config.PromptTextColor.ToNativeColor();
            this.view.Signature.StrokeColor = this.config.StrokeColor.ToNativeColor();
            this.view.Signature.StrokeWidth = this.config.StrokeWidth;
            this.view.Signature.Layer.ShadowOffset = new SizeF(0, 0);
            this.view.Signature.Layer.ShadowOpacity = 1f;

     




            this.view.SaveButton.SetTitle(this.config.SaveText, UIControlState.Normal);
            this.view.SaveButton.TouchUpInside += (sender, args) => {
                if (this.view.Signature.IsBlank)
                    return;

                var points = this.view
                    .Signature
                    .Points
                    .Select(x => x.GetDrawPoint());



                var tempPath = GetTempFilePath();
                using (var image = this.view.Signature.GetImage(this.config.CropImage))
                using (var stream = GetImageStream(image, this.config.ImageType))
                using (var fs = new FileStream(tempPath, FileMode.Create))
                    stream.CopyTo(fs);

                this.DismissViewController(true, null);
                this.onResult(new SignatureResult(false, () => new FileStream(tempPath, FileMode.Open, FileAccess.Read, FileShare.Read), points));
            };

            this.view.CancelButton.SetTitle(this.config.CancelText, UIControlState.Normal);
            this.view.CancelButton.TouchUpInside += (sender, args) => {

#if DEBUG
                System.Diagnostics.Debug.WriteLine("Signature", "Finally has " + this.view.Signature.Points.Count() + " points");

                foreach (var point in this.view.Signature.Points.Take(10))
                    System.Diagnostics.Debug.WriteLine(" - " + point.ToString());
#endif
                this.DismissViewController(true, null);
                this.onResult(new SignatureResult(true, null, null));
            };
        }

        public override void ViewDidAppear(bool animated) {
            base.ViewDidAppear(animated);

            if (this.config.Points != null && this.config.Points.Count() > 0) {
                System.Diagnostics.Debug.WriteLine("Signature", "Has " + this.config.Points.Count() + " points");
                var points = this.config.Points.SkipWhile(p => p.IsEmpty || (p.X == 0 && p.Y == 0)).Select(i => i.GetPointF()).ToArray();

#if DEBUG
                this.view.Signature.LoadPoints(points);
                foreach (var point in points.Take(10))
                    System.Diagnostics.Debug.WriteLine(" - " + point.ToString());

                this.view.Signature.SetNeedsDisplay();

#endif
            }
#if DEBUG
            System.Diagnostics.Debug.WriteLine("Signature", "Now has " + this.view.Signature.Points.Count() + " points");

            foreach (var point in this.view.Signature.Points.Take(10))
                System.Diagnostics.Debug.WriteLine(" - " + point.ToString());
#endif   
        }

        private static string GetTempFilePath() {
            var documents = UIDevice.CurrentDevice.CheckSystemVersion(8, 0)
                ? NSFileManager.DefaultManager.GetUrls(NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomain.User)[0].Path
                : Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            var tempPath = Path.Combine(documents, "..", "tmp");
            return Path.Combine(tempPath, "Signature.tmp");
        }


        private static Stream GetImageStream(UIImage image, ImageFormatType formatType) {
            if (formatType == ImageFormatType.Jpg)
                return image.AsJPEG().AsStream();

            return image.AsPNG().AsStream();
        }
    }
}


//            FROM XAMARIN SAMPLES
//            this.view.Signature.Caption.Font = UIFont.FromName ("Marker Felt", 16f);
//            this.view.Signature.SignaturePrompt.Font = UIFont.FromName ("Helvetica", 32f);
//            this.view.Signature.BackgroundColor = UIColor.FromRGB (255, 255, 200); // a light yellow.
//            this.view.Signature.BackgroundImageView.Image = UIImage.FromBundle ("logo-galaxy-black-64.png");
//            this.view.Signature.BackgroundImageView.Alpha = 0.0625f;
//            this.view.Signature.BackgroundImageView.ContentMode = UIViewContentMode.ScaleToFill;
//            this.view.Signature.BackgroundImageView.Frame = new System.Drawing.RectangleF(20, 20, 256, 256);