////////////////////////////////////////////////////////////////////////////////
//  
// @module IOS Deploy
// @author Stanislav Osipov (Stan's Assets) 
// @support support@stansassets.com
//
////////////////////////////////////////////////////////////////////////////////


using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace SA.IOSDeploy {

	[System.Serializable]
	public class Variable  {
		//Editor Use Only
		public bool IsOpen = true;
		public bool IsListOpen = true;

		public string Name;
		public PlistValueTypes Type         = PlistValueTypes.String;
		public PlistValueTypes ArrayType = PlistValueTypes.String;

		public string StringValue = string.Empty;
		public int IntegerValue = 0;
		public float FloatValue = 0;
		public bool BooleanValue = true;
		
		public List<VariableListed> ArrayValue =  new List<VariableListed>();

		//Dictionary is not serializeable type :(
		public List<VariableListed> DictValues =  new List<VariableListed>();

		public void AddVarToDictionary(VariableListed v) {
			bool valid = true;
			foreach(VariableListed var in DictValues) {
				if(var.DictKey.Equals(v.DictKey)) {
					valid = false;
					break;
				}
			}

			if(valid) {
				DictValues.Add(v);
			}
		}

		public Dictionary<string, VariableListed> DictionaryValue {
			get {
				Dictionary<string, VariableListed> dict =  new Dictionary<string, VariableListed>();

				foreach(VariableListed var in DictValues) { 
					dict.Add(var.DictKey, var);
				}

				return dict;
			}
		}
	}
}