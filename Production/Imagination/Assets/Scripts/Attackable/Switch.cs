using UnityEngine;
using System.Collections;
/// <summary>
/// Lever.
/// 
/// Created by Zach Dubuc
/// 
/// This class will inherit from Attackable and will be responsible for toggling a specified lever 
/// and determining whether or not it will on a timer. The lever will switch on and off based on the OnHit
 /// function from Attackable and the timer will be set in the unity editor if one is needed.
/// </summary>
/// 
/// 19/09/14 Matthew Whitlaw EDIT: Added a getActive function
/// 
public class Switch : MonoBehaviour, Attackable
{

    public bool m_OnTimer;
    public float m_Timer;

	protected float m_SaveTimer;


	bool m_Active = false;
	// Use this for initialization
	void Start () 
    {
		m_SaveTimer = m_Timer;
	}
	
	// Update is called once per frame
	void Update () 
    {
		if(m_Active)
		{
			if(m_OnTimer)
			{
				if(m_Timer <= 0)
				{
					resetLever();
					Debug.Log(m_Active);
				}

				else
				{
					m_Timer -= Time.deltaTime;
				}
			}
		}
	}

	void resetLever()
	{
		m_Active = false;
		m_Timer = m_SaveTimer;
	}

    public void onHit(PlayerProjectile proj)
    {
		m_Active = true;
		Debug.Log (m_Active);
    }

    public void onHit(EnemyProjectile proj)
    {
		return;
    }

	protected virtual void onUse()
	{

	}

	public bool getActive()
	{
		return m_Active;
	}
}
