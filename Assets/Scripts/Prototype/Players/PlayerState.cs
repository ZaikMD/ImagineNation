using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class PlayerState : MonoBehaviour, Observer
{

	protected enum PlayerStates 
	{
		Default = 0,
		Idle,
		Moving,
		Interacting, 
		TakingDamage, 
		Dead
	}

	//player movement causes error as class is not created yet.
	//PlayerMovement m_PlayerMovement;

	protected PlayerStates m_PlayerState;

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

	protected List<InteractableBaseClass> m_InteractionsInRange = new List<InteractableBaseClass>();
	InteractableBaseClass m_CurrentInteraction;
	  
	short m_Health = 100;

	bool m_IsAiming = false;

	bool m_IsPaused = false;


	float timer = 0.0f;
	float delay = 0.5f;


	public CameraController m_CameraController;

    void Awake()
    {
	
    // Player Movement does not exsisit yet;
    //Make the m_PlayerMovement = to this objects playermovement script
    // m_PlayerMovement = this.gameObject.GetComponent<PlayerMovement>(); 
                
        m_PlayerState = PlayerStates.Default; 

		GameManager.Instance.addObserver (this);
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
		/*
		if(timer<delay)
		{
			timer += Time.deltaTime;
			return;
		}
		timer = 0.0f;
*/
		if(!m_IsPaused)
		{
	       //Debug.Log("update");
	       // m_HaveSecondItem = true;

	    	if(m_PlayerState == PlayerStates.Default)
	    	{
				Default();
	    	}

	    	switch(m_PlayerState)
	        {

	        case PlayerStates.Interacting:
				Debug.Log("Interacting with : " + m_CurrentInteraction.getType());
		        Interaction();	
	        	break;

	        case PlayerStates.Idle:	
		        IdleFunction();
		        break;

		    case PlayerStates.Moving:
			    MovingFunction();
			    break;
		
	        case PlayerStates.TakingDamage:
		        TakeDamage();
		        break;

		    case PlayerStates.Dead:
			    Dead();
			    break;

	            
	        }
		}
	}                                                                                                                                     

	protected void Dead()
	{
	   //     disable player.
	   //     decrement m_DeadTimer;  
	   //    if m_DeadTimer is less then 0, call reset player function.
	}


	public void exitInteracting()
	{
		m_CurrentInteraction = null;

		m_Interacting = false;
		PlayerMovement movement = gameObject.GetComponent<PlayerMovement>();
		
		if(movement != null)
		{
			movement.setCanMove ( true );
		}
		
		m_PlayerState = PlayerStates.Default;
	}

	protected void Interaction()
	{
        //add any others that are exitble

		//TODO: change to work off get is exitable
        if(m_CurrentInteraction.getIsExitable())
	    {
	    	if(PlayerInput.Instance.getEnviromentInteraction())
		    {
				exitInteracting();
            	return;
            }
		}

		//TODO: change to getType();
        switch(m_CurrentInteraction.getType())
        {
			case InteractableType.SeeSaw:
			{
				break;
			}

			case InteractableType.DivingBoard:
			{
				break;
			}
		}
		m_PlayerState = PlayerStates.Default;
	}                                                                                                                                     

	void initialInteraction()
	{
		switch (m_CurrentInteraction.getType ()) {
			case InteractableType.SeeSaw:
			{				
				SeeSaw seesaw = (SeeSaw)m_CurrentInteraction;
				seesaw.makeChild (this.gameObject);				

				PlayerMovement movement = gameObject.GetComponent<PlayerMovement> ();

				if (movement != null) 
				{
					movement.setCanMove (false);
				}
				break;
			}

			case InteractableType.DivingBoard:
			{
				DivingBoard divingBoard = (DivingBoard)m_CurrentInteraction;
				divingBoard.notifySeeSaw (this.gameObject);

				PlayerMovement movement = gameObject.GetComponent<PlayerMovement> ();

				if (movement != null) 
				{
					movement.setCanMove (false);
				}
				break;
			}

			case InteractableType.Lever:
			{
				Lever lever = (Lever)m_CurrentInteraction;
				lever.toggleIsOn();
				exitInteracting();
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
  		if(m_PlayerState == PlayerStates.Interacting)
     	{
       		return true;
     	}
     	else
     	{
       		return false;
     	}
	}                                                                                                                                          

	public InteractableType interactionType ()
	{
 		return m_CurrentInteraction.getType();
	}
                                                                                                                                
	protected void MovingFunction()
	{
      //  Debug.Log("State is now Moving");
		m_HaveSecondItem = true;
		if(m_HaveSecondItem)
		{
			if(m_UsingSecondItem)
			{
		   	 //	exit second item;
                if (m_ExitingSecondItem)
                {
					//player state machine shit
                    m_UsingSecondItem = false;
					m_ExitingSecondItem = false;
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
				if(getUseSecondItemInput())
				{
					//player movement shit

					PlayerMovement movement = gameObject.GetComponent<PlayerMovement>();

					if(movement != null)
					{
						movement.setCanMove ( false );
					}
					//----------------------------------------------------

					//player state machine shit
					m_UsingSecondItem = true; 
					m_PlayerState = PlayerStates.Default;
					return;
				}
			} 		
	    }
	    
		if(PlayerInput.Instance.getUseItem())
	    {
		   // does not have code yet
	        attack();
	    }
	 
		m_PlayerState = PlayerStates.Default;
	}

                                                                                                                                        


	protected void IdleFunction()
	{

		if(PlayerInput.Instance.getEnviromentInteraction())
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
				m_PlayerState = PlayerStates.Default;
				initialInteraction();
				return;
			}
		} 

      //  Debug.Log("State is now idle");
       //m_HaveSecondItem = true;
		if(m_HaveSecondItem)
		{
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
				if(getUseSecondItemInput())
                {
					Debug.Log("Use Second Item");
					//Movement shit
					PlayerMovement movement = gameObject.GetComponent<PlayerMovement>();
					
					if(movement != null)
					{
						movement.setCanMove ( false );
					}
					//----------------------------------------------------
		        	m_UsingSecondItem = true;
		        	m_PlayerState = PlayerStates.Default;
			        return;
	            }
	        }
	    }

		if(PlayerInput.Instance.getIsAiming())
		{
			m_IsAiming = !m_IsAiming;

			if (PlayerInput.Instance.getIsAiming())
			{
				m_CameraController.toggleAiming ( );
			}
		}


	    if(m_IsAiming)
	    {
	    	if(PlayerInput.Instance.getUseItem())
	    	{
				aimAttack();
			}
	    
	         m_PlayerState = PlayerStates.Default;
	         return;
	    }

	    if(PlayerInput.Instance.getUseItem())
	    {
	    	attack();
	        m_PlayerState = PlayerStates.Default;
	        return;
	    }        
	  
		m_PlayerState = PlayerStates.Default;
	}
                                                                                                                                         

	/// <summary>
	/// this is a function to see what state we are in.
	/// </summary>
	protected void Default()
	{
		if(m_Health <= 0)
		{
			m_PlayerState = PlayerStates.Dead;
			return;
		}

	    if(m_TakeDamage)	
		{
			m_PlayerState = PlayerStates.TakingDamage;
			return;
		}

		if(m_Interacting)	
		{
			m_PlayerState =  PlayerStates.Interacting;
			return;
		}
		
        Vector2 currentInput = PlayerInput.Instance.getMovementInput();
		if(Mathf.Abs(currentInput.x) > 0.0f || Mathf.Abs(currentInput.y) > 0.0f || !gameObject.GetComponent<PlayerMovement>().IsGrounded())
        {
            m_PlayerState = PlayerStates.Moving;
        }
        else
        {
            m_PlayerState = PlayerStates.Idle;
        }
	}

	public void interactionInRange(InteractableBaseClass Interaction)
	{
		m_InteractionsInRange.Add(Interaction);
	}


	public void interactionOutOfRange(InteractableBaseClass interaction)
	{
	    m_InteractionsInRange.Remove(interaction);
	}

	public void setExitingSecond (bool isExiting)
	{
	    m_ExitingSecondItem = isExiting;
	}

	protected abstract bool getUseSecondItemInput();

	//checks to see if the player is able to use second item.
	protected abstract bool ableToEnterSecondItem();

	protected abstract void useSecondItem();

	protected abstract void attack();

	protected abstract void aimAttack();  // <- derek cannot implement, does not have a aim attack.

	public void recieveEvent(Subject sender, ObeserverEvents recievedEvent)
	{
		if(recievedEvent == ObeserverEvents.PauseGame || recievedEvent == ObeserverEvents.StartGame)
		{
			m_IsPaused = !m_IsPaused;
		}
	}
}