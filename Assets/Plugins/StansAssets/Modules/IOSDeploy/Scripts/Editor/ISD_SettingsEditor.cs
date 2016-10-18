#define DISABLED


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
using UnityEditor;
using System.Reflection;
using System;


namespace SA.IOSDeploy {

	[CustomEditor(typeof(ISD_Settings))]
	public class SettingsEditor : UnityEditor.Editor {
		
		private string newFramework 			= string.Empty;
		private string NewLibrary 				= string.Empty;
		private string NewLinkerFlag 			= string.Empty;
		private string NewCompilerFlag 	= string.Empty;
		private string NewPlistValueName = string.Empty;
		private string newVarKey 					= string.Empty;
		private string NewLanguage 			= string.Empty;

		GUIContent SdkVersion   = new GUIContent("Plugin Version [?]", "This is Plugin version.  If you have problems or compliments please include this so we know exactly what version to look out for.");

		public override void OnInspectorGUI () {
			GUI.changed = false;
			EditorGUILayout.LabelField("IOS Deploy Settings", EditorStyles.boldLabel);
			EditorGUILayout.Space();

			#if DISABLED
			GUI.enabled = false;
			#endif

			Frameworks();
			EditorGUILayout.Space();
			Library ();
			EditorGUILayout.Space();
			LinkerFlags();
			EditorGUILayout.Space();
			CompilerFlags();
			EditorGUILayout.Space();
			PlistValues ();
			EditorGUILayout.Space();
			LanguageValues();
			EditorGUILayout.Space();
			AboutGUI();

			if(GUI.changed) {
				DirtyEditor();
			}
		}

		private void Frameworks() {
			ISD_Settings.Instance.IsfwSettingOpen = EditorGUILayout.Foldout(ISD_Settings.Instance.IsfwSettingOpen, "Frameworks");

			if(ISD_Settings.Instance.IsfwSettingOpen) {
				if (ISD_Settings.Instance.Frameworks.Count == 0) {

					EditorGUILayout.HelpBox("No Frameworks added", MessageType.None);
				}

				EditorGUI.indentLevel++; {	
					foreach(Framework framework in ISD_Settings.Instance.Frameworks) {
						EditorGUILayout.BeginVertical (GUI.skin.box);

						EditorGUILayout.BeginHorizontal();
						framework.IsOpen = EditorGUILayout.Foldout(framework.IsOpen, framework.Name);
						
						if(framework.IsOptional) {
							EditorGUILayout.LabelField("(Optional)");
						}
						bool ItemWasRemoved = DrawSortingButtons((object) framework, ISD_Settings.Instance.Frameworks);
						if(ItemWasRemoved) {
							return;
						}

						EditorGUILayout.EndHorizontal();
						if(framework.IsOpen) {
							EditorGUI.indentLevel++; {
								EditorGUILayout.BeginHorizontal();
								EditorGUILayout.LabelField("Optional");
								framework.IsOptional = EditorGUILayout.Toggle (framework.IsOptional);
								EditorGUILayout.EndHorizontal();
							}EditorGUI.indentLevel--;
						}
						EditorGUILayout.EndVertical ();
					}
				} EditorGUI.indentLevel--;
				EditorGUILayout.Space();

				EditorGUILayout.BeginVertical (GUI.skin.box);
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Add New Framework", GUILayout.Width(120));
				newFramework = EditorGUILayout.TextField(newFramework);
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.EndVertical ();

				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.Space();

				if(GUILayout.Button("Add",  GUILayout.Width(100))) {
					if(!ISD_Settings.Instance.ContainsFreamworkWithName(newFramework) && newFramework.Length > 0) {
						Framework f =  new Framework();
						f.Name = newFramework;
						ISD_Settings.Instance.Frameworks.Add(f);
						newFramework = string.Empty;
					}				
				}
				EditorGUILayout.EndHorizontal();
			}
		}

