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

public class ISN_CheckUrlResult : SA.Common.Models.Result {

	private string _url;


	public ISN_CheckUrlResult(string checkedUrl):base() {
		_url = checkedUrl;
	}


	public ISN_CheckUrlResult(string checkedUrl, SA.Common.Models.Error error):base(error) {
		_url = checkedUrl;
	}



	public string url {
		get {
			return _url;
		}
	}
}
