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

public class IOSBillingInitChecker 
{
	public delegate void BillingInitListener();

	BillingInitListener _listener;


	public IOSBillingInitChecker(BillingInitListener listener) {
		_listener = listener;

		if(IOSInAppPurchaseManager.Instance.IsStoreLoaded) {
			_listener();
		} else {

			IOSInAppPurchaseManager.OnStoreKitInitComplete += HandleOnStoreKitInitComplete;
			if(!IOSInAppPurchaseManager.Instance.IsWaitingLoadResult) {
				IOSInAppPurchaseManager.Instance.LoadStore();
			}
		}
	}

	void HandleOnStoreKitInitComplete (SA.Common.Models.Result obj) {
		IOSInAppPurchaseManager.OnStoreKitInitComplete -= HandleOnStoreKitInitComplete;
		_listener();
	}



}

