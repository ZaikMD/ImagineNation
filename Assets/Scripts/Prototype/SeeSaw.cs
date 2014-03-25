using UnityEngine;
using System.Collections;

public class SeeSaw : MonoBehaviour {


	private GameObject m_SittingPlayer;
	private GameObject m_JumpingPlayer;
	public GameObject m_JumpPoint;
	public GameObject m_SitPoint;
	private  bool m_IsLerping;

	private Vector3 m_JumpPointPos;
	private Vector3 m_SitPointPos;
	private float m_ResetTimer;
	private bool m_HasLaunchedPlayer;
	private float m_LerpTime;
	void Start()
	{
		//Initialize the jump and sit points.
		m_JumpPointPos = m_JumpPoint.transform.position;
		m_SitPointPos = m_SitPoint.transform.position;
		m_ResetTimer = 5.0f;
		m_HasLaunchedPlayer = false;
		m_LerpTime = 5.0f;
	}
	
	void Update()
	{
		if(m_IsLerping)
		{
			m_JumpingPlayer.transform.position = Vector3.Lerp (m_JumpingPlayer.transform.position, m_JumpPointPos ,m_LerpTime);     //Lerp m_JumpingPlayer to m_JumpPoint
			if(m_JumpingPlayer.transform.position == m_JumpPointPos)
			{
				m_JumpingPlayer.transform.parent = null;//If the jumping player has reached the jump point, then notify the player and give back control
				m_IsLerping = false;//Also m_IsLerping = false && call launchPlayer();
				launchPlayer();
			}
		}
		if(m_HasLaunchedPlayer == true)
		{
			m_ResetTimer -= Time.deltaTime;
		}
		if(m_ResetTimer < 0.0f)
		{
			reset();
		}
	}
	void makeChild(GameObject obj)
	{

		m_SittingPlayer = obj.gameObject; //Set m_SittingPlayer to obj  
		m_SittingPlayer.transform.parent = this.transform; //Make obj the child of the SeeSaw 
		m_SittingPlayer.transform.position = m_SitPointPos;  //Set the obj's position to m_SitPoint's position

	}
	
	void launchPlayer()
	{
		m_SittingPlayer.transform.parent = null;  //Terminate Parent-child relation between m_SittingPlayer and the SeeSaw
		m_SittingPlayer.transform.Translate (0, 50.0f, 0.0f); //Apply force to m_SittingPlayer
		m_SittingPlayer = null;

	}
	
	void playerJumping(GameObject obj)
	{
		//this gets called by DivingBoard
		m_JumpingPlayer = obj.gameObject;//Set m_JumpingPlayer to obj

		m_JumpingPlayer.transform.parent = this.transform; //Make obj the child of the SeeSaw

		m_IsLerping = true;//Start the lerp to m_JumpPoint
	}
	
	void exitSeeSaw(GameObject obj)
	{
		//this gets called by the sitting player
		m_SittingPlayer.transform.parent = null;//Terminate parent-child relation between m_SittingPlayer and SeeSaw
		m_SittingPlayer = null;//Clear m_SittingPlayer
	}
	
	void reset()
	{
		//this should be called after the player is launched, resetting the SeeSaw back to it's original Position

		m_SitPoint.transform.position = m_SitPointPos;
		m_JumpPoint.transform.position = m_JumpPointPos;
		//Reset points back to original positions and terminate any Parent-Child relations

		m_JumpingPlayer.transform.parent = null;
		m_SittingPlayer.transform.parent = null;
		m_JumpingPlayer = null;
		m_SittingPlayer = null;
		m_ResetTimer = 5.0f;
		m_HasLaunchedPlayer = false;
		//clear m_JumpingPlayer and m_SittingPlayer;
	}
}
