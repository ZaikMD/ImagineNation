using UnityEngine;
using System.Collections;
/// <summary>
/// Combat item.
/// Created by Zach Dubuc
/// 
/// Handles the combos for players, and calls the attack function from the attacks
/// 
/// </summary>

#region ChangeLog
/*
* 8/10/2014 Edit: Fully Commented- Zach Dubuc
*
* Added in a  for loop to set the attack timer on any special attacks
*/
#endregion
public class CombatItem : MonoBehaviour 
{
	SFXManager m_SFX;
	AnimationState m_AnimState;
	
	BaseAttack[] m_BaseAttacks = new BaseAttack[3]; //Array of the attacks. Combo's are 3 attacks.

	int m_CurrentAttack = 0; //The current Attack
	int m_PreviousAttack = 0;

	string m_Inputs;

    AcceptInputFrom m_ReadInput;

    const ScriptPauseLevel PAUSE_LEVEL = ScriptPauseLevel.Cutscene;

	// Use this for initialization
	void Start () 
	{
		m_BaseAttacks [0] = new LightAttack (); //Two base attacks, and one special which is an AOE around the character
		m_BaseAttacks [1] = new LightAttack ();
		m_BaseAttacks [2] = new HeavyAttack ();

		for(int i = 0; i < m_BaseAttacks.Length; i++)
		{
			//m_BaseAttacks[i].loadPrefab(m_ProjectilePrefab); //Loads the prefab for the projectiles
			m_BaseAttacks.Initialize();
		}
        m_ReadInput = gameObject.GetComponent<AcceptInputFrom> ();
        
		m_SFX = GameObject.FindGameObjectWithTag(Constants.SOUND_MANAGER).GetComponent<SFXManager> ();
		m_AnimState = GetComponent<AnimationState> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
        if (PauseScreen.shouldPause(PAUSE_LEVEL)) { return; }

        if (InputManager.getAttackDown(m_ReadInput.ReadInputFrom))
        {
            if (!m_BaseAttacks[m_PreviousAttack].getAttacking()) //Check if the character is attacking
            {
                if (m_BaseAttacks[m_PreviousAttack].getGraceTimer() <= 0.0f) //Check if the grace timer is over
                {
                    m_CurrentAttack = 0; //If so, the combo gets reset
                    m_BaseAttacks[m_PreviousAttack].resetGraceTimer();
                }

                else
                {
                   // m_BaseAttacks[m_CurrentAttack].startAttack(transform.position, transform.rotation); //Call attack function
					PlaySoundAndAnim();
                    m_PreviousAttack = m_CurrentAttack;//Set the previous attack to the current attack, then increment the current attack
                    m_CurrentAttack++;
                   


                    if (m_CurrentAttack >= m_BaseAttacks.Length) //If the currentAttack is the last one in the array, reset it
                    {
                        m_CurrentAttack = 0;
                    }
                }
            }
        }

        else
        {
            if (!m_BaseAttacks[m_PreviousAttack].getAttacking()) //Check to see if the player is still attacking
            {
                if (!m_BaseAttacks[m_PreviousAttack].getGraceCountdown()) //Check if the grace timer is over
                {
                    m_CurrentAttack = 0; //If so, the combo gets reset
                    m_BaseAttacks[m_PreviousAttack].resetGraceTimer();
                }
            }
        }
		for(int i = 0; i < m_BaseAttacks.Length; i++)
		{
			m_BaseAttacks[i].Update();
		}

	}
#region Sound
	void PlaySoundAndAnim()
	{
		switch(this.gameObject.name)
		{
			case Constants.ALEX_WITH_MOVEMENT_STRING:
				switch(m_CurrentAttack)
				{
					//First attack, play slash one
					case 0:
					m_AnimState.AddAnimRequest(AnimationStates.OverHeadSlash);
					m_SFX.playSound(this.gameObject, Sounds.AlexHitOne);		
					break;
				
					case 1:
					m_AnimState.AddAnimRequest(AnimationStates.OverHeadSlash);
					m_SFX.playSound(this.gameObject, Sounds.AlexHitTwo);
					break;
				
					case 2:
					m_AnimState.AddAnimRequest(AnimationStates.DoubleSlash);
					m_SFX.playSound(this.gameObject, Sounds.AlexHitThree);
					break;
				}
			break;

			case Constants.DEREK_WITH_MOVEMENT_STRING:
				switch(m_CurrentAttack)
				{
					//First attack, play punch for derek
					case 0:
					m_AnimState.AddAnimRequest(AnimationStates.Punch);
					m_SFX.playSound(this.gameObject, Sounds.DerekHitOne);		
					break;
				
					case 1:
					m_AnimState.AddAnimRequest(AnimationStates.Punch);
					m_SFX.playSound(this.gameObject, Sounds.DerekHitTwo);
					break;
				
					case 2:
					m_AnimState.AddAnimRequest(AnimationStates.DoubleSlash);
					m_SFX.playSound(this.gameObject, Sounds.DerekHitThree);
					break;
				}
			break;

			case Constants.ZOE_WITH_MOVEMENT_STRING:
				switch(m_CurrentAttack)
				{
					//First attack, play slash one
					case 0:
					m_AnimState.AddAnimRequest(AnimationStates.OverHeadSlash);
					m_SFX.playSound(this.gameObject, Sounds.ZoeyHitOne);		
					break;
				
					case 1:
					m_AnimState.AddAnimRequest(AnimationStates.OverHeadSlash);
					m_SFX.playSound(this.gameObject, Sounds.ZoeyHitTwo);
					break;
				
					case 2:
					m_AnimState.AddAnimRequest(AnimationStates.DoubleSlash);
					m_SFX.playSound(this.gameObject, Sounds.ZoeyHitThree);
					break;
				}
			break;
		}
	}
#endregion
}