		private void Library () {
			ISD_Settings.Instance.IsLibSettingOpen = EditorGUILayout.Foldout(ISD_Settings.Instance.IsLibSettingOpen, "Libraries");

			if(ISD_Settings.Instance.IsLibSettingOpen){
				if (ISD_Settings.Instance.Libraries.Count == 0) {
					EditorGUILayout.HelpBox("No Libraries added", MessageType.None);
				}

				EditorGUI.indentLevel++; {
					foreach(Lib lib in ISD_Settings.Instance.Libraries) {	
						EditorGUILayout.BeginVertical (GUI.skin.box);
						
						EditorGUILayout.BeginHorizontal();
						lib.IsOpen = EditorGUILayout.Foldout(lib.IsOpen, lib.Name);
						if(lib.IsOptional) {
							EditorGUILayout.LabelField("(Optional)");
						}
			
						bool ItemWasRemoved = DrawSortingButtons((object) lib, ISD_Settings.Instance.Libraries);
						if(ItemWasRemoved) {
							return;
						}					
						EditorGUILayout.EndHorizontal();
						if(lib.IsOpen) {						
							EditorGUI.indentLevel++; {							
								EditorGUILayout.BeginHorizontal();
								EditorGUILayout.LabelField("Optional");
								lib.IsOptional = EditorGUILayout.Toggle (lib.IsOptional);
								EditorGUILayout.EndHorizontal();						
							}EditorGUI.indentLevel--;
						}
						EditorGUILayout.EndVertical ();					
					}
				}EditorGUI.indentLevel--;
				
				EditorGUILayout.Space();

				EditorGUILayout.BeginVertical (GUI.skin.box);
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Add New Library", GUILayout.Width(120));
				NewLibrary = EditorGUILayout.TextField(NewLibrary);
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.EndVertical ();

				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.Space();
				if(GUILayout.Button("Add",  GUILayout.Width(100))) {
					if(!ISD_Settings.Instance.ContainsLibWithName(NewLibrary) && NewLibrary.Length > 0 ) {
						Lib lib = new Lib();
						lib.Name = NewLibrary;
						ISD_Settings.Instance.Libraries.Add(lib);
						NewLibrary = string.Empty;
					}
				}
				EditorGUILayout.EndHorizontal();
			}
		}


		private void LinkerFlags() {
			ISD_Settings.Instance.IslinkerSettingOpne = EditorGUILayout.Foldout(ISD_Settings.Instance.IslinkerSettingOpne, "Linker Flags");
			
			if(ISD_Settings.Instance.IslinkerSettingOpne) {
				if (ISD_Settings.Instance.linkFlags.Count == 0) {				
					EditorGUILayout.HelpBox("No Linker Flags added", MessageType.None);
				}

				foreach(string flasg in ISD_Settings.Instance.linkFlags) {			
					EditorGUILayout.BeginVertical (GUI.skin.box);				
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.SelectableLabel(flasg, GUILayout.Height(18));
					EditorGUILayout.Space();
					
					bool pressed  = GUILayout.Button("x",  EditorStyles.miniButton, GUILayout.Width(20));
					if(pressed) {
						ISD_Settings.Instance.linkFlags.Remove(flasg);
						return;
					}
					EditorGUILayout.EndHorizontal();
					
					EditorGUILayout.EndVertical ();				
				}

				EditorGUILayout.Space();
				EditorGUILayout.BeginHorizontal();
				
				EditorGUILayout.LabelField("Add New Flag");
				NewLinkerFlag = EditorGUILayout.TextField(NewLinkerFlag, GUILayout.Width(200));
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.Space();
				if(GUILayout.Button("Add",  GUILayout.Width(100))) {
					if(!ISD_Settings.Instance.linkFlags.Contains(NewLinkerFlag) && NewLinkerFlag.Length > 0) {
						ISD_Settings.Instance.linkFlags.Add(NewLinkerFlag);
						NewLinkerFlag = string.Empty;
					}
				}
				EditorGUILayout.EndHorizontal();
			}
		}

