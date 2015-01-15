using UnityEngine;
using UnityEditor;
using System.Collections;

public class EnemyAITool : EditorWindow 
{
	GameObject m_SelectedEnemy;

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


	}

	void IdleBehaviourGUI()
	{

	}

	void ChaseBehaviourGUI()
	{
		
	}

	void AttackBehaviourGUI()
	{
		
	}


}
