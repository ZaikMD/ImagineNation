/*
 * Created by Mathieu Elias December 1, 2014
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

public abstract class BaseControlType : MonoBehaviour 
{
	protected EnemyAI[] m_EnemyGroup;
	protected GameObject m_Target;

	public abstract void start(EnemyAI[] enemies, GameObject target);
	public abstract void update();
	public abstract void end();
}
