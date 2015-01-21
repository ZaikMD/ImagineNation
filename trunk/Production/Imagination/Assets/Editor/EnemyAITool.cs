using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class EnemyAITool : EditorWindow 
{
	GameObject m_SelectedEnemy;
	BaseBehaviour[] m_Behaviours;

	List<string> m_tComponentNames;
	List<BaseComponent> m_tComponents;

	int[] m_BehaviourLocs;
	string[] m_BehaviourNames;

	BaseComponent[] m_Components;
	string[] m_ComponentNames;

	bool m_InfoRetrieved = false;

	string[] m_MovementComponents;
	string[] m_CombatComponents;
	string[] m_DeathComponents;
	string[] m_EnterCombatComponents;
	string[] m_LeaveCombatComponents;
	string[] m_TargetingComponents;


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
		{
			GetEnemyInfo ();
			InitStringOptions();
		}

		int loc = 0;

		for (int i = 0; i < m_tComponentNames.Count; i++)
		{
			if (m_BehaviourLocs[loc] == i)
			{
				EditorGUILayout.LabelField (m_BehaviourNames[loc], EditorStyles.largeLabel);
				loc++;
				if (loc >= 3)
					loc = 3;
			}
				if (m_tComponents[i] is BaseMovement)
					EditorGUILayout.Popup(m_tComponentNames[i],1,m_MovementComponents);
				else if (m_tComponents[i] is BaseCombat)
					EditorGUILayout.Popup(m_tComponentNames[i],1,m_CombatComponents);
				else if (m_tComponents[i] is BaseDeath)
					EditorGUILayout.Popup(m_tComponentNames[i],1,m_DeathComponents);
				else if (m_tComponents[i] is BaseEnterCombat)
					EditorGUILayout.Popup(m_tComponentNames[i],1,m_EnterCombatComponents);
				else if (m_tComponents[i] is BaseLeavingCombat)
					EditorGUILayout.Popup(m_tComponentNames[i],1,m_LeaveCombatComponents);
				else if (m_tComponents[i] is BaseTargeting)
					EditorGUILayout.Popup(m_tComponentNames[i],1,m_TargetingComponents);
				
		}

	}

	void GetEnemyInfo()
	{
		//Init lists
		m_tComponentNames = new List<string> ();
		m_tComponents = new List<BaseComponent> ();

		//Init behaviour arrays
		m_BehaviourLocs = new int[5];
		m_BehaviourNames = new string[5];

		int prevLocs = 0;

		// Getting the selected enemies behaviours
		m_Behaviours = m_SelectedEnemy.GetComponentsInChildren<BaseBehaviour> ();
		
		for (int i = 0; i < m_Behaviours.Length; i++)
		{
			m_Behaviours[i].ComponentInfo(out m_ComponentNames, out m_Components);

			m_BehaviourNames[i] = m_Behaviours[i].BehaviourType();

			m_BehaviourLocs[i] = prevLocs;

			prevLocs += m_ComponentNames.Length;

			for (int j = 0; j < m_Components.Length; j++)
			{
				m_tComponentNames.Add( m_ComponentNames[j]);
				m_tComponents.Add(m_Components[j]);
			}
		}

		m_InfoRetrieved = true;
	}

	void InitStringOptions()
	{
		m_MovementComponents = new string[8];
		m_MovementComponents [0] = "Arc Backwards";
		m_MovementComponents [1] = "Build Charge";
		m_MovementComponents [2] = "Charge Target";
		m_MovementComponents [3] = "Chase Target";
		m_MovementComponents [4] = "Jump Back";
		m_MovementComponents [5] = "Knocked Back";
		m_MovementComponents [6] = "MoveAround Nodes";
		m_MovementComponents [7] = "None";

		m_CombatComponents = new string[3];
		m_CombatComponents [0] = "Basic Projectile";
		m_CombatComponents [1] = "Collision Combat";
		m_CombatComponents [2] = "None";

		m_DeathComponents = new string[1];
		m_DeathComponents[0] = "None";

		m_EnterCombatComponents = new string[2];
		m_EnterCombatComponents [0] = "Proximity";
		m_EnterCombatComponents [1] = "None";

		m_LeaveCombatComponents = new string[2];
		m_LeaveCombatComponents [0] = "Proximity";
		m_LeaveCombatComponents [1] = "None";

		m_TargetingComponents = new string[2];
		m_TargetingComponents [0] = "Basic Targeting";
		m_TargetingComponents [1] = "None";
	}
	
}
