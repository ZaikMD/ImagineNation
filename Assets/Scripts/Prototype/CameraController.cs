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
	const float AIMING_CAMERA_HEIGHT = 0.5f;
	const float ZOOM_RETURN_SPEED = 0.01f;

	//Looking helper variables
	const float FORWARD_AMOUNT = 2.0f;
	Vector3 m_LastLookAtPosition = Vector3.zero;

	//Zoom
	float m_Zoom = 0.6f;
	float m_CloseLimit = 0.55f;
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

	bool m_CanFlip = true;
	

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
		if (PlayerInput.Instance.getIsAiming())
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
		updateReticle2DPosition ();
	}
	
	// Updates the position of the camera's origin
	void updatePosition()
	{
		if ( m_State == CameraState.Switching)
		{
		    if (Vector3.Distance(this.transform.parent.position, m_CameraFollow.position) < RANGE_TO_ENABLE_SWITCHING )
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
			Vector3 aPosition = ( AIMING_LOOK_AT_FRONT_AMOUNT * Vector3.Normalize(m_CameraFollow.forward) + m_CameraFollow.position );
			transform.parent.position = new Vector3 ( aPosition.x, transform.parent.position.y, aPosition.z );

			//Fix bobbing effect
			transform.position = new Vector3 (m_CameraFollow.position.x, m_CameraFollow.position.y + AIMING_CAMERA_HEIGHT, m_CameraFollow.position.z);
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
			setOrientation (transform.parent.eulerAngles.y + (ROTATION_SENSITIVITY * PlayerInput.Instance.getCameraMovement().x));
			/*//Turn to face player
			if (PlayerInput.Instance.getCameraMovement().x == 0 && (PlayerInput.Instance.getMovementInput().x != 0.0f || PlayerInput.Instance.getMovementInput().y < 0.0f))
			{


				if (PlayerInput.Instance.getMovementInput().y < 0.0f && PlayerInput.Instance.getMovementInput().x == 0.0f)
				{
					if (m_CanFlip)
					{
						setOrientation (m_CameraFollow.rotation.eulerAngles.y);
						m_CanFlip = false;
						return;
					}
				}
				else
				{
					m_CanFlip = true;
				}
				Vector3 angle = Vector3.Lerp(transform.parent.forward, m_Movement.getControllerProjection(),0.01f);
				transform.parent.LookAt(transform.parent.position + angle);





				//transform.parent.transform.Rotate(Vector3.Lerp( m_CameraFollow.transform.rotation.eulerAngles, transform.parent.rotation.eulerAngles, 0.01f));

				/*float currentAngle = m_CameraFollow.transform.rotation.eulerAngles.y + 360.0f;
				float desiredAngle = transform.parent.transform.rotation.eulerAngles.y + 360.0f;
				float angle = desiredAngle - currentAngle;
				angle -= 360.0f;

				if (Mathf.Abs (angle) > 180.0f)
				{
					angle = -angle;
				}
				transform.parent.transform.Rotate(new Vector3 (0.0f, angle / 0.01f * Time.deltaTime, 0.0f));
				//float angle = Mathf.Lerp(transform.parent.eulerAngles.y, transform.parent.eulerAngles.y + angle, 0.02f))
			}
			else
			{
				//Turn by camera rotation input
				if (!m_CanFlip)
				{
					m_CanFlip = true;
				}
				setOrientation (transform.parent.eulerAngles.y + (ROTATION_SENSITIVITY * PlayerInput.Instance.getCameraMovement().x));
			}*/
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
			transform.parent.transform.position = new Vector3 (transform.parent.position.x,
			                                                   transform.parent.position.y + (ROTATION_SENSITIVITY/4 * PlayerInput.Instance.getCameraMovement().y),
			                                                   transform.parent.position.z );
			//Fix bobbing effec
			transform.position.Set (m_CameraFollow.position.x, m_CameraFollow.position.y + AIMING_CAMERA_HEIGHT, m_CameraFollow.position.z);;
			//this.transform.position = new Vector3 (m_CameraFollow.position.x, m_CameraFollow.position.y + AIMING_CAMERA_HEIGHT, m_CameraFollow.position.z);
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
			transform.LookAt (transform.parent.transform.position);
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
			if (Physics.Raycast(m_CameraFollow.position + m_CameraFollow.forward, (transform.parent.position - m_CameraFollow.position).normalized, out hit, Reticle.RETICLE_DISTANCE))
			{
				m_Reticle.setReticlePosition(hit.point);
				return;
			}

			m_Reticle.setReticlePosition (m_CameraFollow.position + (transform.parent.position - m_CameraFollow.position).normalized * Reticle.RETICLE_DISTANCE);
		}

		//Default position of reticle
		else
		{
			if (Physics.Raycast(m_CameraFollow.position + m_CameraFollow.forward, m_CameraFollow.forward, out hit, Reticle.RETICLE_DISTANCE))
			{
				m_Reticle.setReticlePosition(hit.point);
				return;
			}

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
		if ( m_State == CameraState.Switching )
		{
			return;
		}

		setToNormal ();
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

		m_SavedLocalPosition = this.transform.localPosition;
		transform.position = m_CameraFollow.position;
		transform.parent.position = ( AIMING_LOOK_AT_FRONT_AMOUNT * Vector3.Normalize ( m_CameraFollow.forward ) + m_CameraFollow.position );
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
}
