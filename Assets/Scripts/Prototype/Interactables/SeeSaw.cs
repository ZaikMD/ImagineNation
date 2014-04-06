using UnityEngine;
using System.Collections;

public class SeeSaw : InteractableBaseClass, Observer
{
	//Subject
	public Subject m_DropZone;

	//Players
	GameObject m_SittingPlayer;
	GameObject m_JumpingPlayer;

	//Moveable block
	public GameObject m_Block;


	//Seesaw points
	public GameObject m_JumpPoint;
	public GameObject m_SitPoint;
	public GameObject m_JumpEndPoint;

	//Pieces
	public GameObject m_TopPiece;
	public GameObject m_BottomPiece;

	//Bools
	public bool m_NeedsTopPiece = false;   //DESIGNERS: SET THESE 
	public bool m_NeedsBottomPiece = false;
	public bool m_HasTopPiece = true;
	public bool m_HasBottomPiece = true;
	bool m_IsLerping = false;
	bool m_HasLaunchedPlayer = false;
	bool m_PlayerHasJumped = false;

	//Points
	Vector3 m_JumpPointPos;
	Vector3 m_SitPointPos;
	Vector3 m_SitPointEndPos;
	Vector3 m_BlockPosition;

	//Timer
	float m_ResetTimer = 5.0f;
	const float LERP_TIME = 0.05f;

	bool m_IsEnabled = true;

	public float m_LaunchHeight = 10.0f;

	//Initialization
	void Start()
	{
		//Initialize
		m_JumpPointPos = m_JumpPoint.transform.position;
		m_SitPointPos = m_SitPoint.transform.position;
		m_ResetTimer = 5.0f;
		m_HasLaunchedPlayer = false;

		m_SitPointEndPos = new Vector3 (m_SitPoint.transform.position.x, m_JumpPoint.transform.position.y, m_SitPoint.transform.position.z);


		m_Type = InteractableType.SeeSaw;
		m_IsExitable = true;

		GameManager.Instance.addObserver (this);

		if(m_NeedsTopPiece == true)    //If it needs a piece, set the pieces active or not accordingly 
		{
			m_TopPiece.gameObject.SetActive(false);
			m_JumpPoint.gameObject.SetActive(false);
		}

		if(m_NeedsBottomPiece == true)
		{
			m_BottomPiece.gameObject.SetActive(false);
			m_SitPoint.gameObject.SetActive(false);
		}
		if(m_DropZone != null)
		{
			m_DropZone.addObserver (this);
		}

		if(m_Block != null)
		{
			m_BlockPosition = m_Block.transform.position;
		}
	}


	void Update()
	{
		if(m_IsEnabled)
		{
			if(m_HasTopPiece && m_HasBottomPiece) // if it has both pieces 
			{
				if(m_IsLerping) //If it's lerping
				{
					m_JumpingPlayer.transform.position = Vector3.Lerp(m_JumpingPlayer.transform.position, m_JumpEndPoint.transform.position, LERP_TIME); //Move the Jumping player to the ground

					if(m_JumpingPlayer != null) //If the jumping player isn't null
					{
						if(m_JumpPoint.transform.position.y <= m_JumpEndPoint.transform.position.y + 0.5f) // If the jump point is a bit higher than the end point
						{
							m_JumpingPlayer.transform.parent = null;     //Release player
							if(m_Block != null && m_JumpingPlayer.name != m_Block.name)
							{
								m_JumpingPlayer.gameObject.GetComponent<PlayerState>().exitInteracting(); //Tell the player to stop interacting
							}
						}
				
					}

					m_SitPoint.transform.position = Vector3.Lerp(m_SitPoint.transform.position, m_SitPointEndPos, LERP_TIME); //Lerp the sit and jump points to their positions
					m_JumpPoint.transform.position = Vector3.Lerp(m_JumpPoint.transform.position, m_JumpEndPoint.transform.position, LERP_TIME);
					launchPlayer(); //Launch the Sitting Player

					if(m_HasLaunchedPlayer == true)
					{
						m_ResetTimer -= Time.deltaTime; //Start the reset timer
								
					}
					if(m_ResetTimer < 0.0f)
					{
						reset(); //Reset
					}
				}
			}
		}
	}
	//Sit
	public void makeChild(GameObject obj)  //Called by player, makes the player that interacted with it the child of the seesaw and disables it's movement
	{
		if(obj != null)
		{
			if(m_SittingPlayer == null) //Make sure there isn't a player sitting already
			{
				//Set m_SittingPlayer to obj 
				m_SittingPlayer = obj.gameObject; 
				
				//Make obj the child of the SeeSaw
				m_SittingPlayer.transform.parent = this.transform;
				
				//Set the obj's position to m_SitPoint's position
				m_SittingPlayer.transform.position = m_SitPointPos;
			}
		} 

		if(obj == null) //If the obj passed in is null, set the sitting player to null
		{
			m_SittingPlayer = null;
		}


	}

