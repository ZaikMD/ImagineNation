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
	
	int[] m_CompIndexes;
	
	string[] m_SelectedComponents;

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
		int index = 0;
		for (int i = 0; i < m_tComponentNames.Count; i++)
		{
			if (m_BehaviourLocs[loc] == i)
			{
				EditorGUILayout.LabelField (m_BehaviourNames[loc], EditorStyles.largeLabel);
				loc++;
				if (loc >= 3)
					loc = 3;
			}
				index = 0;

				if (m_tComponents[i] is BaseMovement)
					m_CompIndexes[i] = EditorGUILayout.Popup(m_tComponentNames[i],m_CompIndexes[i],m_MovementComponents);
				else if (m_tComponents[i] is BaseCombat)
					m_CompIndexes[i] = EditorGUILayout.Popup(m_tComponentNames[i],m_CompIndexes[i],m_CombatComponents);
				else if (m_tComponents[i] is BaseDeath)
					m_CompIndexes[i] = EditorGUILayout.Popup(m_tComponentNames[i],m_CompIndexes[i],m_DeathComponents);
				else if (m_tComponents[i] is BaseEnterCombat)
					m_CompIndexes[i] = EditorGUILayout.Popup(m_tComponentNames[i],m_CompIndexes[i],m_EnterCombatComponents);
				else if (m_tComponents[i] is BaseLeavingCombat)
					m_CompIndexes[i] = EditorGUILayout.Popup(m_tComponentNames[i],m_CompIndexes[i],m_LeaveCombatComponents);
				else if (m_tComponents[i] is BaseTargeting)
					m_CompIndexes[i] = EditorGUILayout.Popup(m_tComponentNames[i],m_CompIndexes[i],m_TargetingComponents);
				
		}

		if (GUILayout.Button ("OK"))
		{
			SetSelectedComponents();
			SetEnemyComponents();
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

		m_CompIndexes = new int[m_tComponents.Count];

		m_InfoRetrieved = true;
	}

	void InitStringOptions()
	{
		m_MovementComponents = new string[8];
		m_MovementComponents [0] = "ArcWhileMovingBackwards";
		m_MovementComponents [1] = "BuildUpChargeMovement";
		m_MovementComponents [2] = "ChargeMovement";
		m_MovementComponents [3] = "ChaseTargetMovement";
		m_MovementComponents [4] = "JumpBackMovement";
		m_MovementComponents [5] = "KnockedBackMovement";
		m_MovementComponents [6] = "MovementAroundNodes";
		m_MovementComponents [7] = "NoMovement";

		m_CombatComponents = new string[3];
		m_CombatComponents [0] = "BasicProjectileCombat";
		m_CombatComponents [1] = "CollisionCombat";
	

		m_DeathComponents = new string[1];
		m_DeathComponents[0] = "None";

		m_EnterCombatComponents = new string[2];
		m_EnterCombatComponents [0] = "ProximityAggro";

		m_LeaveCombatComponents = new string[2];
		m_LeaveCombatComponents [0] = "ProximityAggroLeave";

		m_TargetingComponents = new string[2];
		m_TargetingComponents [0] = "BasicTargeting";

	}
	
	void SetSelectedComponents()
	{
		m_SelectedComponents = new string[m_CompIndexes.Length];
		GameObject obj = m_SelectedEnemy.transform.FindChild ("Behaviours").FindChild("Components").gameObject;
		DestroyImmediate(obj, true);
		GameObject componentsObj = new GameObject("Components");
		componentsObj.transform.position = m_SelectedEnemy.transform.position;
		componentsObj.transform.parent = m_SelectedEnemy.transform.FindChild ("Behaviours").transform;

		for (int i = 0; i < m_tComponentNames.Count; i++)
		{			
			if (m_tComponents[i] is BaseMovement)	
			{
				m_SelectedComponents[i] = m_MovementComponents[m_CompIndexes[i]];
				componentsObj.AddComponent( m_MovementComponents[m_CompIndexes[i]]);
			}

			else if (m_tComponents[i] is BaseCombat)	
			{
				m_SelectedComponents[i] = m_CombatComponents[m_CompIndexes[i]];
				componentsObj.AddComponent(m_CombatComponents[m_CompIndexes[i]]);
			}
			else if (m_tComponents[i] is BaseDeath)
			{
				m_SelectedComponents[i] = m_DeathComponents[m_CompIndexes[i]];

			}
			else if (m_tComponents[i] is BaseEnterCombat)
			{
				m_SelectedComponents[i] = m_EnterCombatComponents[m_CompIndexes[i]];
				componentsObj.AddComponent(m_EnterCombatComponents[m_CompIndexes[i]]);
			}
			else if (m_tComponents[i] is BaseLeavingCombat)
			{
				m_SelectedComponents[i] = m_LeaveCombatComponents[m_CompIndexes[i]];
				componentsObj.AddComponent(m_LeaveCombatComponents[m_CompIndexes[i]]);
			}
			else if (m_tComponents[i] is BaseTargeting)
			{
				m_SelectedComponents[i] = m_TargetingComponents[m_CompIndexes[i]];
				componentsObj.AddComponent(m_TargetingComponents[m_CompIndexes[i]]);
			}
		}
	}

	void SetEnemyComponents()
	{
		int loc = 0;

		for (int i = 0; i < m_Behaviours.Length; i++) 
		{
			string[] comps = new string[m_Behaviours[i].numbComponents()];

			for (int j = 0; j < comps.Length; j++)
			{
				comps[j] = m_SelectedComponents[loc];
				loc++;
			}

			m_Behaviours[i].SetComponents(comps);
		}

		Close ();
	}
	
}
