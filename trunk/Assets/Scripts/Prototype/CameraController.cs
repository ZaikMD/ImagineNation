/*TO USE

Attach this component to the camera.

Make this camera a child of the object you want to follow.

Set this camera's local position to where you'd like it in relation to the object you want to follow.





Created by Jason Hein on 3/1/2014


3/2/2014
	Changed Variable Names
3/15/2013
	Updated with aiming and transform switching
3/17/2014
	Updated LookAt function to lerp
	Now instantiates an origin on it's own, instead of being provided one
3/25/2014
	Slowed lookat lerping by a little
3/28/2014
	Increased orientation sensitivity
	Slightly optimized
3/30/2014
	Added reticle functions
4/2/2014
	Now automatically zooms in and out while moving
4/4/2014
	Now can collide with walls
4/5/2014
	Reticle movement smoothed out
4/6/2014
	Smoothed collision with walls
*/




using UnityEngine;
using System.Collections;

/// <summary>
/// Enables camera movement based on input, and the objects transform.
/// </summary>
[RequireComponent (typeof(Camera))]
public class CameraController : MonoBehaviour
{
	//What to follow
	Transform m_CameraFollow;

	//State
	enum CameraState
	{
		Default = 0,
		Aiming,
		Switching
	}
	CameraState m_State = CameraState.Default;


	//Sensitivity
	const float ROTATION_SENSITIVITY = 4.0f;
	const float ZOOM_SENSITIVITY = 0.5f;
	const float CAMERA_FOLLOW_SPEED = 0.15f;
	const float RANGE_TO_ENABLE_SWITCHING = 0.1f;
	const float LOOK_AT_SPEED = 0.04f;
	const float AIMING_LOOK_AT_FRONT_AMOUNT = 20.0f;
	const float AIMING_CAMERA_HEIGHT = 0.3f;
	const float ZOOM_RETURN_SPEED = 0.01f;
	const float AIMING_VERTICAL_SENSITIVITY = 13.0f;
	const float COLLISION_FIX_TIMER = 0.25f;
	const float COLLISION_TURN_SPEED = 0.3f;
	const float COLLISION_ZOOM_SPEED = 0.20f;

	//Looking helper variables
	const float FORWARD_AMOUNT = 2.0f;
	Vector3 m_LastLookAtPosition = Vector3.zero;

	//Zoom
	float m_Zoom = 0.65f;
	float m_CloseLimit = 0.1f;
	const float DEFAULT_ZOOM = 0.65f;
	const float BACK_ZOOM = 0.8f;
	float m_Zoom_Return = DEFAULT_ZOOM;

	//Aiming
	public bool m_EnabledAiming = true;
	Vector3 m_SavedLocalPosition = Vector3.zero;

	//Player movement for aiming
	PlayerMovement m_Movement;

	//Reticle
	Reticle m_Reticle;

	//Movement after collision
	float m_CollisionTimer = 0.0f;
	float m_CollisionOrientation = 0.0f;
	float m_Zoom_Collision = 0.0f;
	bool m_CollisionZoom = false;
	//Use m_Zoom_Return for colllision zoom (and backwards movement)
	



	// Initialization
	void Start ()
	{
		//Limit Zoom
		if (m_Zoom < m_CloseLimit)
		{
			m_Zoom = m_CloseLimit;
		}
		else if (m_Zoom > 1)
		{
			m_Zoom = 1;
		}

		//Set initial Transform to follow
		m_CameraFollow = transform.parent.transform;
		m_Movement = (PlayerMovement)m_CameraFollow.gameObject.GetComponent<PlayerMovement> ();

		//Create an origin point
		GameObject origin = new GameObject ();
		origin.transform.position = transform.parent.transform.position;

		//Set us to follow the transform we were a parent of
		transform.parent = origin.transform;

		//Load Reticle
		GameObject reticle = (GameObject)Instantiate(Resources.Load("Reticle"), m_CameraFollow.position + m_CameraFollow.forward, Quaternion.identity);
		m_Reticle = (Reticle)(reticle.GetComponent<Reticle>());
		m_Reticle.Load();
	}
	
	// Update
	void Update ()
	{
		updateCollisionTimer ();
		updatePosition();
		updateZoom ();
		updateOrientation ();
		updateLookPosition ();
		updateReticlePosition ();
		updateReticle2DPosition ();
	}

	//Updates timer for lerping after collisions
	void updateCollisionTimer()
	{
		//CollisionTimer
		if (m_CollisionTimer > 0.0f)
		{
			m_CollisionTimer -= Time.deltaTime;
		}
	}
	