	//Launch
	void launchPlayer()
	{
		if(!m_HasLaunchedPlayer) //If a player hasn't been launched already, and there is a player sitting down, launch them
		{
			if(m_SittingPlayer != null)
			{
				m_SittingPlayer.gameObject.GetComponent<PlayerMovement>().setCanMove(true); //Give back player movement
				m_SittingPlayer.gameObject.GetComponent<PlayerMovement>().LaunchJump(m_LaunchHeight); //Launch the player

				m_SittingPlayer.gameObject.GetComponent<PlayerState>().exitInteracting(); //Tell the player to stop interacting/
				m_HasLaunchedPlayer = true; 
			}

			else
			{
				m_HasLaunchedPlayer = true;
			}

		}

	}

	//This gets called by DivingBoard to jump
	public void playerJumping(GameObject obj)
	{
		//Set m_JumpingPlayer to obj
		m_JumpingPlayer = obj;

		if(obj.tag == "MoveableBlock")
		{
			m_Block = obj; //Check to see if it was a block that was passed in
		}

		//Make obj the child of the SeeSaw
		m_JumpingPlayer.transform.parent = this.transform;

		m_IsLerping = true; //Start the lerp
	}

	//This should be called after the player is launched, resetting the SeeSaw back to it's original Position
	void reset()
	{
		//Reset points back to original positions
		m_SitPoint.transform.position = m_SitPointPos;
		m_JumpPoint.transform.position = m_JumpPointPos;


		//Clear m_JumpingPlayer and m_SittingPlayer;
		if(m_JumpingPlayer != null)
		{
			m_JumpingPlayer.transform.parent = null;
			m_JumpingPlayer = null;
		}

		if(m_SittingPlayer!= null)
		{
			m_SittingPlayer.transform.parent = null;
			m_SittingPlayer = null;
		}

		m_Block.transform.position = m_BlockPosition;

		//Reset states and timer
		m_ResetTimer = 5.0f;
		m_HasLaunchedPlayer = false;
		m_PlayerHasJumped = false;
		m_IsLerping = false;
	}
	
	void OnTriggerEnter(Collider obj)
	{
		if(obj.tag == "Player")
		{
			obj.gameObject.GetComponent<PlayerState>().interactionInRange(this); //Set this in range for player interactions
		}
	}
	
	void OnTriggerExit(Collider obj)
	{
		if(obj.tag == "Player")
		{
			obj.gameObject.GetComponent<PlayerState>().interactionOutOfRange(this); //Take this out of range for player interactions
		}
	}

	public void recieveEvent(Subject sender, ObeserverEvents recievedEvent)
	{
		if(recievedEvent == ObeserverEvents.PauseGame ||recievedEvent == ObeserverEvents.StartGame)
		{
			m_IsEnabled = !m_IsEnabled; //Enable the seesaw
		}
		if(recievedEvent == ObeserverEvents.PickUpIsAtDropZone)
		{
			if(m_NeedsTopPiece) //If the corrrect piece is placed in the drop zone, enable them and their points
			{
				m_TopPiece.gameObject.SetActive(true);
				m_SitPoint.gameObject.SetActive(true);
			}
			if(m_NeedsBottomPiece)
			{
				m_BottomPiece.gameObject.SetActive(true);
				m_SitPoint.gameObject.SetActive(true);
			}
		}
	}
}
