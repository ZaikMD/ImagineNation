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
		// Store the currently selected game object
		m_SelectedEnemy = Selection.activeGameObject;

		// Place a label with the enemy's name 
		EditorGUILayout.LabelField (m_SelectedEnemy.name, EditorStyles.whiteLargeLabel);

		// If we haven't done it yet then get the enemy info 
		if (!m_InfoRetrieved)
			GetEnemyInfo ();


		int loc = 0;
		int index = 0;
		for (int i = 0; i < m_tComponentNames.Count; i++)
		{

			// If we are in the location for the next behaviour
			if (m_BehaviourLocs[loc] == i)
			{
				//Place the next location label
				EditorGUILayout.LabelField (m_BehaviourNames[loc], EditorStyles.largeLabel);
				// Increment location
				loc++;
				if (loc >= 3)
					loc = 3;
			}
				index = 0;

			// Decide which type of component it is to decide which array of strings to use in the pop up
				if (m_tComponents[i] is BaseMovement)
				m_CompIndexes[i] = EditorGUILayout.Popup(m_tComponentNames[i],m_CompIndexes[i],Constants.MOVEMENT_COMPONENTS_ARRAY);
				else if (m_tComponents[i] is BaseCombat)
				m_CompIndexes[i] = EditorGUILayout.Popup(m_tComponentNames[i],m_CompIndexes[i],Constants.COMBAT_COMPONENTS_ARRAY);
				else if (m_tComponents[i] is BaseDeath)
				m_CompIndexes[i] = EditorGUILayout.Popup(m_tComponentNames[i],m_CompIndexes[i],Constants.DEATH_COMPONENTS_ARRAY);
				else if (m_tComponents[i] is BaseEnterCombat)
				m_CompIndexes[i] = EditorGUILayout.Popup(m_tComponentNames[i],m_CompIndexes[i],Constants.ENTER_COMBAT_COMPONENTS_ARRAY);
				else if (m_tComponents[i] is BaseLeavingCombat)
				m_CompIndexes[i] = EditorGUILayout.Popup(m_tComponentNames[i],m_CompIndexes[i],Constants.LEAVE_COMBAT_COMPONENTS_ARRAY);
				else if (m_tComponents[i] is BaseTargeting)
				m_CompIndexes[i] = EditorGUILayout.Popup(m_tComponentNames[i],m_CompIndexes[i],Constants.TARGETING_COMPONENTS_ARRAY);
				
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
				m_SelectedComponents[i] = Constants.MOVEMENT_COMPONENTS_ARRAY[m_CompIndexes[i]];
				componentsObj.AddComponent( Constants.MOVEMENT_COMPONENTS_ARRAY[m_CompIndexes[i]]);
			}

			else if (m_tComponents[i] is BaseCombat)	
			{
				m_SelectedComponents[i] = Constants.COMBAT_COMPONENTS_ARRAY[m_CompIndexes[i]];
				componentsObj.AddComponent(Constants.COMBAT_COMPONENTS_ARRAY[m_CompIndexes[i]]);
			}
			else if (m_tComponents[i] is BaseDeath)
			{
				m_SelectedComponents[i] = Constants.DEATH_COMPONENTS_ARRAY[m_CompIndexes[i]];

			}
			else if (m_tComponents[i] is BaseEnterCombat)
			{
				m_SelectedComponents[i] = Constants.ENTER_COMBAT_COMPONENTS_ARRAY[m_CompIndexes[i]];
				componentsObj.AddComponent(Constants.ENTER_COMBAT_COMPONENTS_ARRAY[m_CompIndexes[i]]);
			}
			else if (m_tComponents[i] is BaseLeavingCombat)
			{
				m_SelectedComponents[i] = Constants.LEAVE_COMBAT_COMPONENTS_ARRAY[m_CompIndexes[i]];
				componentsObj.AddComponent(Constants.LEAVE_COMBAT_COMPONENTS_ARRAY[m_CompIndexes[i]]);
			}
			else if (m_tComponents[i] is BaseTargeting)
			{
				m_SelectedComponents[i] = Constants.TARGETING_COMPONENTS_ARRAY[m_CompIndexes[i]];
				componentsObj.AddComponent(Constants.TARGETING_COMPONENTS_ARRAY[m_CompIndexes[i]]);
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
