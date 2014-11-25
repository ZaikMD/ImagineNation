﻿using UnityEngine;
using System.Collections;
/// <summary>
/// Combat item.
/// Created by Zach Dubuc
/// 
/// Handles the combos for players, and calls the attack function from the attacks
/// 
/// </summary>
public class Weapon : MonoBehaviour
{
    SFXManager m_SFX;
    AnimationState m_AnimState;

    BaseAttack m_CurrentAttack;//The current Attack
    BaseAttack m_PreviousAttack;

    public GameObject m_LightProjectilePrefab;
    public GameObject m_HeavyProjectilePrefab;

	float m_DownTime = 0.5f;
	float DOWN_TIME;

	bool m_FinishedCombo = false;

    string m_Inputs;
    //The constants for the inputs of the attacks, as well as the 
    //combos the players can do. L = light attack  H = Heavy attack
    const string L = " L";
    const string LL = " L L";
    const string DOUBLEHIT = " L L L";
    const string LH = " L H";
    const string AOE = " L L H";
    const string FRONTLINE = " L H H";
    const string H = " H";
    const string HH = " H H";
    const string FRONTCONE = " H H H";

    string m_LastInput;

    AcceptInputFrom m_ReadInput;
	// Use this for initialization
	void Start () 
    {
        m_ReadInput = gameObject.GetComponent<AcceptInputFrom>();

        m_SFX = GameObject.FindGameObjectWithTag(Constants.SOUND_MANAGER).GetComponent<SFXManager>();
        m_AnimState = GetComponent<AnimationState>();
		DOWN_TIME = m_DownTime;
	}
	
	// Update is called once per frame
	void Update () 
    {
		if(m_FinishedCombo)
		{
			if(m_DownTime > 0.0f)
			{
				m_DownTime -= Time.deltaTime;
			}

			else
			{
				m_DownTime = DOWN_TIME;
				m_FinishedCombo = false;
			}
		}
        updateAttacks();
	}

    void setCurrentAttack()
    {
		if (InputManager.getAttackDown (m_ReadInput.ReadInputFrom) || InputManager.getCharacterSwitchDown(m_ReadInput.ReadInputFrom))
		{
			if(InputManager.getAttackDown(m_ReadInput.ReadInputFrom))
			{
				m_Inputs += L;
				m_LastInput = L;
			}

			if(InputManager.getCharacterSwitchDown(m_ReadInput.ReadInputFrom))
			{
				m_Inputs += H;
				m_LastInput = H;
			}

		
			m_PreviousAttack = m_CurrentAttack;
			switch (m_Inputs) 
			{
				case L:
				{
					LightAttack attack = new LightAttack ();
					attack.loadPrefab (m_LightProjectilePrefab);
					attack.startAttack (transform.position, transform.rotation);
					m_CurrentAttack = attack;
					m_LastInput = "";
					}

					break;

				case LL:
				{
					LightAttack attack = new LightAttack ();
					attack.loadPrefab (m_LightProjectilePrefab);
					attack.startAttack (transform.position, transform.rotation);
					m_CurrentAttack = attack;
					m_LastInput = "";
				}
					break;

				case DOUBLEHIT:
				{
 
				}
					break;

				case LH:
				{
					HeavyAttack attack = new HeavyAttack ();
					attack.loadPrefab (m_HeavyProjectilePrefab);
					attack.startAttack (transform.position, transform.rotation);
					m_CurrentAttack = attack;
					m_LastInput = "";

				}
					break;

				case AOE:
				{					
					SpecialAttack attack = new SpecialAttack();
					attack.loadPrefab (m_LightProjectilePrefab);
					attack.startAttack(transform.position, transform.rotation);
					m_CurrentAttack = attack;
					m_LastInput = "";
					m_Inputs = "";
					m_FinishedCombo = true;
				}
					break;

				case FRONTLINE:
				{
					m_FinishedCombo = true;
				}
					break;

				case H:
				{
					HeavyAttack attack = new HeavyAttack ();
					attack.loadPrefab (m_HeavyProjectilePrefab);
					attack.startAttack (transform.position, transform.rotation);
					m_CurrentAttack = attack;
					m_LastInput = "";
				}
					break;

				case HH:
				{
					HeavyAttack attack = new HeavyAttack ();
					attack.loadPrefab (m_HeavyProjectilePrefab);
					attack.startAttack (transform.position, transform.rotation);
					m_CurrentAttack = attack;
					m_LastInput = "";	
				}
					break;

				case FRONTCONE:
				{
					m_FinishedCombo = true;
				}
					break;

				default:
				{
					m_Inputs = m_LastInput;
				}
					break;
			}
		}
    }

    void updateAttacks()
    {

        if (m_PreviousAttack != null)
        {
            if (!m_PreviousAttack.getAttacking()) //Check if the character is attacking
            {
                if (m_PreviousAttack.getGraceTimer() <= 0.0f) //Check if the grace timer is over
                {
					m_Inputs = "";
					m_LastInput = "";
                   // m_CurrentAttack = null; //If so, the combo gets reset
					if(!m_FinishedCombo)
					{
						setCurrentAttack();
					}
                }


                else
                {
					if(!m_FinishedCombo)
					{
						setCurrentAttack();
					}
                    // PlaySoundAndAnim();
                }
            }
			if(m_CurrentAttack != null)
			{
          		m_CurrentAttack.Update();
			}
			if(m_PreviousAttack != null)
			{
           	 	m_PreviousAttack.Update();
			}
        }

        else
        {
			if(!m_FinishedCombo)
			{
           		 setCurrentAttack();
			}
        }
    }
}