	// Updates the position of the camera's origin
	void updatePosition()
	{
		if ( m_State == CameraState.Switching)
		{
		    if (Vector3.Distance(transform.parent.position, m_CameraFollow.position) < RANGE_TO_ENABLE_SWITCHING )
			{
				m_State = CameraState.Default;
			}
			else
			{
				transform.parent.position = Vector3.Lerp(transform.parent.position, m_CameraFollow.position , CAMERA_FOLLOW_SPEED);
			}
		}
		else if ( m_State == CameraState.Default )
		{
			//We follow the object's position, but not their rotation
			transform.parent.position = m_CameraFollow.position;
			//transform.parent.Rotate (PlayerMovement.getControllerProjection() - transform.parent.rotation.eulerAngles);
		}
		else if ( m_State == CameraState.Aiming )
		{
			//Where to Look At
			//Vector3 aPosition = ( AIMING_LOOK_AT_FRONT_AMOUNT * Vector3.Normalize(m_CameraFollow.forward) + m_CameraFollow.position );
			//transform.parent.position = new Vector3 ( aPosition.x, transform.parent.position.y, aPosition.z );

			//Fix bobbing effect
			//transform.position = m_CameraFollow.position + new Vector3 (0, AIMING_CAMERA_HEIGHT, 0) - m_CameraFollow.forward;
			transform.localPosition = new Vector3 (0, AIMING_CAMERA_HEIGHT, 0);
		}
	}

	// Updates the camera's zoom
	void updateZoom()
	{
		//Make sure there is zoom input
		if (m_State == CameraState.Default)
		{
			if (m_CollisionTimer > 0.0f && m_CollisionZoom)
			{
				//Set Camera Zoom
				setZoom(Mathf.Lerp(m_Zoom , m_Zoom_Collision, COLLISION_ZOOM_SPEED));
				return;
			}

			if (PlayerInput.Instance.getCameraMovement().y != 0)
			{
				//Set Camera Zoom
				setZoom(m_Zoom + -PlayerInput.Instance.getCameraMovement().y * (ZOOM_SENSITIVITY / 100.0f));
			}
			else if (m_Zoom != m_Zoom_Return && PlayerInput.Instance.getMovementInput() != Vector2.zero)
			{
				if (PlayerInput.Instance.getMovementInput().y < 0.0f)
				{
					if (m_Zoom_Return != BACK_ZOOM)
					{
						m_Zoom_Return = BACK_ZOOM;
					}
				}
				else
				{
					if (m_Zoom_Return != DEFAULT_ZOOM)
					{
						m_Zoom_Return = DEFAULT_ZOOM;
					}
				}
				setZoom(Mathf.Lerp(m_Zoom , m_Zoom_Return, ZOOM_RETURN_SPEED));
			}
		}
		else if (m_State == CameraState.Switching && m_Zoom != m_Zoom_Return)
		{
			setZoom(Mathf.Lerp(m_Zoom , m_Zoom_Return, ZOOM_RETURN_SPEED));
		}
	}

	// Zooms the camera in or out
	void setZoom(float aZoom)
	{
		//Get max zoom position for lerping
		Vector3 maxZoomPosition = transform.localPosition * (1 / m_Zoom);
		
		//Set Camera Zoom
		m_Zoom = aZoom;
		
		//Limit Zoom
		if (m_Zoom < m_CloseLimit)
		{
			m_Zoom = m_CloseLimit;
		}
		else if (m_Zoom > 1)
		{
			m_Zoom = 1;
		}
		
		//Set new camera position based on zoom
		transform.localPosition = Vector3.Lerp (Vector3.zero, maxZoomPosition, m_Zoom);
	}

