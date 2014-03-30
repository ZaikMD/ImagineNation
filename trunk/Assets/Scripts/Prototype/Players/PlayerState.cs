using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class PlayerState : MonoBehaviour 
{

	protected enum PlayerStates 
	{
	Default = 0,
	Idle,
	Moving,
	interacting, 
	takingDamage, 
	Dead,
	}

	public enum InteractionTypes
	{
	    PickUp = 0,
	    SeesawBottom,
	    SeesawJump,
	    NPC,
	    CrawlSpace,
	    unKnown
	}

	//player movement causes error as class is not created yet.
	//PlayerMovement m_PlayerMovement;

	protected PlayerStates m_PlayerState;
	InteractionTypes m_InteractionType;


	GameObject m_AimReticle;
	GameObject m_CurrentPartner; // likely set by start menu.
	const float KNOCKBACK_TIME = 0.5f;
	float m_KnockBackTimer = KNOCKBACK_TIME; 
	bool m_UsingSecondItem;
	bool m_HaveSecondItem;
	bool m_Interacting;
	bool m_TakeDamage;
	bool m_ExitingSecondItem;
	bool m_DamagedBy;
	protected List<GameObject> m_InteractionsInRange = new List<GameObject>();
	GameObject m_CurrentInteraction;
	  
	short m_Health = 100;

    
    void Awake()
    {
	
    // Player Movement does not exsisit yet;
    //Make the m_PlayerMovement = to this objects playermovement script
    // m_PlayerMovement = this.gameObject.GetComponent<PlayerMovement>(); 
                
        m_PlayerState = PlayerStates.Default; 
    }



	// Use this for initialization
	void Start ()
    {
        m_HaveSecondItem = true;
        m_UsingSecondItem = false;

	}
	
	// Update is called once per frame
	protected void checkStates()
    {
       // Debug.Log("anything");
       // m_HaveSecondItem = true;

    	if(m_PlayerState == PlayerStates.Default)
    	{
			Default();
    	}

    	switch(m_PlayerState)
        {

        case PlayerStates.interacting:
	        Interaction();	
        	break;

        case PlayerStates.Idle:	
	        IdleFunction();
	        break;

	    case PlayerStates.Moving:
		    MovingFunction();
		    break;
	
        case PlayerStates.takingDamage:
	        TakeDamage();
	        break;

	    case PlayerStates.Dead:
		    Dead();
		    break;

            
        }
	}                                                                                                                                     

	protected void Dead()
	{
	   //     disable player.
	   //     decrement m_DeadTimer;  
	   //    if m_DeadTimer is less then 0, call reset player function.
	}


	protected void Interaction()
	{
        Debug.Log("state is now interacting");
        //add any others that are exitble

		//TODO: change to work off get is exitable
        if(m_InteractionType == InteractionTypes.PickUp || m_InteractionType == InteractionTypes.SeesawBottom)
	    {
	    	if(PlayerInput.Instance.getEnviromentInteraction())
		    {
				m_Interacting = false;
            	return;
            }
		}

		//TODO: change to getType();
        switch(m_InteractionType)
        {	
    		case InteractionTypes.PickUp:
			{	
		   /*    float difference = Vector3.Distance(this.transform.position, pick up destination); //Not made yet.
		        if(difference < 5)
		        {
		    	    // call the use function of the object.
		        }
		        else
		        {
		            // m_PlayerMovement.Move(); // PlayerMovement Not Made yet;		
		        }
		    */
		        m_PlayerState = PlayerStates.Default;
		
		    	break;
			}
		    case InteractionTypes.SeesawBottom:
			{
			    m_PlayerState = PlayerStates.Default;
		    	break;
			}
		    case InteractionTypes.NPC:
			{
			 /*   if( !TalkingDone)
			    {
				    Update talk timer;
			    }
		    else
		    {
			    stop interacting
			    reset timer
		    }
		     */ 
		   		m_PlayerState =  PlayerStates.Default;
		    	break;
			}

			case InteractionTypes.CrawlSpace:
			{
				/*	if(TeleportDone)
					{
					reset stuff
	    			}

				*/
	   			m_PlayerState = PlayerStates.Default;
	    		break;
			}
			case InteractionTypes.SeesawJump:
			{
					/* if(atBottom)
						{
						reset
    					} 
					*/

   				m_PlayerState = PlayerStates.Default;
    			break;
			}
		}

	}                                                                                                                                     


	public void FlagDamage(short amount, GameObject damagedBy)
	{
   		 if (!m_TakeDamage && m_PlayerState != PlayerStates.Dead)
   		 {
		   m_DamagedBy = damagedBy;
     	   applyDamage(amount);
			m_TakeDamage = true;
  		 }
	}

                                                                                                                                                       


	protected void TakeDamage()
	{
		if(m_KnockBackTimer >= 0)
		{
			m_KnockBackTimer -= Time.deltaTime;
			// update player position based off what hurt them (move them away)
    	}
    	else
    	{
      	 // reset m_KnockBackTimer;
			m_KnockBackTimer = KNOCKBACK_TIME;
       		m_TakeDamage = false;	
    	}
	}

	protected void applyDamage(short amount)
	{
    	m_Health -= amount;
    	//update the hud to represent current health status.
	}

	protected void ResetPlayer()
	{
    	if(m_CurrentPartner.GetComponent<PlayerState>().m_PlayerState != PlayerStates.Dead)
    	{
      	   //  if so enable our player and set his , transform. position to equal near our other character
    	}
		else 
    	{
       	  // Call checkpoint reset function 
    	}
	
	}

	protected void KnockBack()
	{
   		//   get gameObject that hit us being stored in member variable
   		//	get the direction of the attacker by using his position and ours to get a normalized vector.
   		//   times the new vector3 by -1 so we are now moving away 
   		//   multiply the normalized vector3 by knockback speed and delta time
   		//   Call our playerMovement.SetKnockBack(pass in the vector3);  
	}
	                                                                                                                                    

	public bool getInteracting()
	{
  		if(m_PlayerState == PlayerStates.interacting)
     	{
       		return true;
     	}
     	else
     	{
       		return false;
     	}
	}                                                                                                                                          

	public InteractionTypes interactionType ()
	{
 		return m_InteractionType;
	}
                                                                                                                                
	protected void MovingFunction()
	{
      //  Debug.Log("State is now Moving");
		if(m_HaveSecondItem)
		{
			if(m_UsingSecondItem)
			{
		   	 //	exit second item;
				//TODO: check if supposed to exit if not call the second item use function
				if(PlayerInput.Instance.getInteractWithEnviromentHeld())
				{
					//call items exit function.
				}
				m_PlayerState = PlayerStates.Default;
				return;
        	}
			else if (ableToEnterSecondItem())
			{
				//TODO: check if entering second item. if so set bool and return and set state to default
				if(PlayerInput.Instance.getEnviromentInteraction())
				{
					m_UsingSecondItem = true; 
					m_Interacting = true;
					m_PlayerState = PlayerStates.Default;

				}
			} 		
	    }
	    
		if(PlayerInput.Instance.getUseItem())
	    {
		   // does not have code yet
	        attack();
		    m_PlayerState = PlayerStates.Default;
	    }
	 
		m_PlayerState = PlayerStates.Default;
	}

                                                                                                                                        


	protected void IdleFunction()
	{
      //  Debug.Log("State is now idle");
       // m_HaveSecondItem = true;
		if(m_HaveSecondItem)
		{
            Debug.Log("have second weapon");
			if(m_UsingSecondItem)
			{
				if(m_ExitingSecondItem)
				{
					// exit second item.

					m_ExitingSecondItem = false;

					m_UsingSecondItem = false;
	            }
	            else
	            {
		           useSecondItem();
	            }

	            m_PlayerState = PlayerStates.Default;
	            return;
	        }
	        else if (ableToEnterSecondItem())
	        {
	            if(PlayerInput.Instance.getEnviromentInteraction())
	            {
		        	m_UsingSecondItem = true;
		        	m_PlayerState = PlayerStates.Default;
			        return;
	            }
	        }
	    }

	    if(PlayerInput.Instance.getIsAiming())
	    {
	    	if(PlayerInput.Instance.getUseItem())
	    	{
				aimAttack();
			}
	    
	         m_PlayerState = PlayerStates.Default;
	         return;
	    }
	    else if(PlayerInput.Instance.getUseItem())
	    {
	    	attack();
	        m_PlayerState = PlayerStates.Default;
	        return;
	    }
	    else if(PlayerInput.Instance.getEnviromentInteraction())
	    {
            Debug.Log("check for interactions");
	    	if(m_InteractionsInRange.Count!= 0)
	        {
		    	if (m_InteractionsInRange.Count > 1)
		        {
			    	float best = Vector3.Distance(this.gameObject.transform.position, m_InteractionsInRange[0].gameObject.transform.position);
			        m_CurrentInteraction = m_InteractionsInRange[0];
			        for( int i = 1; i <  m_InteractionsInRange.Count; i++)
	                {
	 					float Next = Vector3.Distance(this.gameObject.transform.position, m_InteractionsInRange[i].gameObject.transform.position);
						if(Next < best)
						{
	                    	best = Next;		
	                        m_CurrentInteraction = m_InteractionsInRange[i];	
	                    }
	                 }
				}
	            else
	            {   
		        	m_CurrentInteraction = m_InteractionsInRange[0];
	            }

				m_Interacting = true;
	         }
	    }   
		m_PlayerState = PlayerStates.Default;
	}
                                                                                                                                         

	/// <summary>
	/// this is a function to see what state we are in.
	/// </summary>
	protected void Default()
	{
       // Debug.Log("state is now Default");

		if(m_Health <= 0)
		{
			m_PlayerState = PlayerStates.Dead;
			return;
		}

	    if(m_TakeDamage)	
		{
			m_PlayerState = PlayerStates.takingDamage;
			return;
		}
		if(m_Interacting)	
		{
			m_PlayerState =  PlayerStates.interacting;
			return;
		}
        float butterZone = 0.1f;
        Vector2 currentInput = new Vector2(PlayerInput.Instance.getMovementInput().x, PlayerInput.Instance.getMovementInput().y);
        if(currentInput.x > butterZone || currentInput.x < -butterZone || currentInput.y > butterZone || currentInput.y < -butterZone)
        {
            m_PlayerState = PlayerStates.Moving;
        }
        else
        {
            m_PlayerState = PlayerStates.Idle;
        }


	}




	public void interactionInRange(GameObject Interaction)
	{
		m_InteractionsInRange.Add(Interaction);
	}


	public void interactionOutOfRange(GameObject interaction)
	{
	    m_InteractionsInRange.Remove(interaction);
	}

	void setExitingSecond (bool isExiting)
	{
	    m_ExitingSecondItem = isExiting;
	}

	//checks to see if the player is able to use second item.
	public abstract bool ableToEnterSecondItem();

	public abstract void useSecondItem();

	public abstract void attack();

	public abstract void aimAttack();  // <- derek cannot implement, does not have a aim attack.


}