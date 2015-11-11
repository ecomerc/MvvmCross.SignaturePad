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

namespace Acr.MvvmCross.Plugins.SignaturePad.Droid.Broadcast {
    using Android.Content;
    using Cirrious.CrossCore;

    /// <summary>
    /// Broadcast monitor.
    /// </summary>
    [BroadcastReceiver(Enabled = false)]
    [IntentFilter(new[] { "acr.mvvmcross.plugins.signaturepad.droid.SIGNATURE" })]
    public class SignatureReceiver : BroadcastReceiver {
        public override void OnReceive(Context context, Intent intent) {
            // Do stuff here
            var service = Mvx.Resolve<ISignatureService>() as DroidSignatureService;

            if (service != null) {
                var fileStore = intent.GetStringExtra("fileStore");
                var pointsString = intent.GetStringArrayExtra("points");

                if (string.IsNullOrWhiteSpace(fileStore) || pointsString.Length == 0) {
                    service.Cancel();

                } else {
                    var points = pointsString.Select(i => DrawPoint.Parse(i));
                    service.Complete(new SignatureResult(
                        false,
                        () => new FileStream(fileStore, FileMode.Open, FileAccess.Read, FileShare.Read),
                        points
                    ));
                }

            }
        }


    }
}