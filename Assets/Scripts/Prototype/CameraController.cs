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
3/28.2014
	Increased orientation sensitivity
	Slightly optimized
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
	const float ROTATION_SENSITIVITY = 2.0f;
	const float ZOOM_SENSITIVITY = 0.5f;
	const float CAMERA_FOLLOW_SPEED = 0.15f;
	const float RANGE_TO_ENABLE_SWITCHING = 0.1f;
	const float LOOK_AT_SPEED = 0.06f;
	const float AIMING_LOOK_AT_FRONT_AMOUNT = 6.0f;
	const float AIMING_CAMERA_HEIGHT = 0.5f;

	//Looking helper variables
	public float m_ForwardAmount = 3.0f;
	Vector3 m_LastLookAtPosition = Vector3.zero;

	//Zoom
	public float m_Zoom = 0.6f;
	public float m_CloseLimit = 0.55f;

	//Aiming
	public bool m_EnabledAiming = true;
	Vector3 m_SavedLocalPosition = Vector3.zero;

	//Player movement for aiming
	PlayerMovement m_Movement;

	//Reticle
	Reticle m_Reticle;
	

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
		if (Input.GetKeyDown ("x"))
		{
			toggleAiming ( );
		}
		if (m_State == CameraState.Aiming && m_Movement != null)
		{
			m_Movement.AimMovement();
		}

		updatePosition();
		updateZoom ();
		updateOrientation ();
		updateLookPosition ();
		updateReticlePosition ();
	}
	
	// Updates the position of the camera's origin
	void updatePosition()
	{
		if ( m_State == CameraState.Switching)
		{
		    if (Vector3.Distance(this.transform.parent.transform.position, m_CameraFollow.position) < RANGE_TO_ENABLE_SWITCHING )
			{
				m_State = CameraState.Default;
			}
			else
			{
				transform.parent.position = Vector3.Lerp(this.transform.parent.position, m_CameraFollow.position , CAMERA_FOLLOW_SPEED);
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
			Vector3 aPosition = ( AIMING_LOOK_AT_FRONT_AMOUNT * Vector3.Normalize(m_CameraFollow.forward) + m_CameraFollow.position );
			transform.parent.transform.position = new Vector3 ( aPosition.x, transform.parent.transform.position.y, aPosition.z );

			//Fix bobbing effect
			transform.position = new Vector3 (m_CameraFollow.position.x, m_CameraFollow.position.y + AIMING_CAMERA_HEIGHT, m_CameraFollow.position.z);
		}
	}

	// Updates the camera's zoom
	void updateZoom()
	{
		//Make sure there is zoom input
		if (PlayerInput.Instance.getCameraMovement().y == 0 || m_State != CameraState.Default)
		{
			return;
		}
		
		//Set Camera Zoom
		setZoom(m_Zoom + -PlayerInput.Instance.getCameraMovement().y * (ZOOM_SENSITIVITY / 100.0f));
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
		this.transform.localPosition = Vector3.Lerp (Vector3.zero, maxZoomPosition, m_Zoom);
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
			if (PlayerInput.Instance.getCameraMovement().x == 0)
			{
				return;
			}
			
			//Turn
			setOrientation (transform.parent.eulerAngles.y + (ROTATION_SENSITIVITY * PlayerInput.Instance.getCameraMovement().x));
		}
		else if (m_State == CameraState.Aiming)
		{
			if (PlayerInput.Instance.getCameraMovement().y == 0)
			{
				return;
			}

			if (m_Movement)
			{
				m_Movement.AimMovement();
			}

			//Set where to look
			transform.parent.transform.position = new Vector3 (transform.parent.transform.position.x,
			                                                   transform.parent.transform.position.y + (ROTATION_SENSITIVITY/4 * PlayerInput.Instance.getCameraMovement().y),
			                                                        transform.parent.transform.position.z );
			//Fix bobbing effec
			transform.position.Set (m_CameraFollow.position.x, m_CameraFollow.position.y + AIMING_CAMERA_HEIGHT, m_CameraFollow.position.z);;
			//this.transform.position = new Vector3 (m_CameraFollow.position.x, m_CameraFollow.position.y + AIMING_CAMERA_HEIGHT, m_CameraFollow.position.z);
		}
	}

	// Sets revolution around the player
	void setOrientation(float orientation)
	{
		//Turn
		this.transform.parent.eulerAngles = new Vector3(transform.parent.eulerAngles.x, orientation, this.transform.parent.eulerAngles.z);
	}

	// Updates where to look
	void updateLookPosition ()
	{
		if ( m_State == CameraState.Aiming )
		{
			transform.LookAt (transform.parent.transform.position);
		}
		else
		{
			//Where to Look
			Vector3 positionToLookAt = Vector3.Lerp (m_LastLookAtPosition, m_CameraFollow.localPosition + (m_CameraFollow.forward * m_ForwardAmount), LOOK_AT_SPEED);

			//Look in front of object
			transform.LookAt (positionToLookAt);

			//Save the position for lerping
			 m_LastLookAtPosition = positionToLookAt;
		}
	}

	// Updates the position of the reticle
	void updateReticlePosition ()
	{
		//Check collision in front of the followed transform
		RaycastHit hit;
		if (Physics.Raycast(m_CameraFollow.position + m_CameraFollow.forward, (m_Reticle.getTargetPosition() - m_CameraFollow.position).normalized, out hit, Reticle.RETICLE_DISTANCE))
		{
			m_Reticle.setReticlePosition(hit.point);
		}

		//Camera is in aiming state
		else if (m_State == CameraState.Aiming)
		{
			m_Reticle.setReticlePosition (m_CameraFollow.position + (transform.parent.transform.position - m_CameraFollow.position).normalized * Reticle.RETICLE_DISTANCE);
		}

		//Default position of reticle
		else
		{
			m_Reticle.setReticlePosition (m_CameraFollow.position + (m_CameraFollow.forward * Reticle.RETICLE_DISTANCE));
		}

		//Set 2D paint position
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
		if ( m_State == CameraState.Switching )
		{
			return;
		}

		setToNormal ();
		m_State = CameraState.Switching;
		m_CameraFollow = newFollowedTransform;
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
		m_Movement = (PlayerMovement)m_CameraFollow.gameObject.GetComponent<PlayerMovement> ();
		if (m_Movement != null)
		{
			m_Movement.setCanMove(false);
		}

		m_SavedLocalPosition = this.transform.localPosition;
		transform.position = m_CameraFollow.position;
		transform.parent.transform.position = ( AIMING_LOOK_AT_FRONT_AMOUNT * Vector3.Normalize ( m_CameraFollow.forward ) + m_CameraFollow.position );
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
		m_Movement = null;

		transform.parent.transform.position = m_CameraFollow.position;
		transform.localPosition = m_SavedLocalPosition;
		setOrientation (m_CameraFollow.position.y);
		m_State = CameraState.Default;

		//Do not draw reticle by default
		m_Reticle.canDraw(false);
	}
}
