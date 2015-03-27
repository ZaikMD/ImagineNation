/*
 * Created by Jason Hein October 31/2014
 * Designed to be an extension of the BaseEnemyAI script with a NavMash (for movement)
 */ 
#region ChangeLog
/* 
 * 
 */
#endregion

using UnityEngine;
using System.Collections;

//Require our NavMeshAgent for EnemyAI
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyWithMovement : EnemyAI
{
	

	//Public Transform to add PathNodes to the object
	public Transform[] m_PathNodes;

    const ScriptPauseLevel PAUSE_LEVEL = ScriptPauseLevel.Cutscene;

	protected override void Start ()
	{
		m_SFX = SFXManager.Instance;
		base.Start ();
	}

	//FixedUpdate to go by time
	public override void FixedUpdate ()
	{
		//Freeze enemies if the game is paused
        if (PauseScreen.shouldPause(PAUSE_LEVEL))
		{
			GetAgent.SetDestination(transform.position);
			return;
		}
		base.FixedUpdate ();
	}

	//Returns the nav mesh agent
	public NavMeshAgent GetAgent
	{
		get { return gameObject.GetComponent<NavMeshAgent>();}
	}

	protected override void onDeath ()
	{
		if(m_SFX != null)
		{
			m_SFX.playSound(this.transform, Sounds.EnemyDeath); 
		}
		base.onDeath ();

	}
}