	//Colliding
	void OnCollisionStay(Collision collision)
	{
		if (m_State != CameraState.Default)
		{
			return;
		}

		//By default do not zoom in or out from collision
		m_CollisionZoom = false;

		//Math variables for Raycasting
		RaycastHit hit;
		float distance = Vector3.Distance(transform.position, collision.transform.position);

		//Fix distance error
		if (distance == 0.0f)
		{
			distance = 1.0f;
		}

		//Right
		if (Physics.Raycast(transform.position, transform.right, out hit, distance) ||
			//Back RIght
		    Physics.Raycast(transform.position, -transform.forward + transform.right, out hit, distance))
		{
			m_CollisionOrientation = transform.parent.eulerAngles.y + 1.0f;
			m_CollisionTimer = COLLISION_FIX_TIMER;
		}
		//Left
		else if (Physics.Raycast(transform.position, -transform.right, out hit, distance) ||
		         //Back Left
		         Physics.Raycast(transform.position, -transform.forward - transform.right, out hit, distance))
		{
			m_CollisionOrientation = transform.parent.eulerAngles.y - 1.0f;
			m_CollisionTimer = COLLISION_FIX_TIMER;
		}
		/*
		distance = Vector3.Distance (collision.transform.position, m_CameraFollow.position);
		if (distance == 0.0f)
		{
			distance = 1.0f;
		}*/

		//Backwards
		if (Physics.Raycast(transform.position, -transform.forward, out hit, distance))
		{
			m_Zoom_Collision = Vector3.Distance(transform.localPosition, Vector3.zero) * (m_Zoom / Vector3.Distance(collision.transform.position, m_CameraFollow.position));
			m_CollisionZoom = true;
			m_CollisionTimer = COLLISION_FIX_TIMER;
		}

		/*
		//Backwards
		if (Physics.Raycast(m_CameraFollow.position, (transform.position - m_CameraFollow.position).normalized, out hit, distance))
		{
			float localDistance = Vector3.Distance(transform.localPosition, Vector3.zero);
			if (localDistance == 0.0f)
			{
				localDistance = 1.0f;
			}

			m_Zoom_Collision = localDistance * (m_Zoom / Vector3.Distance(collision.transform.position, m_CameraFollow.position));
			m_CollisionZoom = true;
			m_CollisionTimer = COLLISION_FIX_TIMER;
		}*/
	}

	// Updates revolution around the player
	void updateOrientation()
	{
		//Only turn Camera if there is input
		if (m_State == CameraState.Switching)
		{
			return;
		}
		else if (m_State == CameraState.Default)
		{
			if (m_CollisionTimer > 0.0f)
			{
				setOrientation (Mathf.Lerp(transform.parent.eulerAngles.y, m_CollisionOrientation, COLLISION_TURN_SPEED));

			}
			else
			{
				setOrientation (transform.parent.eulerAngles.y + (ROTATION_SENSITIVITY * PlayerInput.Instance.getCameraMovement().x));
			}
		}
		else if (m_State == CameraState.Aiming)
		{
			//Rotate the player
			if (m_Movement)
			{
				m_Movement.AimMovement();
			}
		}
	}

	// Sets revolution around the player
	void setOrientation(float orientation)
	{
		//Turn
		transform.parent.eulerAngles = new Vector3(transform.parent.eulerAngles.x, orientation, transform.parent.eulerAngles.z);
	}

	// Updates where to look
	void updateLookPosition ()
	{
		if ( m_State == CameraState.Aiming )
		{
			transform.LookAt (m_Reticle.transform.position);
		}
		else
		{
			//Where to Look
			m_LastLookAtPosition += m_CameraFollow.position;
			Vector3 positionToLookAt;
			if (PlayerInput.Instance.getMovementInput() == Vector2.zero)
			{
					positionToLookAt = Vector3.Lerp (m_LastLookAtPosition, m_CameraFollow.position + m_CameraFollow.forward, LOOK_AT_SPEED);
			}
			else
			{
					positionToLookAt = Vector3.Lerp (m_LastLookAtPosition, m_CameraFollow.position + (m_CameraFollow.forward * FORWARD_AMOUNT), LOOK_AT_SPEED);
			}

			//Look in front of object
			transform.LookAt (positionToLookAt);

			//Save the position for lerping
			m_LastLookAtPosition = positionToLookAt - m_CameraFollow.position;
		}
	}

	// Updates the position of the reticle
	void updateReticlePosition ()
	{
		//Check collision in front of the followed transform
		RaycastHit hit;

		//Camera is in aiming state
		if (m_State == CameraState.Aiming)
		{
			//Looking up and down
			if (PlayerInput.Instance.getCameraMovement().y != 0)
			{
				m_Reticle.setReticlePosition(m_Reticle.transform.position + m_Reticle.transform.up * PlayerInput.Instance.getCameraMovement().y * Time.deltaTime * ROTATION_SENSITIVITY * AIMING_VERTICAL_SENSITIVITY);
			}

			//Where reticle would normally be in relation to the player
			Vector3 localPositionToPlayer = (m_CameraFollow.forward * Reticle.RETICLE_DISTANCE) + new Vector3 (0, m_Reticle.getTargetPosition().y - m_CameraFollow.position.y, 0);

			//Raycast to close objects
			Physics.Raycast(m_CameraFollow.position + m_CameraFollow.forward, localPositionToPlayer.normalized, out hit, Reticle.RETICLE_DISTANCE);

			//Default reticle position
			m_Reticle.setReticlePosition (m_CameraFollow.position + localPositionToPlayer);

			//Set if reticle is red or blue
			if (hit.transform == null)
			{
				m_Reticle.SetIsOnSomething(false);
			}
			else
			{
				if ( hit.transform.gameObject.CompareTag("Enemy"))
				{
					m_Reticle.SetIsOnSomething(true);
				}
				else
				{
					m_Reticle.SetIsOnSomething(false);
				}
			}
		}

		//Default position of reticle
		else
		{
			m_Reticle.setReticlePosition (m_CameraFollow.position + (m_CameraFollow.forward * Reticle.RETICLE_DISTANCE));
		}
	}

