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

public class ShaderTools : EditorWindow
{
	bool lightMenuToggle;

	//Creates the window
	[MenuItem ("Tools/ShaderTools")]
	static void Init () {
		ShaderTools window = (ShaderTools)EditorWindow.GetWindow(typeof(ShaderTools));
	}

	//Draws the window
	void OnGUI ()
	{
		GUILayout.Label ("Shader Tools", EditorStyles.boldLabel);

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
