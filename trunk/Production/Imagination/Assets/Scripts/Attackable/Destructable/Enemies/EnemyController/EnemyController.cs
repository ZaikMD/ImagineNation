/*
 * Created by Mathieu Elias November 30, 2014
 * Will take over certain actions of a specified group of enemies
 */ 
#region ChangeLog
/* 
 * 
 * 
 */
#endregion
using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour 
{
	public EnemyAI[] m_EnemyGroup;

	bool m_IsGroupAggro;

	public BaseControlType m_ControlType;
	GameObject m_Target;
	
	// Use this for initialization
	void Start () 
	{
		for (int i = 0; i < m_EnemyGroup.Length; i++)
		{
			m_EnemyGroup[i].SetController(this);
		}
	}

	void Update()
	{
		if (PauseScreen.IsGamePaused)
			return;

		if (m_IsGroupAggro)
		{
			if (m_Target = null)
			{
				m_ControlType.end ();
				m_IsGroupAggro = false;
			}

			m_ControlType.update ();
		}
	}

	public void AggroGroup(bool aggroGroup, GameObject target)
	{
		m_IsGroupAggro = aggroGroup;
		m_Target = target;

		if (m_IsGroupAggro)
			m_ControlType.start (m_EnemyGroup, target);

		else
			m_ControlType.end ();
	}
}
