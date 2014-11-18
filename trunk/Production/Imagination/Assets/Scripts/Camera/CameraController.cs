using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Camera))]
public class CameraController : MonoBehaviour {
	
	//What transform to follow
	public Transform m_FollowedTransform;
	
	//Variables that can be set by designers

	//Sensitivty of camera
	float MinimumZoom = 0.2f;
	float LookForawrdAmount = 5.0f;
	float OrientationThreshold = 0.4f;
	float OrientationControlSpeed = 225.0f;
	float ZoomControlSpeed = 0.5f;
	
	//Speeds for lerping
	float PositionLerpSpeed = 6.0f;
	float OrientationLerpSpeed = 0.3f;
	float LookLerpSpeed = 0.6f;
	float ZoomLerpSpeed = 0.7f;

	//Zoom related default values
	float DefaultZoom = 0.4f;
	float BackwardsZoom = 0.95f;

	
	//Offset of the camera from the players positon
	Vector3 m_CameraOffset = Vector3.zero;

	//Offset of the position the camera looks at from the players position (generally in front of the player)
	Vector3 m_LookAtOffset = Vector3.zero;

	//The zoom of the camera
	float m_Zoom;

	//Zoom of the camera to lerp to
	float m_DesiredZoom;

	//Whether or not we can control the camera
	bool m_PlayerIsGrounded;


	//Initialization
	void Start ()
	{
		transform.parent = null;
		m_CameraOffset = transform.position - m_FollowedTransform.position;
		m_Zoom = DefaultZoom;
		m_DesiredZoom = m_Zoom;
		Screen.showCursor = false;
	}
	
	// Update the camera
	void Update ()
	{
		updatePosition ();
		updateZoom ();
		updateOrientation ();
		updateLook ();
	}

	// Set the position of the camera
	public void setPosition(Vector3 aPosition)
	{
		transform.position = aPosition;
		m_CameraOffset = transform.position - m_FollowedTransform.position;
	}

	// Sets the zoom of the camera, and offsets the camera based off of that zoom
	public void setZoom(float AZoom)
	{
		if (AZoom < MinimumZoom)
		{
			AZoom = MinimumZoom;
		}
		else if (AZoom > 1)
		{
			AZoom = 1;
		}

		m_CameraOffset = (m_CameraOffset * (1 / m_Zoom)) * AZoom;
		m_Zoom = AZoom;
	}

	// Rotates the camera around the player, provided a euler roation on the y axis
	public void setOrientation (float aOrientation)
	{
		m_CameraOffset = Quaternion.Euler (0, aOrientation, 0) * m_CameraOffset;
	}

	// Sets where the camera should look, and then has the camera look there
	public void setLookAt(Vector3 lookAt)
	{
		transform.LookAt (lookAt);
	}
	
	// Updates the poisition of the camera to follow the players position
	void updatePosition()
	{
		if (Vector3.Distance(transform.position, m_FollowedTransform.position + m_CameraOffset) > 1.0f)
		{
			transform.position = Vector3.Lerp (transform.position, m_FollowedTransform.position + m_CameraOffset, PositionLerpSpeed * Time.deltaTime);
		}
	}

	// Updates the zoom of the camera based off of if the player is airborne
	void updateZoom()
	{
		//Hit to keep track of raycast collisions
		RaycastHit hit;

		if (m_PlayerIsGrounded && Mathf.Abs(Input.GetAxis("Mouse Y")) > 0.1f)
		{
			setZoom(m_Zoom - Input.GetAxis("Mouse Y") * Time.deltaTime * ZoomControlSpeed);
			return;
		}
		
		//Handle the closest object being in the way
		if (Physics.Raycast(m_FollowedTransform.position, m_CameraOffset, out hit, m_CameraOffset.magnitude))
		{
			//The new zoom we want is in front of that object
			m_DesiredZoom = Vector3.Distance(m_FollowedTransform.position, hit.point) / m_CameraOffset.magnitude;
		}
		else if (Vector3.Dot (transform.forward, m_FollowedTransform.forward) > 0.0f && m_DesiredZoom != DefaultZoom)
		{
			m_DesiredZoom = DefaultZoom;
		}
		else if (m_DesiredZoom != BackwardsZoom)
		{
			m_DesiredZoom = BackwardsZoom;
		}
		//If we are not close enough to the desired zoom
		if (Mathf.Abs(m_Zoom - m_DesiredZoom) > 0.001f)
		{
			setZoom(Mathf.Lerp(m_Zoom, m_DesiredZoom, ZoomLerpSpeed * Time.deltaTime));
		}
	}

	//Updates the camera to rotate to behind the player
	void updateOrientation()
	{
		//If we are allowed to control the camera
		if (m_PlayerIsGrounded && (Input.GetAxis("Vertical") != 0.0f || Input.GetAxis("Horizontal") != 0.0f || Mathf.Abs(Input.GetAxis("Mouse X")) > 0.1f))
		{
			setOrientation (Input.GetAxis("Mouse X") * OrientationControlSpeed * Time.deltaTime);
			return;
		}
		//Calculate the angle between where the player is facing, and where the camera is facing
		float dotOfOrientationVectors = Vector3.Dot (transform.forward, m_FollowedTransform.forward);

		//Calculate amount to rotate
		float rotation = Quaternion.FromToRotation(transform.forward, m_FollowedTransform.forward).eulerAngles.y;
		if (rotation > 180.0f)
		{
			rotation -= 360.0f;
		}

		if (dotOfOrientationVectors < OrientationThreshold)
		{
			//Rotate to face the new direction
			setOrientation (rotation * OrientationLerpSpeed * Time.deltaTime);
		}
	}

	//Lerp towards looking in front of the player
	void updateLook()
	{
		//Calculate the positions to lerp from
		Vector3 lastLookPosition = m_LookAtOffset;
		m_LookAtOffset = Vector3.Lerp (lastLookPosition, m_FollowedTransform.forward * LookForawrdAmount * m_Zoom, LookLerpSpeed * Time.deltaTime);

		setLookAt(m_FollowedTransform.position + m_LookAtOffset);
	}

	//Allows player movement to set whether or not camera controls are in use
	public void setPlayerGrounded(bool grounded)
	{
		m_PlayerIsGrounded = grounded;
	}
}