		private void CompilerFlags() {
			ISD_Settings.Instance.IscompilerSettingsOpen = EditorGUILayout.Foldout(ISD_Settings.Instance.IscompilerSettingsOpen, "Compiler Flags");
			
			if(ISD_Settings.Instance.IscompilerSettingsOpen) {
				if (ISD_Settings.Instance.compileFlags.Count == 0) {
					EditorGUILayout.HelpBox("No Linker Flags added", MessageType.None);
				}

				foreach(string flasg in ISD_Settings.Instance.compileFlags) {
					EditorGUILayout.BeginVertical (GUI.skin.box);
					
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.SelectableLabel(flasg, GUILayout.Height(18));
					
					EditorGUILayout.Space();
					
					bool pressed  = GUILayout.Button("x",  EditorStyles.miniButton, GUILayout.Width(20));
					if(pressed) {
						ISD_Settings.Instance.compileFlags.Remove(flasg);
						return;
					}
					EditorGUILayout.EndHorizontal();
					EditorGUILayout.EndVertical ();
				}

				EditorGUILayout.Space();
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Add New Flag");
				NewCompilerFlag = EditorGUILayout.TextField(NewCompilerFlag, GUILayout.Width(200));
				EditorGUILayout.EndHorizontal();
				
				EditorGUILayout.BeginHorizontal();
				
				EditorGUILayout.Space();
				
				if(GUILayout.Button("Add",  GUILayout.Width(100))) {
					if(!ISD_Settings.Instance.compileFlags.Contains(NewCompilerFlag) && NewCompilerFlag.Length > 0) {
						ISD_Settings.Instance.compileFlags.Add(NewCompilerFlag);
						NewCompilerFlag = string.Empty;
					}
				}
				EditorGUILayout.EndHorizontal();
			}
		}

		private void PlistValues ()	{
			ISD_Settings.Instance.IsPlistSettingsOpen = EditorGUILayout.Foldout(ISD_Settings.Instance.IsPlistSettingsOpen, "Plist values");
			
			if(ISD_Settings.Instance.IsPlistSettingsOpen) {
				if (ISD_Settings.Instance.PlistVariables.Count == 0) {
					EditorGUILayout.HelpBox("No Plist values added", MessageType.None);
				}
				EditorGUI.indentLevel++; {	
					foreach(Variable var in ISD_Settings.Instance.PlistVariables) {
						EditorGUILayout.BeginVertical (GUI.skin.box);
						
						EditorGUILayout.BeginHorizontal();
						var.IsOpen = EditorGUILayout.Foldout(var.IsOpen, var.Name);

						EditorGUILayout.LabelField("(" + var.Type.ToString() +  ")");
						
						bool ItemWasRemoved = DrawSortingButtons((object) var, ISD_Settings.Instance.PlistVariables);
						if(ItemWasRemoved) {
							return;
						}
						EditorGUILayout.EndHorizontal();
						if(var.IsOpen) {						
							EditorGUI.indentLevel++; {
								
								EditorGUILayout.BeginHorizontal();
								EditorGUILayout.LabelField("Type");
								var.Type = (PlistValueTypes) EditorGUILayout.EnumPopup (var.Type);
								EditorGUILayout.EndHorizontal();

								if(var.Type == PlistValueTypes.String || var.Type == PlistValueTypes.Integer || var.Type == PlistValueTypes.Float || var.Type == PlistValueTypes.Boolean) {
									EditorGUILayout.BeginHorizontal();
									EditorGUILayout.LabelField("Value");								
									switch(var.Type) {
									case PlistValueTypes.Boolean:
										var.BooleanValue	 = EditorGUILayout.Toggle (var.BooleanValue);
										break;									
									case PlistValueTypes.Float:
										var.FloatValue = EditorGUILayout.FloatField(var.FloatValue);
										break;									
									case PlistValueTypes.Integer:
										var.IntegerValue = EditorGUILayout.IntField (var.IntegerValue);
										break;									
									case PlistValueTypes.String:
										var.StringValue = EditorGUILayout.TextField (var.StringValue);
										break;
									}
									EditorGUILayout.EndHorizontal();
								}
								if(var.Type == PlistValueTypes.Array) {
									DrawArrayValues(var);
								}
								if(var.Type == PlistValueTypes.Dictionary) {
									DrawDictionaryValues(var);
								}
							}EditorGUI.indentLevel--;
						}
						EditorGUILayout.EndVertical ();
					}
					EditorGUILayout.Space();
				} EditorGUI.indentLevel--;

				EditorGUILayout.BeginVertical (GUI.skin.box);
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("Add New Variable");
				NewPlistValueName = EditorGUILayout.TextField(NewPlistValueName);
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.Space();
				if(GUILayout.Button("Add",  GUILayout.Width(100))) {
					if (NewPlistValueName.Length > 0) {
						Variable var = new Variable ();
						var.Name = NewPlistValueName;
						ISD_Settings.Instance.PlistVariables.Add (var);
					}
					NewPlistValueName = string.Empty;
				}
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.Space();
				EditorGUILayout.EndVertical ();
			}
		}

