/*
*DarknessDamage
*
*resposible for killing things slowly
*
*Created by: Kris Matis
*/

#region ChangeLog
/*
* 8/10/2014 Edit: Fully Commented - Kris Matis.
*
* 
*/
#endregion

using UnityEngine;
using System.Collections;

public class DarknessDamage : MonoBehaviour 
{
    const ScriptPauseLevel PAUSE_LEVEL = ScriptPauseLevel.Cutscene;

	public float m_HitInterval;
	private float m_Timer;

	void Start()
	{
		m_Timer = m_HitInterval;
	}

	//onTriggerEnter damage the destructable if there is one
    void OnTriggerEnter(Collider obj)
    {
		GameObject otherObject = obj.gameObject;

		//When an enemy enters a darkness, they die
		if (otherObject.tag == Constants.ENEMY_STRING)
		{
			//Kill the enemy
			Destructable destructable = otherObject.GetComponent<Destructable>();
			if (destructable != null)
			{
				destructable.instantKill();
			}
			return;
		}

		//Otherwise damage a player
        Destructable objDestructable = (Destructable)obj.GetComponentInChildren<Destructable>();
        if (objDestructable != null)
        {
            objDestructable.onHit(new EnemyProjectile(), Vector3.zero);
		}
    }

	//OnTriggerStay damage the destructable if there is one
    void OnTriggerStay(Collider obj)
    {
        if (PauseScreen.shouldPause(PAUSE_LEVEL)) { return; }

		m_Timer -= Time.deltaTime;
		if(m_Timer < 0)
		{
       		Destructable objDestructable = (Destructable)obj.GetComponentInChildren<Destructable>();
        	if (objDestructable != null)
       	    {
        	   	 objDestructable.onHit(new EnemyProjectile(), Vector3.zero);
        	}

			m_Timer = m_HitInterval;
		}
    }
}
