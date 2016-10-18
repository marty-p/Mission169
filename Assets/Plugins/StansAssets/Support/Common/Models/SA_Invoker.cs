////////////////////////////////////////////////////////////////////////////////
//  
// @module Assets Common Lib
// @author Osipov Stanislav (Stan's Assets) 
// @support support@stansassets.com
// @website https://stansassets.com
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System;
using System.Collections;


namespace SA.Common.Models {

	public class Invoker : MonoBehaviour {

		private Action _callback;

		public static Invoker Create() {
			return  new GameObject("Invoker").AddComponent<Invoker>();
		}

		public void StartInvoke(Action callback, float time) {
			_callback = callback;
			Invoke("TimeOut", time);
		}


		void TimeOut() {
			if(_callback != null) {
				_callback();
			}

			Destroy(gameObject);
		}
	}

}
