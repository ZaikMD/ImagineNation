using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour
{
	public static PlayerInput Instance{ get; private set; }
	// Use this for initialization
	
	void Awake()
	{
		//if theres another instance (there shouldnt be) destroy it... there can be only one
		if(Instance != null && Instance != this)
		{
			//destroy all other instances
			Destroy(gameObject);
		}
		
		//set the instance
		Instance = this;
		
		//prevents this object being destroyed between scene loads
		DontDestroyOnLoad(gameObject);
	}

	//--------------------------------------------------------------------------------------------------
	//const string PLAYER_1 = "joystick1 ";
	
	const string A = "360_A";
	const string B = "360_B";
	const string X = "360_X";
	const string Y = "360_Y";
	const string LB = "360_LB";
	const string RB = "360_RB";
	const string BACK = "360_BACK";
	const string START = "360_START";
	const string L3 = "360_L3";
	const string R3 = "360_R3";
	
	//		AXIS
	//Analog Axis sensitivity should be 1, digital should be 1000.
	
	const string LEFT_STICK_V = "Vertical";
	const string LEFT_STICK_H = "Horizontal";
	const string RIGHT_STICK_V = "360_RIGHT_STICK_VERTICAL";
	const string RIGHT_STICK_H = "360_RIGHT_STICK_HORIZONTAL";
	const string DPAD_V = "360_DPAD_VERTICAL";
	const string DPAD_H = "360_DPAD_HORIZONTAL";
	//this wont work in multiplayer
	//unity will interpert all the triggers with one axis
	const string LEFT_TRIGGER = "360_LEFT_TRIGGER";
	const string RIGHT_TRIGGER = "360_RIGHT_TRIGGER";

	//Triggers (Left Trigger Axis: 9th, 
	//Right Trigger Axis: 10th (Both axis are 0-1))
	//----------------------------------------------------

	public Vector2 getMovementInput()
	{
		return new Vector2(Input.GetAxis (LEFT_STICK_H), Input.GetAxis (LEFT_STICK_V));
	}

	public bool getJumpInput()
	{
		//get the jump input
		if(Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown(A))
		{
			return true;
		}
		else
		{
			return false;
		} 
	}

	public bool getSwitchInput()
	{
		if(Input.GetKeyDown(KeyCode.Tab) || Input.GetButtonDown(Y))
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public bool getEnviromentInteraction()
	{
		if(Input.GetKeyDown(KeyCode.F) || Input.GetButtonDown(B))
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public Vector2 getCameraMovement()
	{
		Vector2 m_CameraMovement = new Vector2 ();

		float mouse = Input.GetAxis ("Mouse X");
		float gamepad = Input.GetAxis (RIGHT_STICK_H);
		if( Mathf.Abs(mouse) > 0.0f || Mathf.Abs(gamepad) > 0.0f)
		{
			if( Mathf.Abs(mouse) > Mathf.Abs(gamepad))
			{
				m_CameraMovement.x = mouse;
			}
			else
			{
				m_CameraMovement.x = gamepad;
			}
		}
		
		mouse = Input.GetAxis("Mouse Y");
		gamepad = Input.GetAxis(RIGHT_STICK_V);
		if( Mathf.Abs(mouse) > 0.0f || Mathf.Abs(gamepad) > 0.0f)
		{
			if( Mathf.Abs(mouse) > Mathf.Abs(gamepad))
			{
				m_CameraMovement.y = mouse;
			}
			else
			{
				m_CameraMovement.y = gamepad;
			}
		}

		return m_CameraMovement;
	}

	public bool getPause()
	{
		if(Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown(START))
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public bool getUseItem()
	{
		if(Input.GetMouseButtonDown(0) || Input.GetButtonDown(X))
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public bool getIsAimingHeld()
	{
		if(Input.GetKey(KeyCode.LeftShift) || (Input.GetButton(LB)))
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public bool getIsAiming()
	{
		if(Input.GetKeyDown(KeyCode.LeftShift) || Input.GetButtonDown(LB))
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public bool getJumpHeld()
	{
		if(Input.GetKey(KeyCode.Space) || Input.GetButton(A))
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public bool getSwitchCharsHeld()
	{
		if(Input.GetKey(KeyCode.Tab) || Input.GetButton(Y))
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public bool getEnviromentInteractionHeld()
	{
		if(Input.GetKey(KeyCode.F) || Input.GetButton(B))
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public bool getPauseHeld()
	{
		if(Input.GetKey(KeyCode.Escape) || Input.GetButton(START))
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public bool getUseItemHeld()
	{
		if(Input.GetMouseButton(0) || Input.GetButton(X))
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}