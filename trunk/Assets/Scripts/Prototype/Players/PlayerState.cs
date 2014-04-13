using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// the annoying class that controls the entire game flow
/// </summary>
public abstract class PlayerState : MonoBehaviour, Observer
{

	public enum PlayerStates 
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

	public PlayerStates m_PlayerState;

	GameObject m_AimReticle;

	const float KNOCKBACK_TIME = 1.0f;
	float m_KnockBackTimer = KNOCKBACK_TIME; 
	bool m_UsingSecondItem;
	bool m_HaveSecondItem;
	bool m_Interacting;
	bool m_TakeDamage;
	bool m_ExitingSecondItem;
	bool m_DamagedBy;

	protected List<InteractableBaseClass> m_InteractionsInRange = new List<InteractableBaseClass>();
	InteractableBaseClass m_CurrentInteraction;
	  
	Health m_Health;

	bool m_IsAiming = false;

	bool m_IsPaused = false;

	public bool m_IsActive = false;

	public CameraController m_CameraController;


    void Awake()
    {
	
    // Player Movement does not exsisit yet;
    //Make the m_PlayerMovement = to this objects playermovement script
    // m_PlayerMovement = this.gameObject.GetComponent<PlayerMovement>(); 
                
        m_PlayerState = PlayerStates.Default; 
		
		GameManager.Instance.addObserver (this);

  		CharacterSwitch.Instance.addObserver (this);

    }

	// Use this for initialization
	void Start ()
    {
		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");

		for(int i = 0; i < players.Length; i++)
		{
			Collider[] colliders1 = (Collider[]) players[i].GetComponentsInChildren(typeof(Collider));
			for(int c = 0; c < players.Length; c++)
			{
				if(i != c)
				{
					Collider[] colliders2 = (Collider[])players[c].GetComponentsInChildren(typeof(Collider));
					for(int x = 0; x < colliders1.Length; x++)
					{
						for(int y = 0; y < colliders2.Length; y++)
						{
							Physics.IgnoreCollision(colliders1[x],colliders2[y]);
						}
					}
				}
			}
		}

        m_PlayerState = PlayerStates.Default;

        m_HaveSecondItem = true;
        m_UsingSecondItem = false;

		gameObject.GetComponent<PlayerMovement>().setCanMove(m_IsActive);

		if(m_IsActive)
		{
			//Debug.Log(this.gameObject.name + " is active");
			GameObject.FindGameObjectWithTag("Camera").GetComponentInChildren<CameraController>().switchTo(this.gameObject.transform);
		}

		m_Health = gameObject.GetComponent<Health> ();
		m_Health.m_MaxHealth = 100;
		m_Health.resetHealth ();

		start ();
	}

    public void SetUp()
    {
        m_PlayerState = PlayerStates.Default;

        GameManager.Instance.addObserver(this);

        CharacterSwitch.Instance.addObserver(this);

        m_HaveSecondItem = true;
        m_UsingSecondItem = false;

        gameObject.GetComponent<PlayerMovement>().setCanMove(m_IsActive);

        if (m_IsActive)
        {
            GameObject.FindGameObjectWithTag("Camera").GetComponentInChildren<CameraController>().switchTo(this.gameObject.transform);
        }

        start();
    }


	protected abstract void start ();

	// Update is called once per frame
	protected void checkStates()
    {
		//only updates if the script is active and unpaused
		if(m_IsActive)
		{
			if(!m_IsPaused)
			{
		       // m_HaveSecondItem = true;

		    	if(m_PlayerState == PlayerStates.Default)
		    	{
				//	Debug.Log ("Default");
					Default();
		    	}

		    	switch(m_PlayerState)
		        {

		        case PlayerStates.Interacting:
				//	Debug.Log ("Interacting");
			        Interaction();	
		        	break;

		        case PlayerStates.Idle:	
				//	Debug.Log ("Idle");
			        IdleFunction();
			        break;

			    case PlayerStates.Moving:
				//	Debug.Log ("Moving");
				    MovingFunction();
				    break;
			
		        case PlayerStates.TakingDamage:
				//	Debug.Log ("TakingDamage");
			        TakeDamage();
			        break;

			    case PlayerStates.Dead:
				//	Debug.Log ("Dead");
				    Dead();
				    break;
		            
		        }
			}
		}
	}

	//not implemented yet since no enemies
	protected void Dead()
	{
		//RespawnManager.Instance.playerDied (this.gameObject);
	   //     disable player.
	   //     decrement m_DeadTimer;  
	   //    if m_DeadTimer is less then 0, call reset player function.
	}

