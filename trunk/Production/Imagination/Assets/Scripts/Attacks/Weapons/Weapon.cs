using UnityEngine;
using System.Collections;
/// <summary>
/// Combat item.
/// Created by Zach Dubuc
/// 
/// Handles the combos for players, and calls the attack function from the attacks
/// 
/// ChangeLog:
/// 1/12/14: Fully commented - Zach Dubuc
/// 
/// </summary>
public class Weapon : MonoBehaviour
{
    SFXManager m_SFX;
    AnimationState m_AnimState;

    BaseAttack m_CurrentAttack;//The current Attack

    public GameObject m_LightProjectilePrefab; //Prefabs for the projectiles
    public GameObject m_HeavyProjectilePrefab;

	float m_DownTime = 0.5f; //Downtime between the end and start of new combos
	float DOWN_TIME;

	float m_DoubleHitTimer = 0.2f; //The timer for the double hit finisher
	float DOUBLE_HIT_TIMER;
	bool m_DoubleHitActivated = false; //Bool for if it is activated 
	bool m_FinishedCombo = false; //Bool for when a combo has been pulled off

    string m_Inputs; //String that holds all the inputs
	string m_LastInput; //String that holds the last input the player used

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
	const string STRING_RESET = "";

	//Attack movement speed
	protected float m_AttackMoveSpeed = 0.2f;


    
	
	BaseMovementAbility m_BaseMovementAbility; //For access to the players BaseMovementAbility so they can stop moving 
											   //When they are attacking
    AcceptInputFrom m_ReadInput;  //To get the input
	PlayerInfo playerinfo; //Info for the players

	void Start () 
    {
        m_ReadInput = gameObject.GetComponent<AcceptInputFrom>(); //Get the input

        m_SFX = GameObject.FindGameObjectWithTag(Constants.SOUND_MANAGER).GetComponent<SFXManager>(); //Sound stuff
        m_AnimState = GetComponent<AnimationState>();
		DOWN_TIME = m_DownTime; //Set reference for the down time timer
		DOUBLE_HIT_TIMER = m_DoubleHitTimer; //Set the reference for the double hit timer
		m_BaseMovementAbility = gameObject.GetComponent (typeof(BaseMovementAbility)) as BaseMovementAbility; //Get the movement
		playerinfo = gameObject.GetComponent (typeof(PlayerInfo)) as PlayerInfo; //Get player infor

	}
	
	// Update is called once per frame
	void Update () 
    {
		if(m_FinishedCombo) //If the combo is finished
		{
			if(m_DownTime > 0.0f) //If there is still down time
			{
				m_DownTime -= Time.deltaTime; //Decrement
			}

			else
			{ //Otherwise reset DownTime and m_FinishedCombo
				m_DownTime = DOWN_TIME;
				m_FinishedCombo = false;
			}
		}
        updateAttacks(); //Call update for attacks

		if(m_DoubleHitActivated) //If the player did a double hit combo
		{
			m_DoubleHitTimer -= Time.deltaTime; //Decrement the timer
		}

		if(m_DoubleHitTimer < 0.0f) //If the timer is done, create the second projectile
		{
			LightAttack attack = new LightAttack (); //Make the attack
			attack.loadPrefab (m_LightProjectilePrefab);//Load the prefab and call startAttack.
			attack.startAttack (transform.position, transform.eulerAngles, playerinfo.i_Character);
			m_CurrentAttack = attack;//Set the current attack
			m_LastInput = ""; //Reset last input
			m_DoubleHitTimer = DOUBLE_HIT_TIMER; //Set the double hit timer
			m_DoubleHitActivated = false; //Double hit is now false
		}

		if(m_CurrentAttack != null) //Null check for CurrentAttack
		{
			if(m_CurrentAttack.getAttacking()) //If the current attack is attacking
			{
				if(m_BaseMovementAbility.GetIsGrounded()) //If the player is grounded
				{
					//Calculate forced input for the step of the attack
					Vector3 forcedInput = m_BaseMovementAbility.GetProjection();

					//If their is no input, we instead choose the forward direction of the player
					if (forcedInput == Vector3.zero)
					{
						forcedInput = transform.forward;
						forcedInput.y = 0.0f;
					}

					//Set the direction to move while attacking
					if (m_CurrentAttack.getForceInput())
					{
						m_BaseMovementAbility.SetForcedInput (forcedInput);
					}

					//Set the player to move more slowly
					m_BaseMovementAbility.SetSpeedMultiplier(m_CurrentAttack.getAttackMoveSpeed());
				}
			}

			else //Otherwise the player can move
			{
				m_BaseMovementAbility.SetForcedInput (Vector3.zero);
				m_BaseMovementAbility.SetSpeedMultiplier(BaseMovementAbility.DEFAULT_SPEED_MULTIPLIER);
			}
		}
	}

