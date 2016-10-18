////////////////////////////////////////////////////////////////////////////////
//  
// @module IOS Deploy
// @author Stanislav Osipov (Stan's Assets) 
// @support support@stansassets.com
//
////////////////////////////////////////////////////////////////////////////////


using UnityEngine;
using System.IO;
using System.Collections.Generic;


#if UNITY_EDITOR
using UnityEditor;
#endif


namespace SA.IOSDeploy {

	#if UNITY_EDITOR
	[InitializeOnLoad]
	#endif
	public class ISD_Settings : ScriptableObject{

		public const string VERSION_NUMBER = "2.2";

		public bool IsfwSettingOpen;
		public bool IsLibSettingOpen;
		public bool IslinkerSettingOpne;
		public bool IscompilerSettingsOpen;
		public bool IsPlistSettingsOpen;
		public bool IsLanguageSettingOpen = true;

		public List<Framework> Frameworks =  new List<Framework>();
		public List<Lib> Libraries =  new List<Lib>();





		public List<string> compileFlags =  new List<string>();
		public List<string> linkFlags =  new List<string>();


		public List<Variable>  PlistVariables =  new List<Variable>();

		public List<string> langFolders = new List<string>();

		
		private const string ISDAssetName = "ISD_Settings";
		private const string ISDAssetExtension = ".asset";

		private static ISD_Settings instance;

		

		public static ISD_Settings Instance
		{
			get
			{
				if(instance == null)
				{
					instance = Resources.Load(ISDAssetName) as ISD_Settings;
					if(instance == null)
					{
						instance = CreateInstance<ISD_Settings>();
						#if UNITY_EDITOR


						SA.Common.Util.Files.CreateFolder(SA.Common.Config.SETTINGS_PATH);
						string fullPath = Path.Combine(Path.Combine("Assets", SA.Common.Config.SETTINGS_PATH), ISDAssetName + ISDAssetExtension );
						
						AssetDatabase.CreateAsset(instance, fullPath);
						#endif

					}
				}
				return instance;
			}
		}



		public bool ContainsFreamworkWithName(string name) {
			foreach(Framework f in ISD_Settings.Instance.Frameworks) {
				if(f.Name.Equals(name)) {
					return true;
				}
			}
			
			return false;
		}

		public bool ContainsPlistVarkWithName(string name) {
			foreach(Variable var in ISD_Settings.Instance.PlistVariables) {
				if(var.Name.Equals(name)) {
					return true;
				}
			}
			
			return false;
		}
		
		
		public bool ContainsLibWithName(string name) {
			foreach(Lib l in ISD_Settings.Instance.Libraries) {
				if(l.Name.Equals(name)) {
					return true;
				}
			}
			
			return false;
		}
							
	}
}
