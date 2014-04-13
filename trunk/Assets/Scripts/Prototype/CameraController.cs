/*TO USE

Attach this component to the camera.

Make this camera a child of the object you want to follow.

Set this camera's local position to where you'd like it in relation to the object you want to follow.





Created by Jason "The Casual" Hein on 3/1/2014


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
4/6/2014
	Smoothed collision with walls
	No longer needs a collider or rigid body
4/8/2014
	Smoothed camera collsiion
	Reduced collision sensitivity
	Removed mouse cursor
4/9/2014
	Now ignores single thin objects
4/11/2014
	Reticle now turns red over interactables
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
	const float RANGE_TO_ENABLE_SWITCHING = 1.0f;
	const float LOOK_AT_SPEED = 0.04f;
	const float AIMING_LOOK_AT_FRONT_AMOUNT = 20.0f;
	const float AIMING_CAMERA_HEIGHT = 0.3f;
	const float AIMING_VERTICAL_SENSITIVITY = 13.0f;

	//Looking helper variables
	const float FORWARD_AMOUNT = 2.0f;
	Vector3 m_LastLookAtPosition = Vector3.zero;

	//Zoom
	public float m_Zoom = 0.65f;
	float m_CloseLimit = 0.10f;
	const float DEFAULT_ZOOM = 0.65f;
	const float BACK_ZOOM = 0.8f;
	float m_Zoom_Return = DEFAULT_ZOOM;
	const float TUNNEL_ZOOM = 0.25f;
	const float ZOOM_RETURN_SPEED = 0.02f;

	//Aiming
	public bool m_EnabledAiming = true;
	Vector3 m_SavedLocalPosition = Vector3.zero;

	//Player movement for aiming
	PlayerMovement m_Movement;

	//Movement after Collision
	float m_CollisionOrientation = 0.0f;
	float m_CollisionTimer = 0.0f;
	float m_Zoom_Collision = 0.0f;
	bool m_CollisionZoom = false;

	//Reticle
	Reticle m_Reticle;

	//Clipping in tunnels
	float m_DefaultClippingPlane = 0.0f;
	float m_ClippingTimer = 0.0f;
	const float CLIPPING_TIMER = 0.4f;
	const float TUNNEL_CLIPPING = 2.0f;

	//Collision
	const float COLLISION_FIX_TIMER = 4.0f;
	const float COLLISION_TURN_SPEED = 0.1f;
	const float COLLISION_ZOOM_SPEED = 0.1f;
	const float COLLISION_CHECK_RANGE = 4.0f;
	const float COLLISION_TURN_AMOUNT = 1.0f;
	const float COLLISION_MAX_THINNESS_OF_OBJECT = 2.0f;



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

		//Remove mouse cursor
		Screen.showCursor = false;

		//Near clipping
		m_DefaultClippingPlane = camera.nearClipPlane;

		//Set initial Transform to follow
		m_CameraFollow = transform.parent.transform;
		m_Movement = (PlayerMovement)m_CameraFollow.gameObject.GetComponent<PlayerMovement> ();

		//Create an origin point
		GameObject origin = new GameObject ();
		origin.transform.position = transform.parent.transform.position;

		//Set us to follow the transform we were a parent of
		transform.parent = origin.transform;

		//setting the origins tag
		origin.tag = "Camera";

		//Load Reticle
		GameObject reticle = (GameObject)Instantiate(Resources.Load("Reticle"), m_CameraFollow.position + m_CameraFollow.forward, Quaternion.identity);
		m_Reticle = (Reticle)(reticle.GetComponent<Reticle>());
		m_Reticle.Load();
	}
	
	// Update
	void Update ()
	{
		updateCollisionTimer ();
		updateCameraCollision ();
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

		//Tunel Clipping Plane Timer
		if (m_ClippingTimer > 0.0f)
		{
			m_ClippingTimer -= Time.deltaTime;

			if (m_ClippingTimer <= 0.0f)
			{
				camera.nearClipPlane = m_DefaultClippingPlane;
			}
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
			if (PlayerInput.Instance.getCameraMovement().y != 0)
			{
				//Set Camera Zoom
				setZoom(m_Zoom + -PlayerInput.Instance.getCameraMovement().y * (ZOOM_SENSITIVITY / 100.0f));
				m_Zoom_Return = DEFAULT_ZOOM;
				m_CollisionTimer = 0.0f;
			}
			else if (m_CollisionTimer > 0.0f && m_CollisionZoom)
			{
				//Set Camera Zoom
				setZoom(Mathf.Lerp(m_Zoom , m_Zoom_Collision, COLLISION_ZOOM_SPEED));
				m_Zoom_Return = DEFAULT_ZOOM;
			}
			else if (m_Zoom != m_Zoom_Return && PlayerInput.Instance.getMovementInput() != Vector2.zero)
			{
				if (PlayerInput.Instance.getMovementInput().y < 0.0f && m_Zoom_Return != BACK_ZOOM)
				{
					m_Zoom_Return = BACK_ZOOM;
				}
				else if (m_Zoom_Return != DEFAULT_ZOOM)
				{
					m_Zoom_Return = DEFAULT_ZOOM;
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

	//Makes the camera collide with walls.
	void updateCameraCollision()
	{
		if (m_State != CameraState.Default)
		{
			return;
		}
		
		//By default do not zoom in or out from collision
		m_CollisionZoom = false;

		//Hit variable for getting raycast information
		RaycastHit hit;


		//Back Right for slight turning
		if (Physics.Raycast(transform.position, -transform.forward + transform.right, out hit, COLLISION_CHECK_RANGE) ||
		    Physics.Raycast(transform.position, transform.right, out hit, COLLISION_CHECK_RANGE))
		{
			m_CollisionOrientation = transform.parent.eulerAngles.y + COLLISION_TURN_AMOUNT;
			m_CollisionTimer = COLLISION_FIX_TIMER;
		}
		//Back Left for slight turning
		else if (Physics.Raycast(transform.position, -transform.forward - transform.right, out hit, COLLISION_CHECK_RANGE) || 
		    Physics.Raycast(transform.position, -transform.right, out hit, COLLISION_CHECK_RANGE))
		{
			m_CollisionOrientation = transform.parent.eulerAngles.y - COLLISION_TURN_AMOUNT;
			m_CollisionTimer = COLLISION_FIX_TIMER;
		}
		//Check if their is a wall in the wayS
		if (Physics.Raycast(m_CameraFollow.position, (transform.position - m_CameraFollow.position).normalized, out hit, Vector3.Distance(transform.position, m_CameraFollow.position)))
		{
			//Check if the object hit is thin, and should be ignored
			RaycastHit hit2;
			Physics.Raycast(transform.position, (m_CameraFollow.position - transform.position).normalized, out hit2, Vector3.Distance(transform.position, m_CameraFollow.position));
			if (Vector3.Distance(hit.point, hit2.point) < COLLISION_MAX_THINNESS_OF_OBJECT)
			{
				return;
			}


			m_Zoom_Collision = (m_Zoom / Vector3.Distance(transform.localPosition + (transform.position - m_CameraFollow.position).normalized, Vector3.zero)) *
				Vector3.Distance(m_CameraFollow.position, hit.point);

			if (m_Zoom_Collision < TUNNEL_ZOOM)
			{
				camera.nearClipPlane = TUNNEL_CLIPPING;
				m_ClippingTimer = CLIPPING_TIMER;
			}

			m_CollisionZoom = true;
			m_CollisionTimer = COLLISION_FIX_TIMER;
		}
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
			if (PlayerInput.Instance.getCameraMovement().x != 0)
			{
				setOrientation (transform.parent.eulerAngles.y + (ROTATION_SENSITIVITY * PlayerInput.Instance.getCameraMovement().x));
				m_CollisionTimer = 0.0f;
			}
			else if (m_CollisionTimer > 0.0f)
			{
				setOrientation (Mathf.Lerp(transform.parent.eulerAngles.y, m_CollisionOrientation, COLLISION_TURN_SPEED));
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
			else if (hit.transform.gameObject.CompareTag("Enemy") || hit.transform.gameObject.GetComponent<InteractableBaseClass>() != null || 
				    (m_CameraFollow.gameObject.name == "Zoey" && (hit.transform.gameObject.CompareTag("Glass"))) || 
			        (m_CameraFollow.gameObject.name == "Derek" && (hit.transform.gameObject.CompareTag("VelcroWall") || hit.transform.gameObject.CompareTag("DestructibleWall"))) ||
			        (m_CameraFollow.gameObject.name == "Alex" && (hit.transform.gameObject.CompareTag("NerfWall") || hit.transform.gameObject.CompareTag("NerfTarget"))))
			{
				m_Reticle.SetIsOnSomething(true);
			}
			else
			{
				m_Reticle.SetIsOnSomething(false);
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
			m_Movement.setCanMove(m_CameraFollow.gameObject.GetComponent<PlayerState>().m_IsActive);
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
		if (m_State == CameraState.Default)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}
