using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Linq.Expressions;

namespace EcoMerc.MvvmCross.Plugins.SignaturePad.Droid.Extensions {
    public static class SignaturePadConfigurationExtensions {



        public static void SaveToIntent(this SignaturePadConfiguration config, Intent intent) {

            intent.PutExtra(() => config.BackgroundColor);
            intent.PutExtra(() => config.CancelText);
            intent.PutExtra(() => config.CaptionText);
            intent.PutExtra(() => config.CaptionTextColor);
            intent.PutExtra(() => config.ClearText);
            intent.PutExtra(() => config.ClearTextColor);
            intent.PutExtraEnum(() => config.ImageType);
            intent.PutExtra(() => config.PromptText);
            intent.PutExtra(() => config.PromptTextColor);
            intent.PutExtra(() => config.SaveText);
            intent.PutExtra(() => config.SignatureBackgroundColor);
            intent.PutExtra(() => config.SignatureLineColor);
            intent.PutExtra(() => config.StrokeColor);
            intent.PutExtra(() => config.StrokeWidth);
            intent.PutExtra(() => config.BackgroundImage);
            intent.PutExtra(() => config.BackgroundImageAlpha);
            intent.PutExtra(() => config.CropImage);

            intent.PutExtra(() => config.Points);

        }


        public static void LoadFromIntent(this SignaturePadConfiguration config, Intent intent) {

            intent.GetExtra(() => config.SignatureBackgroundColor);
            intent.GetExtra(() => config.SignatureLineColor);
            intent.GetExtra(() => config.StrokeColor);
            intent.GetExtra(() => config.StrokeWidth);

            intent.GetExtra(() => config.BackgroundColor);
            intent.GetExtra(() => config.CancelText);
            intent.GetExtra(() => config.CaptionText);
            intent.GetExtra(() => config.CaptionTextColor);
            intent.GetExtra(() => config.ClearText);
            intent.GetExtra(() => config.ClearTextColor);
            intent.GetExtraEnum(() => config.ImageType);
            intent.GetExtra(() => config.PromptText);
            intent.GetExtra(() => config.PromptTextColor);
            intent.GetExtra(() => config.CropImage);

            intent.GetExtra(() => config.SaveText);
            intent.GetExtra(() => config.BackgroundImage);
            intent.GetExtra(() => config.BackgroundImageAlpha);
            intent.GetExtra(() => config.Points);

        }

    }
}