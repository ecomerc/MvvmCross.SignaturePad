using System;
using System.IO;
using System.Linq;
using System.Drawing;
using Android.App;
using Android.OS;
using Android.Widget;
using Android.Views;
using Cirrious.MvvmCross.Plugins.Color.Droid;
using Cirrious.CrossCore;
using SignaturePad;
using Splat;
using Android.Content;

namespace Acr.MvvmCross.Plugins.SignaturePad.Droid {

    [Activity(Process = ":AcrMvxSignaturePad")]
    public class SignaturePadActivity : Activity {
        private static readonly string fileStore;
        private SignaturePadView signatureView;
        private Button btnSave;
        private Button btnCancel;
        private SignaturePadConfiguration currentConfig;


        static SignaturePadActivity() {
            var path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            fileStore = Path.Combine(path, "signature.tmp");

        }


        protected override void OnCreate(Bundle bundle) {
            base.OnCreate(bundle);
            this.SetContentView(Resource.Layout.SignaturePad);

            //TODO Load this somehow.
            this.currentConfig = new SignaturePadConfiguration();

            var rootView = this.FindViewById<RelativeLayout>(Resource.Id.rootView);
            this.signatureView = this.FindViewById<SignaturePadView>(Resource.Id.signatureView);
            this.btnSave = this.FindViewById<Button>(Resource.Id.btnSave);
            this.btnCancel = this.FindViewById<Button>(Resource.Id.btnCancel);

            var cfg = currentConfig;
            rootView.SetBackgroundColor(cfg.BackgroundColor.ToAndroidColor());
            this.signatureView.BackgroundColor = cfg.SignatureBackgroundColor.ToAndroidColor();
            //this.signatureView.BackgroundImageView.SetImage();
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
                .Select(x => new DrawPoint(x.X, x.Y));

            //var service = this.Resolve();

            using (var image = this.signatureView.GetImage()) {
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
            /*
            service.Complete(new SignatureResult(
                false,
                () => new FileStream(fileStore, FileMode.Open, FileAccess.Read, FileShare.Read),
                points
            ));
*/

            this.Finish();
        }


        private void OnCancel(object sender, EventArgs args) {
            var intent = new Intent("acr.mvvmcross.plugins.signaturepad.droid.SIGNATURE");
            SendBroadcast(intent);

            this.Finish();
        }
    }
}