		private void DrawArrayValues (Variable var) {
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Values Type");

			PlistValueTypes type = (PlistValueTypes) EditorGUILayout.EnumPopup (var.ArrayType);

			if(type == PlistValueTypes.Array ||type == PlistValueTypes.Dictionary) {
				type = var.ArrayType;
			}
			var.ArrayType = type;

			EditorGUILayout.EndHorizontal();
			var.IsListOpen = EditorGUILayout.Foldout(var.IsListOpen, "Array Values");
			/*	foreach(KeyValuePair<string, ISD_VariableListed> pair  in var.DictionaryValue) {
				EditorGUI.indentLevel++; {				
					EditorGUILayout.BeginHorizontal();
					ISD_VariableListed v = pair.Value;

					v.IsOpen = EditorGUILayout.Foldout(v.IsOpen, v.DictKey);
					EditorGUILayout.LabelField("(" + var.Type.ToString() +  ")");
					bool removed = DrawSrotingButtons((object) v, var.ArrayValue);
					if(removed) {
						return;
					}*/

			if(var.IsListOpen) {
				foreach(VariableListed v  in var.ArrayValue) {
					EditorGUILayout.BeginHorizontal();
					GUI.enabled = false;
					v.Type = (PlistValueTypes) EditorGUILayout.EnumPopup (var.ArrayType);
					GUI.enabled = true;
					DrawValueFiled(v);

					bool removed = DrawSortingButtons((object) v, var.ArrayValue);
					if(removed) {
						return;
					}
					EditorGUILayout.EndHorizontal();
				}
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.Space();
				if(GUILayout.Button("Add New Value",  GUILayout.Width(100))) {
					var.ArrayValue.Add(new VariableListed());
				}
				EditorGUILayout.EndHorizontal();
			}
			EditorGUILayout.Space();
		}

		private void DrawDictionaryValues (Variable var) {
			var.IsListOpen = EditorGUILayout.Foldout(var.IsListOpen, "Dictionary Values");

			if(var.IsListOpen) {
				foreach(KeyValuePair<string, VariableListed> pair  in var.DictionaryValue) {
					EditorGUI.indentLevel++; {					
					EditorGUILayout.BeginHorizontal();
						VariableListed v = pair.Value;

						v.IsOpen = EditorGUILayout.Foldout(v.IsOpen, v.DictKey);
						bool removed = DrawSortingButtons((object) v, var.ArrayValue);
						if(removed) {
							return;
						}
						EditorGUILayout.EndHorizontal();

						if(v.IsOpen) {
							//EditorGUILayout.BeginHorizontal();
							v.Type = (PlistValueTypes) EditorGUILayout.EnumPopup (v.Type);
							Debug.Log ("v.Type " + v.Type);
							DrawValueFiled(v);    //VALUE
							//EditorGUILayout.EndHorizontal();
						}
					} EditorGUI.indentLevel--;
				}
				
				EditorGUILayout.BeginHorizontal();

				EditorGUILayout.LabelField("Add New Value With Key:");
				newVarKey = EditorGUILayout.TextField(newVarKey);

				EditorGUILayout.Space();
				if(GUILayout.Button("Add",  GUILayout.Width(100))) {
					if(newVarKey.Length > 0) {
						VariableListed newDictValue =  new VariableListed();
						newDictValue.DictKey = newVarKey;
						var.AddVarToDictionary(newDictValue);
					}
				}
				EditorGUILayout.EndHorizontal();
			}
			EditorGUILayout.Space();
		}



		private void DrawValueFiled(VariableListed var, string caption = "") {
			switch(var.Type) {
				case PlistValueTypes.Boolean: 
					var.BooleanValue	 = EditorGUILayout.Toggle (var.BooleanValue);
					break;				
				case PlistValueTypes.Float:
					var.FloatValue = EditorGUILayout.FloatField("Test", var.FloatValue);
					break;				
				case PlistValueTypes.Integer:
					var.IntegerValue = EditorGUILayout.IntField (var.IntegerValue);
					break;				
				case PlistValueTypes.String:
					var.StringValue = EditorGUILayout.TextField (var.StringValue);
					break;
				//Creating array and dictionary for info.plist
				case PlistValueTypes.Array:
				//TODO
					break;
				case PlistValueTypes.Dictionary:
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField("something");
					EditorGUILayout.EndHorizontal();

				//TODO
					break;
			}
		}

