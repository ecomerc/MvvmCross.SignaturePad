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

namespace Acr.MvvmCross.Plugins.SignaturePad.Droid.Extensions {
    public static class SignaturePadConfigurationExtensions {



        public static void SaveToIntent(this SignaturePadConfiguration config, Intent intent) {
            intent.PutExtra(() => config.BackgroundColor.ARGB);
            intent.PutExtra(() => config.CancelText);
            intent.PutExtra(() => config.CaptionText);
            intent.PutExtra(() => config.CaptionTextColor.ARGB);
            intent.PutExtra(() => config.ClearText);
            intent.PutExtra(() => config.ClearTextColor.ARGB);
            intent.PutExtraEnum(() => config.ImageType);
            intent.PutExtra(() => config.PromptText);
            intent.PutExtra(() => config.PromptTextColor.ARGB);
            intent.PutExtra(() => config.SaveText);
            intent.PutExtra(() => config.SignatureBackgroundColor.ARGB);
            intent.PutExtra(() => config.SignatureLineColor.ARGB);
            intent.PutExtra(() => config.StrokeColor.ARGB);
            intent.PutExtra(() => config.StrokeWidth);
            intent.PutExtra(() => config.BackgroundImage);
            intent.PutExtra(() => config.BackgroundImageAlpha);

        }


        public static void LoadFromIntent(this SignaturePadConfiguration config, Intent intent) {
            intent.GetExtra(() => config.BackgroundColor.ARGB);
            intent.GetExtra(() => config.CancelText);
            intent.GetExtra(() => config.CaptionText);
            intent.GetExtra(() => config.CaptionTextColor.ARGB);
            intent.GetExtra(() => config.ClearText);
            intent.GetExtra(() => config.ClearTextColor.ARGB);
            intent.GetExtraEnum(() => config.ImageType);
            intent.GetExtra(() => config.PromptText);
            intent.GetExtra(() => config.PromptTextColor.ARGB);
            intent.GetExtra(() => config.SaveText);
            intent.GetExtra(() => config.SignatureBackgroundColor.ARGB);
            intent.GetExtra(() => config.SignatureLineColor.ARGB);
            intent.GetExtra(() => config.StrokeColor.ARGB);
            intent.GetExtra(() => config.StrokeWidth);
            intent.GetExtra(() => config.BackgroundImage);
            intent.GetExtra(() => config.BackgroundImageAlpha);
        }

    }
}