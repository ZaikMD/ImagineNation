/*
 * Created by Mathieu Elias December 1, 2014
 * Base class for control types for groups of enemies
 */ 
#region ChangeLog
/* 
 * 
 * 
 */
#endregion
using UnityEngine;
using System.Collections;

public abstract class BaseControlType : MonoBehaviour 
{
	protected EnemyAI[] m_EnemyGroup;
	protected GameObject m_Target;

	public abstract void start(EnemyAI[] enemies, GameObject target);
	public abstract void update();
	public abstract void end();
}
