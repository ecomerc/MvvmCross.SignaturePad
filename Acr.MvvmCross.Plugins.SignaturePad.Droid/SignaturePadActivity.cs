using System;
using System.IO;
using System.Linq;
using System.Drawing;
using Android.App;
using Android.OS;
using Android.Widget;
using Android.Views;
using SignaturePad;
using Android.Content;
using Acr.MvvmCross.Plugins.SignaturePad.Droid.Extensions;
using Android.Media;
using MvvmCross.Plugins.Color.Droid;

namespace Acr.MvvmCross.Plugins.SignaturePad.Droid {

    [Activity()] //Process = ":AcrMvxSignaturePad"
    public class SignaturePadActivity : Activity {
        private static readonly string fileStore;
        private SignaturePadView signatureView;
        private Button btnSave;
        private Button btnCancel;
        private SignaturePadConfiguration currentConfig;
        private bool hasBackground;


        static SignaturePadActivity() {
            var path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            fileStore = Path.Combine(path, "signature.tmp");

        }


        protected override void OnCreate(Bundle bundle) {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(bundle);
            this.SetContentView(Resource.Layout.SignaturePad);

            SignaturePadOrientation actualOrientation = SignaturePadOrientation.Automatic;

            this.currentConfig = new SignaturePadConfiguration();
            this.currentConfig.LoadFromIntent(this.Intent);

            var rootView = this.FindViewById<RelativeLayout>(Resource.Id.rootView);
            this.signatureView = this.FindViewById<SignaturePadView>(Resource.Id.signatureView);
            this.btnSave = this.FindViewById<Button>(Resource.Id.btnSave);
            this.btnCancel = this.FindViewById<Button>(Resource.Id.btnCancel);

            var cfg = currentConfig;
            rootView.SetBackgroundColor(cfg.BackgroundColor.ToAndroidColor());
            this.signatureView.BackgroundColor = cfg.SignatureBackgroundColor.ToAndroidColor();


            this.signatureView.Caption.Text = cfg.CaptionText;
            this.signatureView.Caption.SetTextColor(cfg.CaptionTextColor.ToAndroidColor());
            this.signatureView.ClearLabel.Text = cfg.ClearText;
            this.signatureView.ClearLabel.SetTextColor(cfg.ClearTextColor.ToAndroidColor());
            this.signatureView.SignatureLineColor = cfg.SignatureLineColor.ToAndroidColor();
            this.signatureView.SignaturePrompt.Text = cfg.PromptText;
            this.signatureView.SignaturePrompt.SetTextColor(cfg.PromptTextColor.ToAndroidColor());
            this.signatureView.StrokeColor = cfg.StrokeColor.ToAndroidColor();
            this.signatureView.StrokeWidth = cfg.StrokeWidth;


            this.btnSave.Text = cfg.SaveText;
            this.btnCancel.Text = cfg.CancelText;
            if (string.IsNullOrWhiteSpace(cfg.CancelText)) {
                this.btnCancel.Visibility = ViewStates.Invisible;
            }

            var exif = new ExifInterface(cfg.BackgroundImage);
            var orientation = exif.GetAttribute(ExifInterface.TagOrientation);
            var width = exif.GetAttributeInt(ExifInterface.TagImageWidth, 100);
            var height = exif.GetAttributeInt(ExifInterface.TagImageLength, 80);            
            switch (orientation) {
                case "1": // landscape
                case "3": // landscape
                    actualOrientation = SignaturePadOrientation.Landscape;
                    break;

                case "8":
                case "4":
                case "6": // portrait
                    actualOrientation = SignaturePadOrientation.Portrait;
                    break;

                case "0": //undefined
                default:
                    if (width > height)
                        actualOrientation = SignaturePadOrientation.Landscape;
                    else
                        actualOrientation = SignaturePadOrientation.Portrait;
                    break;
            }

            switch (actualOrientation) {
                case SignaturePadOrientation.Landscape:
                    this.RequestedOrientation = Android.Content.PM.ScreenOrientation.Landscape;
                    break;
                case SignaturePadOrientation.Portrait:
                    this.RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;
                    break;

            }


            this.signatureView.BackgroundImageView.LayoutParameters.Height = ViewGroup.LayoutParams.FillParent;
            this.signatureView.BackgroundImageView.LayoutParameters.Width = ViewGroup.LayoutParams.FillParent;


            this.signatureView.BackgroundImageView.ViewTreeObserver.GlobalLayout += (sender, e) =>  //also tried with _View
            {

                var newSize = new Size(this.signatureView.Width, this.signatureView.Height);
                if (newSize.Width > 0 && !hasBackground) {
                    if (cfg.Points != null && cfg.Points.Count() > 0) {

                        var points = cfg.Points.Select(i => i.GetPointF()).ToArray();
                        this.signatureView.LoadPoints(points);
                    } else {
                    }
                    //Get a smaller image if needed (memory optimization)
                    var bm = LoadAndResizeBitmap(cfg.BackgroundImage, newSize);
                    if (bm != null) {
                        hasBackground = true;
                        switch (cfg.BackgroundImageSize) {
                            case SignaturePadBackgroundSize.Fill:
                                this.signatureView.BackgroundImageView.SetScaleType(ImageView.ScaleType.FitXy);
                                this.signatureView.BackgroundImageView.SetAdjustViewBounds(true);
                                break;
                            case SignaturePadBackgroundSize.Stretch:
                                this.signatureView.BackgroundImageView.SetScaleType(ImageView.ScaleType.FitXy);
                                break;

                        }
                        this.signatureView.BackgroundImageView.SetImageBitmap(bm);
                    }
                }
            };


        }


