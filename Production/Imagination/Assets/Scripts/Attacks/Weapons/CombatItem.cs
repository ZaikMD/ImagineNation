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
* 
*/
#endregion
public class CombatItem : MonoBehaviour 
{

	public GameObject m_ProjectilePrefab;
	BaseAttack[] m_BaseAttacks = new BaseAttack[3]; //Array of the attacks. Combo's are 3 attacks.

	int m_CurrentAttack = 0; //The current Attack
	int m_PreviousAttack = 0;

    AcceptInputFrom m_ReadInput;
	// Use this for initialization
	void Start () 
	{
		m_BaseAttacks [0] = new BaseAttack (); //Two base attacks, and one special wich is an AOE around the character
		m_BaseAttacks [1] = new BaseAttack ();
		m_BaseAttacks [2] = new SpecialAttack ();

		for(int i = 0; i < m_BaseAttacks.Length; i++)
		{
			m_BaseAttacks[i].loadPrefab(m_ProjectilePrefab); //Loads the prefab for the projectiles
			m_BaseAttacks.Initialize();
		}

        m_ReadInput = gameObject.GetComponent<AcceptInputFrom>();
        

	}
	
	// Update is called once per frame
	void Update () 
	{

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
                    m_BaseAttacks[m_CurrentAttack].startAttack(transform.position, transform.rotation); //Call attack function

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
}
