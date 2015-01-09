using UnityEngine;
using System.Collections;

//Created by Jason Hein on Jan 9th 2015
//
//
//Can be found in the Tools Tab

#region changelog
/*
 * 
 */ 
#endregion


using UnityEngine;
using UnityEditor;
using System.Collections;

public class ObjectTools : EditorWindow
{
	bool lightMenuToggle;
	
	//Creates the window
	[MenuItem ("Tools/ShaderSettings")]
	static void Init () {
		ShaderSettings window = (ShaderSettings)EditorWindow.GetWindow(typeof(ShaderSettings));
	}
	
	//Draws the window
	void OnGUI ()
	{
		GUILayout.Label ("Shader Settings", EditorStyles.boldLabel);
		
		lightMenuToggle = EditorGUILayout.BeginToggleGroup ("Lights", lightMenuToggle);
		
		if (lightMenuToggle)
		{
			if(GUILayout.Button("No Shadows"))
			{
				SetShadowsOfAllLights(LightShadows.None);
			}
			if(GUILayout.Button("Soft Shadows"))
			{
				SetShadowsOfAllLights(LightShadows.Soft);
			}
			if(GUILayout.Button("Hard Shadows"))
			{
				SetShadowsOfAllLights(LightShadows.Hard);
			}
		}
		EditorGUILayout.EndToggleGroup ();
	}
	
	//Sets the shadows for all of the lights to none, soft or hard
	void SetShadowsOfAllLights (LightShadows lightShadows)
	{
		Light[] lights = Object.FindObjectsOfType<Light> ();
		foreach (Light light in lights)
		{
			light.shadows = lightShadows;
		}
	}
}



