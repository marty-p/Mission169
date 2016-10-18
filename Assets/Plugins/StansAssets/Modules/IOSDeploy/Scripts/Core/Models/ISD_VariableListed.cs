////////////////////////////////////////////////////////////////////////////////
//  
// @module IOS Deploy
// @author Stanislav Osipov (Stan's Assets) 
// @support support@stansassets.com
//
////////////////////////////////////////////////////////////////////////////////


using UnityEngine;
using System.Collections;

namespace SA.IOSDeploy {

	[System.Serializable]
	public class VariableListed {
		
		public bool IsOpen = true;

		public string DictKey = string.Empty;
		public string StringValue = string.Empty;
		public int IntegerValue = 0;
		public float FloatValue = 0;
		public bool BooleanValue = true;

		public PlistValueTypes Type = PlistValueTypes.String;
	}
}