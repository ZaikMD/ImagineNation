using UnityEngine;
using UnityEditor;
using System.Collections;

public class EnemyAITool : EditorWindow 
{
	GameObject m_SelectedEnemy;
	BaseBehaviour[] m_Behaviours;

	[MenuItem ("Tools/EnemyAI")]
	public static void ShowWindow()
	{
		if (Selection.activeGameObject == null)
		{
			Debug.Log("Select and enemy");
			return;
		}

		if (Selection.activeGameObject.GetComponent<EnemyAI>() == null)
		{
			Debug.Log("Select and enemy");
			return;
		}

		EditorWindow.GetWindow(typeof(EnemyAITool));
	}

	void OnGUI()
	{
		m_SelectedEnemy = Selection.activeGameObject;

		EditorGUILayout.LabelField (m_SelectedEnemy.name, EditorStyles.whiteLargeLabel);

		// Getting the selected enemies behaviours
		m_Behaviours = m_SelectedEnemy.GetComponentsInChildren<BaseBehaviour> ();

		int numbComponents;

		for (int i = 0; i < m_Behaviours.Length; i++)
		{

		}


	}

}
