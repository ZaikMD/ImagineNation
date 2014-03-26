﻿/*TO USE

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
*/





using UnityEngine;
using System.Collections;

/// <summary>
/// Enables camera movement based on input, and the objects transform.
/// </summary>
[RequireComponent (typeof(Camera))]
public class CameraController : MonoBehaviour {

	//State
	enum CameraState
	{
		Default = 0,
		Aiming,
		Switching
	}
	CameraState m_State = CameraState.Default;


	//Sensitivity
	const float ROTATION_SENSITIVITY = 1.0f;
	const float ZOOM_SENSITIVITY = 0.5f;
	const float CAMERA_FOLLOW_SPEED = 0.15f;
	const float RANGE_TO_ENABLE_SWITCHING = 0.1f;
	const float LOOK_AT_SPEED = 0.10f;
	const float AIMING_LOOK_AT_FRONT_AMOUNT = 5.0f;
	const float AIMING_CAMERA_HEIGHT = 0.5f;

	//What to follow
	Transform m_CameraFollow;

	//Looking helper variables
	public float m_ForwardAmount = 5.0f;
	Vector3 m_LastLookAtPosition = Vector3.zero;

	//Zoom
	public float m_Zoom = 0.5f;
	public float m_CloseLimit = 0.3f;

	//Aiming
	public bool m_EnabledAiming = true;
	Vector3 m_SavedLocalPosition = Vector3.zero;



	// Use this for initialization
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
		m_CameraFollow = this.transform.parent.transform;

		//Create an origin point
		GameObject origin = new GameObject ();
		origin.transform.position = this.transform.parent.transform.position;

		//Set us to follow the transform we were a parent of
		this.transform.parent = origin.transform;
	}
	
	// Update is called once per frame
	void Update ()
	{
		updatePosition();
		updateZoom ();
		updateOrientation ();
		updateLookPosition ();
	}
	
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
				this.transform.parent.position = Vector3.Lerp(this.transform.parent.position, m_CameraFollow.position , CAMERA_FOLLOW_SPEED);
			}
		}
		else if ( m_State == CameraState.Default )
		{
			//We follow the object's position, but not their rotation
			this.transform.parent.position = m_CameraFollow.position;
		}
		else if ( m_State == CameraState.Aiming )
		{
			//Where to Look At
			Vector3 aPosition = ( AIMING_LOOK_AT_FRONT_AMOUNT * Vector3.Normalize(m_CameraFollow.forward) + m_CameraFollow.position );
			this.transform.parent.transform.position = new Vector3 ( aPosition.x, this.transform.parent.transform.position.y, aPosition.z );

			//Fix bobbing effect
			this.transform.position = new Vector3 (m_CameraFollow.position.x, m_CameraFollow.position.y + AIMING_CAMERA_HEIGHT, m_CameraFollow.position.z);
		}
	}

	void updateZoom()
	{
		//Make sure there is zoom input
		if (Mathf.Abs (Input.GetAxis ("Mouse Y")) == 0 || m_State != CameraState.Default)
		{
			return;
		}
		
		//Set Camera Zoom
		setZoom(m_Zoom + -Input.GetAxis ("Mouse Y") * (ZOOM_SENSITIVITY / 100.0f));
	}
	
	void setZoom(float aZoom)
	{
		//Get max zoom position for lerping
		Vector3 maxZoomPosition = this.transform.localPosition * (1 / m_Zoom);
		
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

	void updateOrientation()
	{
		//Only turn Camera if there is input
		if (m_State == CameraState.Switching)
		{
			return;
		}
		else if (m_State == CameraState.Default)
		{
			if (Mathf.Abs (Input.GetAxis ("Mouse X")) == 0)
			{
				return;
			}
			
			//Turn
			setOrientation (this.transform.parent.eulerAngles.y + (ROTATION_SENSITIVITY * Input.GetAxis ("Mouse X")));
		}
		else if (m_State == CameraState.Aiming)
		{
			if ( Mathf.Abs (Input.GetAxis ("Mouse Y")) == 0)
			{
				return;
			}

			//Set where to look
			this.transform.parent.transform.position = new Vector3 (this.transform.parent.transform.position.x,
			                                                        this.transform.parent.transform.position.y + (ROTATION_SENSITIVITY/4 * Input.GetAxis ("Mouse Y")),
			                                                        this.transform.parent.transform.position.z );
			//Fix bobbing effect
			this.transform.position.Set (m_CameraFollow.position.x, m_CameraFollow.position.y + AIMING_CAMERA_HEIGHT, m_CameraFollow.position.z);;
			//this.transform.position = new Vector3 (m_CameraFollow.position.x, m_CameraFollow.position.y + AIMING_CAMERA_HEIGHT, m_CameraFollow.position.z);
		}
	}

	void setOrientation(float orientation)
	{
		//Turn
		this.transform.parent.eulerAngles = new Vector3(this.transform.parent.eulerAngles.x, orientation, this.transform.parent.eulerAngles.z);
	}

	void updateLookPosition ()
	{
		if ( m_State == CameraState.Aiming )
		{
			this.transform.LookAt (this.transform.parent.transform.position);
		}
		else
		{
			//Get Position to Look at (Currently in front of followed object)
			Vector3 objectForward = Vector3.Normalize(m_CameraFollow.forward);
			Vector3 originPosition = m_CameraFollow.localPosition;

			//Where to Look
			Vector3 positionToLookAt = Vector3.Lerp (m_LastLookAtPosition, originPosition + (objectForward * m_ForwardAmount), LOOK_AT_SPEED);

			//Look in front of object
			this.transform.LookAt (positionToLookAt);

			//Save the position for lerping
			 m_LastLookAtPosition = positionToLookAt;
		}
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

		m_SavedLocalPosition = this.transform.localPosition;
		this.transform.position = m_CameraFollow.position;
		this.transform.parent.transform.position = ( AIMING_LOOK_AT_FRONT_AMOUNT * Vector3.Normalize ( m_CameraFollow.forward ) + m_CameraFollow.position );
		m_State = CameraState.Aiming;
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

		this.transform.parent.transform.position = m_CameraFollow.position;
		this.transform.localPosition = m_SavedLocalPosition;
		setOrientation (m_CameraFollow.position.y);
		m_State = CameraState.Default;
	}
}
