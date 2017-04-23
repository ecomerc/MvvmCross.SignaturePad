﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace EcoMerc.MvvmCross.Plugins.SignaturePad {
    
    public interface ISignatureService {
	
		Task<SignatureResult> Request(SignaturePadConfiguration config = null, CancellationToken cancelToken = default(CancellationToken));
    }
}