	/// <summary>
	/// Sets the current attack.
	/// Checks to see what input the player used, then adds it to the input string. Then goes through
	/// a switch statement to determine what attack to use. If no combo is found that matches the string
	/// the input string is reset and last input is added to it. The switch statement goes through again.
	/// </summary>
    void setCurrentAttack()
    {
		if(m_BaseMovementAbility.GetIsGrounded()) //If the player is grounded
		{
			//If one of the attack buttons was pressed
			if (InputManager.getAttackDown (m_ReadInput.ReadInputFrom) || InputManager.getHeavyAttackDown(m_ReadInput.ReadInputFrom))
			{
				if(InputManager.getAttackDown(m_ReadInput.ReadInputFrom))
				{
					//If X or left click was pressed, then it's a light attack.
					//Add the L string to the input string
					m_Inputs += L;
					m_LastInput = L; //Set last input
				}

				if(InputManager.getHeavyAttackDown(m_ReadInput.ReadInputFrom))
				{
					//If Y or Right click was pressed, then it's a heavy attack.
					//Add the H string to the input string.
					m_Inputs += H;
					m_LastInput = H; //Set last input
				}

				//Switch statement for the string. It will go through and see if the input string matches
				//Any of the cases. IF it does, then it will start the corresponding attack. If not, it
				//resets the input string and goes through again using the last input the player did.
				switch (m_Inputs) 
				{
					case L:  //A Light attack
					{
						LightAttack attack = new LightAttack ();
						attack.loadPrefab (m_LightProjectilePrefab);
						attack.startAttack (transform.position, transform.eulerAngles, playerinfo.i_Character);
						m_CurrentAttack = attack;
						m_LastInput = STRING_RESET;
						}

						break;

					case LL: //A Light attack
					{
						LightAttack attack = new LightAttack ();
						attack.loadPrefab (m_LightProjectilePrefab);
						attack.startAttack (transform.position, transform.eulerAngles, playerinfo.i_Character);
						m_CurrentAttack = attack;
						m_LastInput = STRING_RESET;
					}
						break;

					case DOUBLEHIT: //Two light attacks in fast concession
					{
						LightAttack attack = new LightAttack ();
						attack.loadPrefab (m_LightProjectilePrefab);
						attack.startAttack (transform.position, transform.eulerAngles, playerinfo.i_Character);
						m_CurrentAttack = attack;
						m_LastInput = STRING_RESET;
						m_DoubleHitActivated = true;
					}
						break;

					case LH: //A heavy attack
					{
						HeavyAttack attack = new HeavyAttack ();
						attack.loadPrefab (m_HeavyProjectilePrefab);
						attack.startAttack (transform.position, transform.eulerAngles, playerinfo.i_Character);
						m_CurrentAttack = attack;
						m_LastInput = STRING_RESET;

					}
						break;

					case AOE: //Light attacks in a circle around the player
					{					
						SpecialAttack attack = new SpecialAttack();
						attack.loadPrefab (m_LightProjectilePrefab);
						attack.startAttack(transform.position, transform.eulerAngles, playerinfo.i_Character);
						m_CurrentAttack = attack;
						m_LastInput = STRING_RESET;
						m_Inputs = STRING_RESET;
						m_FinishedCombo = true;
					}
						break;

					case FRONTLINE: //Light attack that goes for a ways infront of the player
				    {
						FrontalLine attack = new FrontalLine ();
						attack.loadPrefab (m_LightProjectilePrefab);
						attack.startAttack (transform.position, transform.eulerAngles, playerinfo.i_Character);
						m_CurrentAttack = attack;
						m_LastInput = STRING_RESET;
						m_FinishedCombo = true;
					}
						break;

					case H: //A Heavy attack
					{
						HeavyAttack attack = new HeavyAttack ();
						attack.loadPrefab (m_HeavyProjectilePrefab);
						attack.startAttack (transform.position, transform.eulerAngles, playerinfo.i_Character);
						m_CurrentAttack = attack;
						m_LastInput = STRING_RESET;
					}
						break;

					case HH: //A heavy attack
					{
						HeavyAttack attack = new HeavyAttack ();
						attack.loadPrefab (m_HeavyProjectilePrefab);
						attack.startAttack (transform.position, transform.eulerAngles, playerinfo.i_Character);
						m_CurrentAttack = attack;
						m_LastInput = STRING_RESET;	
					}
						break;

					case FRONTCONE: //A cone of heavy attacks in front of the player
					{
						FrontalConeAttack attack = new FrontalConeAttack ();
						attack.loadPrefab (m_HeavyProjectilePrefab);
						attack.startAttack (transform.position, transform.eulerAngles, playerinfo.i_Character);
						m_CurrentAttack = attack;
						m_LastInput = STRING_RESET;
						m_FinishedCombo = true;
					}
						break;

					default:
					{
						m_Inputs = m_LastInput; //Default
					}
						break;
				}
			}
		}
    }

    void updateAttacks()
    {

		if (m_CurrentAttack != null) //Null check for the current attack
        {
            if (!m_CurrentAttack.getAttacking()) //Check if the character is attacking
            {

				if (m_CurrentAttack.getGraceTimer() <= 0.0f) //Check if the grace timer is over
                {
					m_Inputs = STRING_RESET; //Reset the strings
					m_LastInput = STRING_RESET;
					if(!m_FinishedCombo)
					{
						setCurrentAttack(); //If the player didn't finish a combo, then call setCurrentAttack
					}
                }


                else //Otherwise
                {
					if(!m_FinishedCombo) //If a combo wasn't finished
					{
						setCurrentAttack(); //Call setCurrentAttack
					}
                }
            }
			if(m_CurrentAttack != null) //Null check
			{
          		m_CurrentAttack.Update(); //Update the current attack
			}
        }

        else //Otherwsie
        {
			if(!m_FinishedCombo)
			{
           		 setCurrentAttack(); //Call setCurrentAttack
			}
        }
    }
}
