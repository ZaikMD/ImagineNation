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
	bool cameraMenuToggle;
	
	//Creates the window
	[MenuItem ("Tools/ObjectTools")]
	static void Init () {
		ObjectTools window = (ObjectTools)EditorWindow.GetWindow(typeof(ObjectTools));
	}
	
	//Draws the window
	void OnGUI ()
	{
		GUILayout.Label ("Object Tools", EditorStyles.boldLabel);
		
		cameraMenuToggle = EditorGUILayout.BeginToggleGroup ("EdtitorCamera", cameraMenuToggle);
		
		if (cameraMenuToggle)
		{
			if(GUILayout.Button("Jump To Object"))
			{
				JumpEditorCameraToObject ();
			}
		}
		EditorGUILayout.EndToggleGroup ();
	}
	
	//Sets the shadows for all of the lights to none, soft or hard
	void JumpEditorCameraToObject ()
	{
		Vector3 position = SceneView.lastActiveSceneView.pivot;
		position.z -= 10.0f;
		SceneView.lastActiveSceneView.pivot = position;
		SceneView.lastActiveSceneView.Repaint();
	}
}



