using MvvmCross.iOS.Views.Presenters;
using MvvmCross.Platform;
using System;
using System.Threading;
using System.Threading.Tasks;


namespace EcoMerc.MvvmCross.Plugins.SignaturePad.Touch {

	public class TouchSignatureService : ISignatureService {

		public Task<SignatureResult> Request(SignaturePadConfiguration config = null, CancellationToken cancelToken = default(CancellationToken)) {
			config = config ?? SignaturePadConfiguration.Default;
			var tcs = new TaskCompletionSource<SignatureResult>();
			var controller = new MvxSignatureController(config, x => tcs.TrySetResult(x));

			var presenter = Mvx.Resolve<IMvxIosViewPresenter>();
			presenter.PresentModalViewController(controller, true);
			cancelToken.Register(() => {
				tcs.TrySetCanceled();
				controller.DismissViewController(true, null);
			});
			return tcs.Task;
		}
	}
}