	//used to call any functions that need calling when stoping the interaction with an object
	void stopInteractiong ()
	{
		switch (m_CurrentInteraction.getType ()) 
		{
			case InteractableType.SeeSaw:
			{			
				SeeSaw seeSaw = (SeeSaw)m_CurrentInteraction;
				seeSaw.makeChild(null);
				break;
			}
				
			case InteractableType.MovingBlock:
			{
				MoveableBlock moveableBlock = (MoveableBlock)m_CurrentInteraction;
				moveableBlock.onExit();
				break;
			}
				
			case InteractableType.PickUp:
			{
				PickUp pickUp = (PickUp)m_CurrentInteraction;
				pickUp.DropItem();
				break;
			}

			default:
			{
				break;
			}
		}
	}

	//used to exit interactions
	//public so interactions the player cant exit can call it
	public void exitInteracting()
	{
		stopInteractiong ();

		m_CurrentInteraction = null;

		m_Interacting = false;
		PlayerMovement movement = gameObject.GetComponent<PlayerMovement>();
		
		if(movement != null)
		{
			if(m_IsActive)
			{
				movement.setCanMove ( true );
			}
		}
		
		m_PlayerState = PlayerStates.Default;
	}

	//called every update loop any interaction functions that need regular calling should be called here
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
			case InteractableType.MovingBlock:
			{
				MoveableBlock moveblock = (MoveableBlock)m_CurrentInteraction;
				
				gameObject.GetComponent<PlayerMovement>().BlockHeldMovement(moveblock.m_BlockSize);

				break;
			}

