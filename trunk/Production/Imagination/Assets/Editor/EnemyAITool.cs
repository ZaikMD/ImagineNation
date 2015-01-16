using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class EnemyAITool : EditorWindow 
{
	GameObject m_SelectedEnemy;
	BaseBehaviour[] m_Behaviours;

	List<string> m_tComponentNames;
	List<BaseComponent> m_tComponents;

	BaseComponent[] m_Components;
	string[] m_ComponentNames;

	bool m_InfoRetrieved = false;

	[MenuItem ("Tools/EnemyAI")]
	public static void ShowWindow()
	{
		// if we do not have an object in selection, write it to the console and leave
		if (Selection.activeGameObject == null)
		{
			Debug.Log("Select and enemy");
			return;
		}

		//If the object selected does not have enemyAI on it write it to the console and leave
		if (Selection.activeGameObject.GetComponent<EnemyAI>() == null)
		{
			Debug.Log("Select and enemy");
			return;
		}
		// Show the window
		EditorWindow.GetWindow(typeof(EnemyAITool));
	}

	void OnGUI()
	{
		m_SelectedEnemy = Selection.activeGameObject;

		EditorGUILayout.LabelField (m_SelectedEnemy.name, EditorStyles.whiteLargeLabel);

		if (!m_InfoRetrieved)
		GetEnemyInfo ();

		for (int i = 0; i < m_tComponentNames.Count; i++)
		{
			EditorGUILayout.LabelField (m_tComponentNames[i]);
		}

	}

	void GetEnemyInfo()
	{
		//Init lists
		m_tComponentNames = new List<string> ();
		m_tComponents = new List<BaseComponent> ();

		// Getting the selected enemies behaviours
		m_Behaviours = m_SelectedEnemy.GetComponentsInChildren<BaseBehaviour> ();
		
		for (int i = 0; i < m_Behaviours.Length; i++)
		{
			m_Behaviours[i].ComponentInfo(out m_ComponentNames, out m_Components);
			
			for (int j = 0; j < m_Components.Length; j++)
			{
				m_tComponentNames.Add( m_ComponentNames[j]);
				m_tComponents.Add(m_Components[j]);
			}
		}

		m_InfoRetrieved = true;
	}

}