	//Set 2D paint position
	void updateReticle2DPosition ()
	{
		m_Reticle.setReticleScreenPosition (camera.WorldToScreenPoint (m_Reticle.getTargetPosition()));
	}

	/// <summary>
	/// Switches to the camera controller that calls this function. The previous controller is provided in order to deactivate that controller, and set this controllers zoom and orietnation to be the same as the previous controller's zoom and orietnation.
	/// </summary>
	/// <param name="other">Other.</param>
	/*public void switchFrom(CameraController other)
	{
	//CAMERAS SWITCHING BETWEEN EACH OTHER


		//Switch Active Camera
		this.camera.enabled = true;
		other.camera.enabled = false;

		//Set this controllers properties to the others properties
		updatePosition();
		setZoom (other.m_Zoom);
		setOrientationOfCamera(other.transform.parent.eulerAngles.y);
		updateLookPosition ();
	}*/ 

	/// <summary>
	/// Provided a new transform to follow, this will switch the camera to following that new transform.
	/// </summary>
	/// <param name="newFollowedTransform">New followed transform.</param>
	public void switchTo (Transform newFollowedTransform)
	{
		if ( m_State == CameraState.Switching || m_State == CameraState.Aiming)
		{
			return;
		}

		//setToNormal ();
		m_State = CameraState.Switching;
		m_CameraFollow = newFollowedTransform;
		m_Movement = (PlayerMovement)m_CameraFollow.gameObject.GetComponent<PlayerMovement> ();
	}

	/// <summary>
	/// Toggles between aiming and default view.
	/// </summary>
	public void toggleAiming ( )
	{
		if ( m_State == CameraState.Switching)
		{
			return;
		}
		else if ( m_State == CameraState.Default )
		{
			if (m_EnabledAiming)
			{
				setToAiming ();
			}
		}
		else
		{
			setToNormal ();
		}

		if (m_State == CameraState.Aiming && m_Movement != null)
		{
			m_Movement.AimMovement();
		}
	}

	/// <summary>
	/// Enables aiming for this camera.
	/// </summary>
	public void enableAiming()
	{
		m_EnabledAiming = true;
	}

	/// <summary>
	/// Disables aiming for this camera.
	/// </summary>
	public void disableAiming()
	{
		m_EnabledAiming = false;
		if (m_State == CameraState.Aiming)
		{
			setToNormal ();
		}
	}

	/// <summary>
	/// Sets to the camera to aiming view.
	/// </summary>
	public void setToAiming ()
	{
		if ( m_State != CameraState.Default )
		{
			return;
		}

		//Disable moving while aiming
		if (m_Movement != null)
		{
			m_Movement.setCanMove(false);
		}
		
		//Set positions
		m_SavedLocalPosition = transform.localPosition;
		transform.localPosition = Vector3.zero;
		transform.parent.position = transform.parent.transform.position + new Vector3 (0, AIMING_CAMERA_HEIGHT, 0);
		m_State = CameraState.Aiming;

		//Draw reticle while aiming
		m_Reticle.canDraw(true);
	}

	/// <summary>
	/// Sets to the camera to the default view.
	/// </summary>
	public void setToNormal ()
	{
		if ( m_State != CameraState.Aiming)
		{
			return;
		}

		//Disable moving while aiming
		if (m_Movement != null)
		{
			m_Movement.setCanMove(true);
		}

		transform.parent.position = m_CameraFollow.position;
		transform.localPosition = m_SavedLocalPosition;
		setOrientation (m_CameraFollow.position.y);
		m_State = CameraState.Default;

		//Do not draw reticle by default
		m_Reticle.canDraw(false);
	}

	/// <summary>
	/// Get's if you can switch or not.
	/// </summary>
	public bool isAbleToSwitch()
	{
		if (m_State != CameraState.Switching)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}
