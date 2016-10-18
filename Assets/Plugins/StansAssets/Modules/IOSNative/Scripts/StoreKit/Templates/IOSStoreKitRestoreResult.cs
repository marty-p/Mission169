////////////////////////////////////////////////////////////////////////////////
//  
// @module IOS Native Plugin
// @author Osipov Stanislav (Stan's Assets) 
// @support support@stansassets.com
// @website https://stansassets.com
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

public class IOSStoreKitRestoreResult : SA.Common.Models.Result {


	//--------------------------------------
	// Initialize
	//--------------------------------------


	public  IOSStoreKitRestoreResult(SA.Common.Models.Error e) : base(e) {
	
	}

	public IOSStoreKitRestoreResult() : base()  {
	
	}

	public IOSTransactionErrorCode TransactionErrorCode {
		get {
			if(_Error != null) {
				return (IOSTransactionErrorCode) _Error.Code;
			} else {
				return IOSTransactionErrorCode.SKErrorNone;
			}

		}
	}

}