			default:
			{
				break;
			}
		}
		m_PlayerState = PlayerStates.Default;
	}                                                                                                                                     

	//called when initaily interactiong with something
	void initialInteraction()
	{
		switch (m_CurrentInteraction.getType ()) 
		{
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
				PlayerMovement movement = gameObject.GetComponent<PlayerMovement> ();
				
				if (movement != null) 
				{
					movement.setCanMove (false);
				}
				
				DivingBoard divingBoard = (DivingBoard)m_CurrentInteraction;
				divingBoard.notifySeeSaw (this.gameObject);

				
				break;
			}

			case InteractableType.Lever:
			{
				Lever lever = (Lever)m_CurrentInteraction;
				lever.toggleIsOn();
				exitInteracting();
				break;
			}

			case InteractableType.CrawlSpace:
			{
				CrawlSpaces crawlSpace = (CrawlSpaces)m_CurrentInteraction;
				crawlSpace.OnUse(this.gameObject);
				break;
			}

			case InteractableType.MovingBlock:
			{
				MoveableBlock moveableBlock = (MoveableBlock)m_CurrentInteraction;
				moveableBlock.onUse(this.gameObject);
				break;
			}

			case InteractableType.PickUp:
			{
				PickUp pickUp = (PickUp)m_CurrentInteraction;
				pickUp.PickUpItem(this.gameObject);
				break;
			}

			case InteractableType.NPC:
			{
				NPC npc = (NPC)m_CurrentInteraction;
				npc.setShowText(true, this.gameObject);

				PlayerMovement movement = gameObject.GetComponent<PlayerMovement> ();
				
				if (movement != null) 
				{
					movement.setCanMove (false);
				}

				break;
			}
		}
	}

	//not implemented/tested yet since no enemies
	public void FlagDamage(short amount)
	{
  		 if (!m_TakeDamage && m_PlayerState != PlayerStates.Dead)
   		 {
     	   	applyDamage(amount);
			m_TakeDamage = true;
			gameObject.GetComponent<PlayerMovement>().setCanMove(false);

			if(m_CurrentInteraction != null)
			{
				exitInteracting();
			}

			if(m_UsingSecondItem)
			{
				setExitingSecond(true);
			}
		}
	}                                                                                                                                                      

	//not implemented/tested yet since no enemies
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
			gameObject.GetComponent<PlayerMovement>().setCanMove(true);
			m_PlayerState = PlayerStates.Default;
    	}
	}

	//not implemented/tested yet since no enemies
	protected void applyDamage(int amount)
	{
    	m_Health.takeDamage(amount);
    	//update the hud to represent current health status.
	}

	public int getHealth()
	{
		return m_Health.getHealth ();
	}

	//not implemented/tested yet since no enemies
	protected void ResetPlayer()
	{
		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");

		PlayerState partner;
		if(players[0] != this.gameObject )
		{
			partner = players[0].GetComponent<PlayerState>();
		}
		else
		{
			partner = players[1].GetComponent<PlayerState>();
		}


		if(partner.m_PlayerState != PlayerStates.Dead)
    	{
      	   //  if so enable our player and set his , transform. position to equal near our other character
    	}
		else 
    	{
       	  // Call checkpoint reset function 
    	}
	
	}

	//not implemented/tested yet since no enemies
	protected void KnockBack()
	{
   		//   get gameObject that hit us being stored in member variable
   		//	get the direction of the attacker by using his position and ours to get a normalized vector.
   		//   times the new vector3 by -1 so we are now moving away 
   		//   multiply the normalized vector3 by knockback speed and delta time
   		//   Call our playerMovement.SetKnockBack(pass in the vector3);  
	}
	                                                                                                                                    

	//returns when the player is interacting
	public bool getInteracting()
	{
  		if(m_CurrentInteraction != null)
     	{
       		return true;
     	}
     	else
     	{
       		return false;
     	}
	}                                                                                                                                          

	//returns the type of the current interaction
	public InteractableType interactionType ()
	{
 		return m_CurrentInteraction.getType();
	}
                        
	//decides what to do when youre moving
	protected void MovingFunction()
	{
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
					enterSecond();
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

                                                                                                                                        

	//decides what to do when youre stationary
	protected void IdleFunction()
	{
		if(m_IsAiming)
		{
			if(PlayerInput.Instance.getIsAiming())
			{
				toggleAiming();
				return;
			}

			if(PlayerInput.Instance.getUseItem())
			{
				aimAttack();
			}
			
			m_PlayerState = PlayerStates.Default;
			return;
		}
		if(!m_UsingSecondItem)
		{
			if(PlayerInput.Instance.getEnviromentInteraction())
			{
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
		}
		
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
					//Movement shit
					PlayerMovement movement = gameObject.GetComponent<PlayerMovement>();
					
					if(movement != null)
					{
						movement.setCanMove ( false );
					}
					//----------------------------------------------------
		        	m_UsingSecondItem = true;
		        	m_PlayerState = PlayerStates.Default;
					enterSecond();
			        return;
	            }
	        }
	    }
		
		if(PlayerInput.Instance.getIsAiming())
		{
			toggleAiming();
		}

	    if(PlayerInput.Instance.getUseItem())
	    {
	    	attack();
	        m_PlayerState = PlayerStates.Default;
	        return;
	    }        
	  
		m_PlayerState = PlayerStates.Default;
	}
                                                                                                                                         
	//toggles whether the player is aiming or not
	public void toggleAiming()
	{
		m_IsAiming = !m_IsAiming;
		m_CameraController.toggleAiming ( );
	}

	//returns whether the player is aiming or not
	public bool getIsAiming()
	{
		return m_IsAiming;
	}

	/// <summary>
	/// this is a function to see what state we are in.
	/// </summary>
	protected void Default()
	{
		if(!m_UsingSecondItem)
		{
			if(PlayerInput.Instance.getSwitchInput())
			{ 
				if (m_CameraController.isAbleToSwitch())
				{
					CharacterSwitch.Instance.switchCharacters();
				}
				return;
			}
		}

		if(m_Health.getHealth() <= 0)
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

	//used by interactions to tell the player statemachine that they are in range
	public void interactionInRange(InteractableBaseClass Interaction)
	{
		for (int i = 0; i < m_InteractionsInRange.Count; i++)
		{
			if (m_InteractionsInRange[i] == Interaction)
				return;
		}
		m_InteractionsInRange.Add(Interaction);
	}

	//used by interactions to tell the player state machine that an interaction is now out of range
	public void interactionOutOfRange(InteractableBaseClass interaction)
	{
	    m_InteractionsInRange.Remove(interaction);
	}

	//used to flag if the player should exit their second item
	public void setExitingSecond (bool isExiting)
	{
	    m_ExitingSecondItem = isExiting;
	}

	protected abstract bool getUseSecondItemInput();

	//checks to see if the player is able to use second item.
	protected abstract bool ableToEnterSecondItem();

	protected abstract void useSecondItem();

	protected abstract void attack();

	protected abstract void enterSecond();

	protected abstract void aimAttack();  // <- derek cannot implement, does not have a aim attack.

	//observer design pattern things
	public void recieveEvent(Subject sender, ObeserverEvents recievedEvent)
	{
		if(recievedEvent == ObeserverEvents.PauseGame || recievedEvent == ObeserverEvents.StartGame)
		{
			m_IsPaused = !m_IsPaused;
			return;
		}

		if(recievedEvent == ObeserverEvents.CharacterSwitch)
		{
			m_IsActive = !m_IsActive;

			gameObject.GetComponent<PlayerMovement>().setCanMove(m_IsActive);

			//Debug.Log(this.gameObject.name + " is " + m_IsActive);

			if(m_IsActive)
			{
				//Debug.Log(this.gameObject.name + " is telling the camera to switch to it");
				GameObject.FindGameObjectWithTag("Camera").GetComponentInChildren<CameraController>().switchTo(this.gameObject.transform);
			}

			return;
		}
	}
}