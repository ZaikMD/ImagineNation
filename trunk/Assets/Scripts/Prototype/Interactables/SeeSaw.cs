using UnityEngine;
using System.Collections;

public class SeeSaw : InteractableBaseClass, Observer
{
	//Subject
	public Subject m_DropZone; //The dropZone

	//Players
	GameObject m_SittingPlayer;
	GameObject m_JumpingPlayer;
	public GameObject m_Block;

	//Seesaw points
	public GameObject m_JumpPoint;
	public GameObject m_SitPoint;
	public GameObject m_JumpEndPoint;

	//Pieces
	public GameObject m_TopPiece;
	public GameObject m_BottomPiece;

	//Bools
	public bool m_HasTopPiece = true;
	public bool m_HasBottomPiece = true;
	bool m_IsLerping;
	bool m_HasLaunchedPlayer;
	bool m_IsPlatformLerping = false;
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
	public float m_BottomLaunchHeight = 5.0f;

	//Initialization
	void Start()
	{
		//Initialize the jump and sit points.
		m_JumpPointPos = m_JumpPoint.transform.position;
		m_SitPointPos = m_SitPoint.transform.position;
		m_ResetTimer = 5.0f;
		m_HasLaunchedPlayer = false;

		m_SitPointEndPos = new Vector3 (m_SitPoint.transform.position.x, m_SitPoint.transform.position.y + m_BottomLaunchHeight, m_SitPoint.transform.position.z);

		m_Type = InteractableType.SeeSaw;
		
		m_IsExitable = true;

		GameManager.Instance.addObserver (this);

		if(!m_HasTopPiece)
		{
			m_TopPiece.gameObject.SetActive(false);
			m_JumpPoint.gameObject.SetActive(false);
		}

		if(!m_HasBottomPiece)
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

	//Launch
	void Update()
	{
		if(m_IsEnabled)
		{
			if(m_HasTopPiece && m_HasBottomPiece)
			{
					if(m_IsLerping)
					{
						launchPlayer();
						m_JumpingPlayer.transform.position = Vector3.Lerp(m_JumpingPlayer.transform.position, m_JumpEndPoint.transform.position, LERP_TIME);

						m_SitPoint.transform.position = Vector3.Lerp(m_SitPoint.transform.position, m_SitPointEndPos, LERP_TIME/2);
						m_JumpPoint.transform.position = Vector3.Lerp(m_JumpPoint.transform.position, m_JumpEndPoint.transform.position, LERP_TIME);

						if(m_JumpingPlayer != null)
						{
							if(m_JumpPoint.transform.position.y <= m_JumpEndPoint.transform.position.y + 0.5f)
							{
								m_JumpingPlayer.transform.parent = null;
								
								if(m_Block == null)
								{
									m_JumpingPlayer.gameObject.GetComponent<PlayerState>().exitInteracting();
								}
								m_IsLerping = false;
								Debug.Log("PlayerLaunched");
							}
						}
						
					}

			if(m_HasLaunchedPlayer)
			{
				m_ResetTimer -= Time.deltaTime;
			}
			if(m_ResetTimer < 0.0f)
			{
				reset();
			}
		}
	}
}
	//Sit
	public void makeChild(GameObject obj)  //Called by player
	{
		if(obj != null)
		{
			if(m_SittingPlayer == null)
			{
				if(obj.name != "Alex")
				{
					//Set m_SittingPlayer to obj 
					m_SittingPlayer = obj.gameObject; 
					//Make obj the child of the SeeSaw
					m_SittingPlayer.transform.parent = this.transform;
					//Set the obj's position to m_SitPoint's position
					m_SittingPlayer.transform.position = m_SitPointPos;
				}

				else
				{
					obj.GetComponent<PlayerState>().exitInteracting();
				}
			}
		} 

		if(obj == null)
		{
			m_SittingPlayer = null;
		}
	}

	//Launch
	void launchPlayer()
	{
		if(!m_HasLaunchedPlayer)
		{
			if(m_SittingPlayer != null)
			{
				CharacterSwitch.Instance.switchCharacters();
				m_SittingPlayer.gameObject.GetComponent<PlayerMovement>().setCanMove(true);
				m_SittingPlayer.gameObject.GetComponent<PlayerMovement>().LaunchJump(m_LaunchHeight);
				m_SittingPlayer.gameObject.GetComponent<PlayerState>().exitInteracting();
				m_HasLaunchedPlayer = true; 

				//SoundManager.Instance.playSound(Sounds.SeesawJump, this.transform.position);
			}

		}

	}

	//This gets called by DivingBoard to jump
	public void playerJumping(GameObject obj)
	{
		if(m_SittingPlayer != null)
		{
			//Set m_JumpingPlayer to obj
			m_JumpingPlayer = obj;

			if(obj.tag == "MoveableBlock")
			{
				m_Block = obj;
			}

			//Make obj the child of the SeeSaw
			m_JumpingPlayer.transform.parent = this.transform;

			m_IsLerping = true; //Start the lerp to m_JumpPoint
		}

		else
		{
			obj.gameObject.GetComponent<PlayerState>().exitInteracting();
		}

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

		//Reset states and timer
		m_ResetTimer = 5.0f;
		m_HasLaunchedPlayer = false;
		m_IsPlatformLerping = false;
		m_PlayerHasJumped = false;
		m_IsLerping = false;
	}
	
	void OnTriggerEnter(Collider obj)
	{
		if(obj.tag == "Player")
		{
			obj.gameObject.GetComponent<PlayerState>().interactionInRange(this);
		}
	}
	
	void OnTriggerExit(Collider obj)
	{
		if(obj.tag == "Player")
		{
			obj.gameObject.GetComponent<PlayerState>().interactionOutOfRange(this);
		}
	}

	public void recieveEvent(Subject sender, ObeserverEvents recievedEvent)
	{
		if(recievedEvent == ObeserverEvents.PauseGame ||recievedEvent == ObeserverEvents.StartGame)
		{
			m_IsEnabled = !m_IsEnabled;
		}
		if(recievedEvent == ObeserverEvents.PickUpIsAtDropZone)
		{
			if(!m_HasTopPiece)
			{
				m_TopPiece.gameObject.SetActive(true);
				m_JumpPoint.gameObject.SetActive(true);
				m_HasTopPiece = true;
			}
			if(!m_HasBottomPiece)
			{
				m_BottomPiece.gameObject.SetActive(true);
				m_SitPoint.gameObject.SetActive(true);
				m_HasBottomPiece = true;
			}
		}
	}
}
