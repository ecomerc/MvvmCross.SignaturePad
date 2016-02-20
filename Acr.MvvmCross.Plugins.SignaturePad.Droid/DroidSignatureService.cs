using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Acr.MvvmCross.Plugins.SignaturePad.Droid.Extensions;


namespace Acr.MvvmCross.Plugins.SignaturePad.Droid {
    
	public class DroidSignatureService : ISignatureService {
		internal SignaturePadConfiguration CurrentConfig { get; private set; }
		private TaskCompletionSource<SignatureResult> tcs;


		internal void Complete(SignatureResult result) {
			this.tcs.TrySetResult(result);
		}


		internal void Cancel() {
			this.tcs.TrySetResult(new SignatureResult(true, null, null));
		}


		public virtual Task<SignatureResult> Request(SignaturePadConfiguration config, CancellationToken cancelToken) {
			CurrentConfig = config ?? SignaturePadConfiguration.Default;

			this.tcs = new TaskCompletionSource<SignatureResult>();
			cancelToken.Register(this.Cancel);
			//var activity = Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity;
            
            var intent = new Android.Content.Intent(Android.App.Application.Context, typeof(SignaturePadActivity));
            intent.AddFlags(Android.Content.ActivityFlags.NewTask); //NOT RECOMMENDED; BUT NECCESSARY
            intent.AddFlags(Android.Content.ActivityFlags.ClearWhenTaskReset);
            intent.AddFlags(Android.Content.ActivityFlags.NoHistory);

            CurrentConfig.SaveToIntent(intent);


            var recieverIntent = new Android.Content.IntentFilter("acr.mvvmcross.plugins.signaturepad.droid.SIGNATURE");
            Android.App.Application.Context.RegisterReceiver(new Broadcast.SignatureReceiver(), recieverIntent);
            Android.App.Application.Context.StartActivity(intent);

			return this.tcs.Task;
		}
    }
}