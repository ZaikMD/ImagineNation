using UnityEngine;
using System.Collections;
/// <summary>
/// 
/// 
/// Created by Zach Dubuc
/// 
/// This class will inherit from Attackable and will be responsible for toggling a specified lever 
/// and determining whether or not it will on a timer. The lever will switch on and off based on the OnHit
 /// function from Attackable and the timer will be set in the unity editor if one is needed.
/// </summary>
/// 
/// 19/09/14 Matthew Whitlaw EDIT: Added a getActive function
/// 22/09/14 Zach Dubuc EDIT: Added a material when the switch is active/inactive
/// 08/10/2014 Zach Dubuc EDIT: Added in comments
/// 27/10/2014 Zach Dubuc EDIT: Added a rotation/ Movement to the switch, as well as a pause for camera
///  12/11/2014 Edit: Added in beenHit Overide- Zach Dubuc
public class Switch : SwitchBaseClass, Attackable
{
    //Bools
    public bool m_OnTimer;
    public float m_Timer;
	public bool m_Rotate;
	bool m_PlayingSound;
    
    //Save for the timer
	protected float m_SaveTimer;

	public GameObject m_MovingPiece;

	//Variables for Rotation/Movement

	Quaternion m_Angle;
	Vector3 m_MoveToPoint;
	float m_LerpTime;	

	//Variable for pausing in case the camera needs to show what happens

	public bool m_WillPauseForCamera;
	public float m_CameraPauseTimer;

	const ScriptPauseLevel PAUSE_LEVEL = ScriptPauseLevel.PauseMenu;

	//Sound varible
	private SFXManager m_SFX;

	// Use this for initialization
	void Start () 
    {
		m_SaveTimer = m_Timer;

		m_Angle.Set(0,
		            m_MovingPiece.transform.localRotation.y,
		            m_MovingPiece.transform.localRotation.z,
		            m_MovingPiece.transform.localRotation.w);

		m_MoveToPoint = new Vector3(m_MovingPiece.transform.position.x,
		                            m_MovingPiece.transform.position.y,
		                            m_MovingPiece.transform.position.z - 0.3f);

		m_LerpTime = 0.5f;

		m_SFX = SFXManager.Instance;
	}
	
	// Update is called once per frame
	void Update () 
    {
		m_PlayingSound = false;

        if (PauseScreen.shouldPause(PAUSE_LEVEL)) { return; }

		if(m_WillPauseForCamera) //If the switch will pause for the camera to show something
		{
			if(m_BeenHit) //If the switch has been hit to start pausing it
			{
				movePiece(); //Call move Piece which will move the pieces of the switch
				if(m_CameraPauseTimer <= 0) //If the Pause timer for the camera is done counting down
				{
					m_Activated = true; //The switch is activated

				}
				else
				{
					m_CameraPauseTimer -= Time.deltaTime; //OtherWise countdown
				}
			}
		}

		if(m_Activated) //If the lever is activated
		{
			movePiece();
			if(m_OnTimer) //If there is a timer
			{
                if (m_Timer <= 0) //If the timer is zero, reset the switch
				{
					resetLever();
				}

				else
				{
					m_Timer -= Time.deltaTime; //Otherwise decrement the timer
				}
			}
		}
	}

	void movePiece()
	{
		if(m_Rotate)
		{ //If the switch Rotates, then rotate it to the Angle
			m_MovingPiece.transform.localRotation = Quaternion.Lerp(m_MovingPiece.transform.localRotation, m_Angle, m_LerpTime);
		}
		if(!m_Rotate)
		{//Else it moves and is a button, so move it back a bit
			m_MovingPiece.transform.position = Vector3.Lerp(m_MovingPiece.transform.position, m_MoveToPoint, m_LerpTime);
		}

	}

	void resetLever() //Reset variables
	{
        m_Activated = false;
		m_Timer = m_SaveTimer;
	}

    public void onHit(LightCollider proj, float damage) //If the player hits the switch, set activated to true
    {
		//TO make sure we only play sounds once
		if(m_PlayingSound == false)
		{
			m_SFX.playSound(transform, Sounds.LeverHit);
			m_PlayingSound = true;
		}

		if(!m_WillPauseForCamera)
		{
       		m_Activated = true;
		}
		else
		{
			m_BeenHit = true;
		}
    }

    public void onHit(HeavyCollider proj, float damage) //If the player hits the switch, set activated to true
    {
		//TO make sure we only play sounds once
		if(m_PlayingSound == false)
		{
			m_SFX.playSound(transform, Sounds.LeverHit);
			m_PlayingSound = true;
		}

        if (!m_WillPauseForCamera)
        {
            m_Activated = true;
        }
        else
        {
            m_BeenHit = true;
        }
    }

    public void onHit(EnemyProjectile proj) //Ignore enemys
    {
		return;
    }

	public void onHit(EnemyProjectile proj, Vector3 KnockBackDirection) //Ignore enemys
	{
		return;
	}

	public override bool beenHit()
	{
		return m_BeenHit;
	}

	protected virtual void onUse()
	{

	}

	public override bool getActive () //Returns whether or not the switch is active
	{
		return m_Activated;
	}

}