		private void AboutGUI() {
			GUI.enabled = true;
			EditorGUILayout.HelpBox("About the Plugin", MessageType.None);
			EditorGUILayout.Space();
		

			SA.Common.Editor.Tools.SelectableLabelField(SdkVersion,   ISD_Settings.VERSION_NUMBER);
			SA.Common.Editor.Tools.SupportMail();

			SA.Common.Editor.Tools.DrawSALogo();
			#if DISABLED
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Note: This version of IOS Deploy designed for Stan's Assets");
			EditorGUILayout.LabelField("plugins internal use only. If you want to use IOS Deploy  ");
			EditorGUILayout.LabelField("for your project needs, please, ");
			EditorGUILayout.LabelField("purchase a copy of IOS Deploy plugin.");
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.Space();
			
			if(GUILayout.Button("Documentation",  GUILayout.Width(150))) {
				Application.OpenURL("https://goo.gl/sOJFXJ");
			}
			
			if(GUILayout.Button("Purchase",  GUILayout.Width(150))) {
				Application.OpenURL("https://goo.gl/Nqbuuv");
			}		
			EditorGUILayout.EndHorizontal();
			#endif
		}
		
		private void SelectableLabelField(GUIContent label, string value) {
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(label, GUILayout.Width(180), GUILayout.Height(16));
			EditorGUILayout.SelectableLabel(value, GUILayout.Height(16));
			EditorGUILayout.EndHorizontal();
		}

		private bool DrawSortingButtons(object currentObject, IList ObjectsList) {		
			int ObjectIndex = ObjectsList.IndexOf(currentObject);
			if(ObjectIndex == 0) {
				GUI.enabled = false;
			} 

			bool up 		= GUILayout.Button("↑", EditorStyles.miniButtonLeft, GUILayout.Width(20));
			if(up) {
				object c = currentObject;
				ObjectsList[ObjectIndex]  		= ObjectsList[ObjectIndex - 1];
				ObjectsList[ObjectIndex - 1] 	=  c;
			}
			
			if(ObjectIndex >= ObjectsList.Count -1) {
				GUI.enabled = false;
			} else {
				GUI.enabled = true;
			}
			
			bool down = GUILayout.Button("↓", EditorStyles.miniButtonMid, GUILayout.Width(20));
			if(down) {
				object c = currentObject;
				ObjectsList[ObjectIndex] =  ObjectsList[ObjectIndex + 1];
				ObjectsList[ObjectIndex + 1] = c;
			}

			GUI.enabled = true;
			bool r = GUILayout.Button("-", EditorStyles.miniButtonRight, GUILayout.Width(20));
			if(r) {
				Debug.Log ("remove " + currentObject.ToString());
				ObjectsList.Remove(currentObject);
			}
			
			return r;
		}

		private void LanguageValues ()	{
			ISD_Settings.Instance.IsLanguageSettingOpen = EditorGUILayout.Foldout(ISD_Settings.Instance.IsLanguageSettingOpen, "Languages");
			
			if(ISD_Settings.Instance.IsLanguageSettingOpen)	 {
				if (ISD_Settings.Instance.langFolders.Count == 0)	{
					EditorGUILayout.HelpBox("No Languages added", MessageType.None);
				}

				foreach(string lang in ISD_Settings.Instance.langFolders) 	{
					EditorGUILayout.BeginVertical (GUI.skin.box);
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.SelectableLabel(lang, GUILayout.Height(18));
					EditorGUILayout.Space();
					
					bool pressed  = GUILayout.Button("x",  EditorStyles.miniButton, GUILayout.Width(20));
					if(pressed) 	{
						ISD_Settings.Instance.langFolders.Remove(lang);
						return;
					}
					
					EditorGUILayout.EndHorizontal();
					EditorGUILayout.EndVertical ();
				}
				EditorGUILayout.Space();

				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Add New Language");
				NewLanguage = EditorGUILayout.TextField(NewLanguage, GUILayout.Width(200));
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();			
				EditorGUILayout.Space();
				
				if(GUILayout.Button("Add",  GUILayout.Width(100)))	{
					if(!ISD_Settings.Instance.langFolders.Contains(NewLanguage) && NewLanguage.Length > 0)	{
						ISD_Settings.Instance.langFolders.Add(NewLanguage);
						NewLanguage = string.Empty;
					}				
				}
				EditorGUILayout.EndHorizontal();
			}
		}

		private static void DirtyEditor() {
			#if UNITY_EDITOR
			EditorUtility.SetDirty(ISD_Settings.Instance);
			#endif
		}
	}

}