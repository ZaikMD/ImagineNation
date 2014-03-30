using UnityEngine;
using System.Collections;

public struct movementInput
{
	public float x;
	public float y;
}

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


	movementInput m_PlayerMovement; //why is this not a vector2 instead?
	bool m_Jump;
	bool m_SwitchChars;
	bool m_InteractWithEnviroment;
	movementInput m_CameraMovement;
	bool m_Pause;
	bool m_UseItem;
	bool m_IsAiming;

	bool m_JumpHeld;
	bool m_SwitchCharsHeld;
	bool m_InteractWithEnviromentHeld;
	bool m_PauseHeld;
	bool m_UseItemHeld;

	void Start () 
	{
		Debug.Log ("Start");
		m_PlayerMovement.x = 0.0f; 
		m_PlayerMovement.y = 0.0f;
		m_Jump = false;
		m_SwitchChars = false;
		m_InteractWithEnviroment = false;
		m_CameraMovement.x = 0.0f;
		m_CameraMovement.y = 0.0f;
		m_Pause = false;
		m_UseItem = false;
		m_IsAiming = false;
    }
    
    void Update()
	{
		//reset the input
		m_PlayerMovement.x = 0.0f; 
		m_PlayerMovement.y = 0.0f;
		m_Jump = false;
		m_SwitchChars = false;
		m_InteractWithEnviroment = false;
		m_CameraMovement.x = 0.0f;
		m_CameraMovement.y = 0.0f;
		m_Pause = false;
        m_UseItem = false;
        m_IsAiming = false;
		//--------------------------------------------

		//get the movement input
		//http://wiki.unity3d.com/index.php?title=Xbox360Controller

		//controller input


		m_PlayerMovement.x = Input.GetAxis (LEFT_STICK_H);
		m_PlayerMovement.y = Input.GetAxis (LEFT_STICK_V);



		//get the jump input
		if(Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown(A))
		{
			m_Jump = true;
			Debug.Log("Jump");
		}
		else
		{
			m_Jump = false;
		}

		//get the jump held input
		if(Input.GetKey(KeyCode.Space) || Input.GetButton(A))
		{
			m_JumpHeld = true;
		}
		else
		{
			m_JumpHeld = false;
		}

		//get the switch character input
		if(Input.GetKeyDown(KeyCode.Tab) || Input.GetButtonDown(Y))
		{
			m_SwitchChars = true;
			Debug.Log("Switch Characters");
		}
		else
		{
			m_SwitchChars = false;
		}

		//get the switch character held input
		if(Input.GetKey(KeyCode.Tab) || Input.GetButton(Y))
		{
			m_SwitchCharsHeld = true;
		}
		else
		{
			m_SwitchCharsHeld = false;
		}

		//get the enviroment interaction input
		if(Input.GetKeyDown(KeyCode.F) || Input.GetButtonDown(B))
		{
			m_InteractWithEnviroment = true;
			Debug.Log("Interact With Enviroment");
		}
		else
		{
			m_InteractWithEnviroment = false;
		}

		//get the enviroment interaction held input
		if(Input.GetKey(KeyCode.F) || Input.GetButton(B))
		{
			m_InteractWithEnviromentHeld = true;
		}
		else
		{
			m_InteractWithEnviromentHeld = false;
		}

		//get Camera Input
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

		//Debug.Log ("Camera x: " + m_CameraMovement.x + "Camera y: " + m_CameraMovement.y);

		//get the Puase input
		if(Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown(START))
		{
			m_Pause = true;
			Debug.Log("Pause");
		}
		else
		{
			m_Pause = false;
		}

		//get the Puase held input
		if(Input.GetKey(KeyCode.Escape) || Input.GetButton(START))
		{
			m_PauseHeld = true;
		}
		else
		{
			m_PauseHeld = false;
		}

		//get the use item input
		if(Input.GetMouseButtonDown(0) || Input.GetButtonDown(X))
		{
			m_UseItem = true;
			Debug.Log("Use Item");
		}
		else
		{
			m_UseItem = false;
		}

		//get the use item held input
		if(Input.GetMouseButton(0) || Input.GetButton(X))
		{
			m_UseItemHeld = true;
		}
		else
		{
			m_UseItemHeld = false;
		}

		//get the aiming input
		//might have to invert the triggers data in the input manager
		if(Input.GetKey(KeyCode.LeftShift) || (Input.GetAxis(LEFT_TRIGGER) != 0.0f))
		{
			m_IsAiming = true;
			Debug.Log("Aiming");
		}
		else
		{
			m_IsAiming = false;
		}
	}

	public movementInput getMovementInput()
	{
		return m_PlayerMovement;
	}

	public bool getJumpInput()
	{
		return m_Jump;
	}

	public bool getSwitchInput()
	{
		return m_SwitchChars;
	}

	public bool getEnviromentInteraction()
	{
		return m_InteractWithEnviroment;
	}

	public movementInput getCameraMovement()
	{
		return m_CameraMovement;
	}

	public bool getPause()
	{
		return m_Pause;
	}

	public bool getUseItem()
	{
		return m_UseItem;
	}

	public bool getIsAiming()
	{
		return m_IsAiming;
	}

	public bool getJumpHeld()
	{
		return m_JumpHeld;
	}

	public bool getSwitchCharsHeld()
	{
		return m_SwitchCharsHeld;
	}

	public bool getInteractWithEnviromentHeld()
	{
		return m_InteractWithEnviromentHeld;
	}

	public bool getPauseHeld()
	{
		return m_PauseHeld;
	}

	public bool getUseItemHeld()
	{
		return m_UseItemHeld;
	}
}