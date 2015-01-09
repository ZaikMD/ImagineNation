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
	GameObject m_SelectedObject;
	string m_SelectedObjectName = "";

	bool m_TransformMenuToggle;

	//Creates the window
	[MenuItem ("Tools/Object Tools")]
	static void Init () {
		ObjectTools window = (ObjectTools)EditorWindow.GetWindow(typeof(ObjectTools));
	}

	//Get the current object
	void OnSelectionChange() 
	{
		m_SelectedObject = Selection.activeGameObject;
		if (m_SelectedObject != null)
		{
			m_SelectedObjectName = m_SelectedObject.name;
		}
	}
	
	//Draws the window
	void OnGUI ()
	{
		if (m_SelectedObject != null)
		{
			//Name of selected object
			GUILayout.TextField (m_SelectedObjectName);

			//Transform tools
			m_TransformMenuToggle = EditorGUILayout.Foldout (m_TransformMenuToggle, "Transform Tools");
			if (m_TransformMenuToggle)
			{
				if(GUILayout.Button("Jump to camera"))
				{
					JumpObjectToCamera ();
				}
				if(GUILayout.Button("Set position between child objects"))
				{
					SetObjectPositionToChildObjectPosition ();
				}
			}
		}
		//Show that no gameobject is currently selected
		else
		{
			GUILayout.TextField ("Nothing Selected");
		}
	}
	
	//Sets the shadows for all of the lights to none, soft or hard
	void JumpObjectToCamera ()
	{
		Undo.RecordObject (m_SelectedObject.transform, "Jump Object To Camera");
		m_SelectedObject.transform.position = SceneView.lastActiveSceneView.pivot;
	}

	//Sets the objects position to the average of all child object
	void SetObjectPositionToChildObjectPosition ()
	{
		if (m_SelectedObject.transform.childCount > 0)
		{
			Vector3 pos = Vector3.zero;
			Transform[] childTransforms = m_SelectedObject.GetComponentsInChildren<Transform>();
			Transform[] oldParents = new Transform[childTransforms.Length];

			//Get the old child postions
			int index = 0;
			foreach (Transform child in childTransforms)
			{
				pos += child.position;
				oldParents[index] = child.parent;
				child.parent = null;
				index++;
			}

			//Set the selected parents positions
			m_SelectedObject.transform.position = pos /= childTransforms.Length;

			//Sets the children back to their original position
			index = 0;
			foreach (Transform child in childTransforms)
			{
				child.parent = oldParents[index];
				index++;
			}
		}
	}
}