        protected override void OnResume() {
            base.OnResume();
            this.btnSave.Click += this.OnSave;
            this.btnCancel.Click += this.OnCancel;
        }


        protected override void OnPause() {
            base.OnPause();
            this.btnSave.Click -= this.OnSave;
            this.btnCancel.Click -= this.OnCancel;
        }

        private void OnSave(object sender, EventArgs args) {
            if (this.signatureView.IsBlank)
                return;

            var points = this.signatureView
                .Points
                .Select(x => x.GetDrawPoint());
            
            //var service = this.Resolve();

            using (var image = this.signatureView.GetImage(this.currentConfig.CropImage,true)) {
                using (var fs = new FileStream(fileStore, FileMode.Create)) {
                    var format = currentConfig.ImageType == ImageFormatType.Png
                        ? Android.Graphics.Bitmap.CompressFormat.Png
                        : Android.Graphics.Bitmap.CompressFormat.Jpeg;
                    image.Compress(format, 100, fs);
                }
            }



            var intent = new Intent("acr.mvvmcross.plugins.signaturepad.droid.SIGNATURE");
            intent.PutExtra("fileStore", fileStore);
            intent.PutExtra("points", points.Select(i => i.ToString()).ToArray());
            SendBroadcast(intent);

            this.Finish();
        }


        private void OnCancel(object sender, EventArgs args) {
            var intent = new Intent("acr.mvvmcross.plugins.signaturepad.droid.SIGNATURE");
            SendBroadcast(intent);

            this.Finish();
        }

        private Size GetBitmapSize(string fileName) {
            var options = new Android.Graphics.BitmapFactory.Options { InJustDecodeBounds = true };
            Android.Graphics.BitmapFactory.DecodeFile(fileName, options);
            return new Size(options.OutWidth, options.OutHeight);

        }

        private Android.Graphics.Bitmap LoadAndResizeBitmap(string fileName, Size newSize) {
            var exif = new ExifInterface(fileName);

            var width = exif.GetAttributeInt(ExifInterface.TagImageWidth, 100);
            var height = exif.GetAttributeInt(ExifInterface.TagImageLength, 80);
            var orientation = exif.GetAttribute(ExifInterface.TagOrientation);


            // We calculate the ratio that we need to resize the image by
            // in order to fit the requested dimensions.

            var inSampleSize = 1.0;

            if (newSize.Height < height || newSize.Width < width) {
                inSampleSize = newSize.Width > newSize.Height
                    ? newSize.Height / height
                        : newSize.Width / width;
            }

            var options = new Android.Graphics.BitmapFactory.Options {
                InJustDecodeBounds = false,
                InSampleSize = (int)inSampleSize
            };
            // Now we will load the image and have BitmapFactory resize it for us.
            var resizedBitmap = Android.Graphics.BitmapFactory.DecodeFile(fileName, options);

            var rotate = false;
            
            switch (orientation) {
                case "1": // landscape
                case "3": // landscape
                    if (width < height)
                        rotate = true;
                    break;
                case "8":
                case "4":
                case "6": // portrait
                    if (width > height)
                        rotate = true;
                    break;
                case "0": //undefined
                default:
                    break;
            }

            if (rotate) {
                var mtx = new Android.Graphics.Matrix();
                mtx.PreRotate(90);
                resizedBitmap = Android.Graphics.Bitmap.CreateBitmap(resizedBitmap, 0, 0, resizedBitmap.Width, resizedBitmap.Height, mtx, false);
                mtx.Dispose();
                mtx = null;

            }


            return resizedBitmap;
        }

    }
}

