using UnityEngine;
using System.Collections;
/// <summary>
/// 
/// 
/// Created by Zach Dubuc
/// 
/// This class will inherit from Attackable and will be responsible for toggling a specified lever 
/// and determining whether or not it will on a timer. The lever will switch on and off based on the OnHit
 /// function from Attackable and the timer will be set in the unity editor if one is needed.
/// </summary>
/// 
/// 19/09/14 Matthew Whitlaw EDIT: Added a getActive function
/// 22/09/14 Zach Dubuc EDIT: Added a material when the switch is active/inactive
/// 
public class Switch : SwitchBaseClass, Attackable
{
    //Bools
    public bool m_OnTimer;
    public float m_Timer;
    bool m_Activated;

    //Save for the timer
	protected float m_SaveTimer;

    //Materials for the switch
	public Material m_ActiveMaterial;
	public Material m_InactiveMaterial;

    //Gameobject to hold the switch prefab that will change colours
	public GameObject m_SwitchChange;

	// Use this for initialization
	void Start () 
    {
		m_SaveTimer = m_Timer;
	}
	
	// Update is called once per frame
	void Update () 
    {
		if(m_Activated) //If the lever is activated
		{
			if(m_OnTimer) //If there is a timer
			{
                if (m_Timer <= 0) //If the timer is zero, reset the switch
				{
					resetLever();
				}

				else
				{
					m_Timer -= Time.deltaTime; //Otherwise decrement the timer
				}
			}
            m_SwitchChange.renderer.material = m_ActiveMaterial; //Change the colour of the switch is it is activate
		}

		else
		{
            m_SwitchChange.renderer.material = m_InactiveMaterial; //Else change the colour if it isn't activated
		}
	}

	void resetLever() //Reset variables
	{
        m_Activated = false;
		m_Timer = m_SaveTimer;
	}

    public void onHit(PlayerProjectile proj) //If the player hits the switch, set activated to true
    {
        m_Activated = true;
    }

    public void onHit(EnemyProjectile proj) //Ignore enemys
    {
		return;
    }

	protected virtual void onUse()
	{

	}

	public override bool getActive () //Returns whether or not the switch is active
	{
		return m_Activated;
	}

}
