using UnityEngine;
using System.Collections;

public class CubeSurface : MonoBehaviour 
{
	public Color mainColor ;

	private Material material;

	void Start () 
	{
		material = GetComponent<Renderer>().material = GetComponent<Renderer>().material;
	}
	
	void Update () 
	{
		material.color = mainColor;
	}
}
