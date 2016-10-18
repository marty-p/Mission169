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


using System.Text;
using System.Security.Cryptography;

namespace SA.Common.Util {

	public static class General {


		public static void Invoke(float time, Action callback) {

			var i = SA.Common.Models.Invoker.Create();
			i.StartInvoke(callback, time);
		}


		public static T ParseEnum<T>(string value) {
			try {
				T val = (T) Enum.Parse(typeof(T), value, true);
				return val;
			} catch(Exception ex) {
				Debug.LogWarning("Enum Parsing failed: " + ex.Message);
				return default(T);
			}
		}


		/// <summary>
		/// Current UTC timestamp format
		/// </summary>
		public static Int32 CurrentTimeStamp {
			get {
				return (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
			}
		}



		/// <summary>
		/// HMAC SHA256 hex key 
		/// </summary>
		public static string  HMAC(string key, string data) {
			var keyByte = ASCIIEncoding.UTF8.GetBytes(key);
			using (var hmacsha256 = new HMACSHA256(keyByte)) {
				hmacsha256.ComputeHash(ASCIIEncoding.UTF8.GetBytes(data));

				byte[] buff = hmacsha256.Hash;
				string sbinary = "";

				for (int i = 0; i < buff.Length; i++)
					sbinary += buff[i].ToString("X2"); /* hex format */
				return sbinary.ToLower();

			}
		}





		public static void CleanupInstallation() {

			#if UNITY_EDITOR
			SA.Common.Util.Files.DeleteFolder(SA.Common.Config.VERSION_INFO_PATH);

			SA.Common.Util.Files.DeleteFolder(SA.Common.Config.SETTINGS_PATH);
			SA.Common.Util.Files.DeleteFolder(SA.Common.Config.SETTINGS_REMOVE_PATH);


			SA.Common.Util.Files.DeleteFolder(SA.Common.Config.ANDROID_DESTANATION_PATH);
			SA.Common.Util.Files.DeleteFolder(SA.Common.Config.IOS_DESTANATION_PATH);


			UnityEditor.AssetDatabase.Refresh ();
			#endif

		}

	}

}
