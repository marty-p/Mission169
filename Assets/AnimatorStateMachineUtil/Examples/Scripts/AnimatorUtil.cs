using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;

[RequireComponent(typeof(Animator))]
public class AnimatorUtil : MonoBehaviour 
{

	public Animator animator;
	protected UnityEditor.Animations.AnimatorController animatorController;
	public StateMethod[] stateUpdateMethods;
	protected Dictionary<int,string> stateHashToMethods = new Dictionary<int, string>();

	void Awake () 
	{
		animator = GetComponent<Animator>();
		animatorController = animator.runtimeAnimatorController as UnityEditor.Animations.AnimatorController;
		
		foreach( StateMethod stateMethod in stateUpdateMethods )
		{
			stateHashToMethods[ Animator.StringToHash( stateMethod.state ) ] = stateMethod.method;
		}
		
	}
	
	void Update () 
	{
		int stateId = animator.GetCurrentAnimatorStateInfo(0).nameHash;
		
		if( stateHashToMethods.ContainsKey( stateId ) )
		{
		
			SendMessage( stateHashToMethods[stateId], SendMessageOptions.DontRequireReceiver );
		}
	}
	
	
}

[System.Serializable]
public class StateMethod
{
	public string state;
	public string method;
}