/*
*TPCamera
*
*resposible for camera controll / movement
*
*Created by: Kris Matis
*/

#region ChangeLog
/*
* 8/10/2014 Edit: Fully Commented - Kris Matis.
*
* 
*/
#endregion

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(AcceptInputFrom))]
public class TPCamera : MonoBehaviour
{
	const string CAMERA_IGNORE_COLLISION_LAYER = "CameraCollisionIgnore";

	//the list of camera behaviours being used
    List<Behaviour> m_Behaviours = new List<Behaviour>();
	//what the camera accepts input from 
    AcceptInputFrom m_AcceptInputFrom;

	//the players game object or an object at the players position (camera looks at this)
    public GameObject Player;

	//the point that will be rotated (should be a parent of the lerp point)
    public GameObject RotationPoint;
	//used to speed up or slow down how fast you spin the camera
    public Vector2 RotationScale;

	//th point that the camera lerps its position to
    public GameObject LerpTo;

	//how fast the lerp is
    const float LERP_AMOUNT = 0.16f;

	//used to decide if the debug rays should be drawn
    public bool DrawRays = false;
	//the offset of the rayscasts (should be ofset more than the clipping plane
    public Vector2 RaycastOffset;

	// the camera to control
    Camera m_Camera;

	//the auto lerp amount
    const float AUTO_LERP_BASE_AMOUNT = 0.08f;

	//the script that should be running on a game object at the player location (used to find action areas)
    public ActionAreaDetector ActionAreaDetect;

	// Use this for initialization
	void Start ()
    {
		//find the camera on this gameobject
        m_Camera = gameObject.GetComponent<Camera>();

		//===================================================================
		//find the player the camera is on
		//then find out if theyre player one or two
		//adjust the screen to be on the correct players side
        Characters currentCharacter;
        switch (transform.parent.name)
        {
            case Constants.ALEX_STRING:
                currentCharacter = Characters.Alex;
                break;
            case Constants.DEREK_STRING:
                currentCharacter = Characters.Derek;
                break;
            case Constants.ZOE_STRING:
                currentCharacter = Characters.Zoe;
                break;
            default:
				#if DEBUG || UNITY_EDITOR
               		Debug.LogError("parent is named wrong");
				#endif
                currentCharacter = Characters.Zoe;
                break;
        }
		/* horizontal split
        if (GameData.Instance.PlayerOneCharacter == currentCharacter)
        {
           m_Camera.rect = new Rect(0.0f, 0.5f, 1.0f, 0.5f);
        }
        else
        {
            m_Camera.rect = new Rect(0.0f, 0.0f, 1.0f, 0.5f);
        }*/

		//vertical split
		if (GameData.Instance.PlayerOneCharacter == currentCharacter)
		{
			m_Camera.rect = new Rect(0.0f, 0.0f, 0.5f, 1.0f);
		}
		else
		{
			m_Camera.rect = new Rect(0.5f, 0.0f, 0.5f, 1.0f);
		}
		//==================================================================================
        //get the accept input from script on the camera game object
		m_AcceptInputFrom = gameObject.GetComponent<AcceptInputFrom>();

		//add the desired behaviours
        m_Behaviours.Add(new Rotation2(this));
        m_Behaviours.Add(new LerpToPosition(this));
        m_Behaviours.Add(new LookAt(this));
        m_Behaviours.Add(new Collision(this));
		m_Behaviours.Add(new ActionArea (this));
	}
	
	// Update is called once per frame
	void Update ()
    {
		//move the rotation points position to the player
        RotationPoint.transform.position = Vector3.Lerp(RotationPoint.transform.position, Player.transform.position, LERP_AMOUNT);

		//update all the beahaviours
        for (int i = 0; i < m_Behaviours.Count; i++)
        {
            m_Behaviours[i].behavior();
        }
	}

    //=================================================================================================
    //=================================================================================================
    //behaviors
    #region Behaviors
	//base class for the behaviours
    protected abstract class Behaviour
    {
		//the instance of TPCamera that is using this behaviour
        protected TPCamera m_Containing;
        public Behaviour(TPCamera containing)
        {
            m_Containing = containing;
        }
        public virtual void behavior()
        {
        }        
    }

    //=================================================================================================

	//behaviour for action areas
    protected class ActionArea : Behaviour
    {
        public ActionArea(TPCamera containing)
            : base(containing)
        {
        }

        public override void behavior()
        {
			//checks if the player is in an action area
			if(m_Containing.ActionAreaDetect.m_CurrentActionArea != null)
			{
				//rotates to match the action area camera mount points rotation
                m_Containing.transform.forward = Helpers.lerpVector3(m_Containing.transform.forward, m_Containing.ActionAreaDetect.m_CurrentActionArea.CameraMountPoint.transform.forward, AUTO_LERP_BASE_AMOUNT * 2.0f);

				//moves to the action area camera mount points position
                m_Containing.gameObject.transform.position = Helpers.lerpVector3(m_Containing.transform.position, m_Containing.ActionAreaDetect.m_CurrentActionArea.CameraMountPoint.transform.position, LERP_AMOUNT / 2.0f);
            }
		}
    }

    //=================================================================================================

	//behaviour that handles auto rotation and player rotation of the camera
    protected class Rotation : Behaviour
    {
        public Rotation(TPCamera containing)
            : base(containing)
        {
        }

        public override void behavior()
        {
			//return if an action area has been found
			if (m_Containing.ActionAreaDetect.m_CurrentActionArea != null)
			{
				return;
			}

            //get the input for camera Rotation
            Vector2 rotationInput = InputManager.getCamera(m_Containing.m_AcceptInputFrom.ReadInputFrom);

            //rotate in the y to move the camera horizontally, since the camera needs to be on a set horizontal axis this is done in world space
            m_Containing.RotationPoint.transform.Rotate(0.0f, -rotationInput.x * m_Containing.RotationScale.x, 0.0f, Space.World);

            //rotate the camera up and down since were spinning inb the y this needs to be done in local space
            m_Containing.RotationPoint.transform.Rotate(rotationInput.y * m_Containing.RotationScale.y, 0.0f, 0.0f, Space.Self);

            //get the current euler angles
            Vector3 eulerangles = m_Containing.RotationPoint.transform.rotation.eulerAngles;

            //since the we need the angle to stay between 0 - 75 and 295-360 we need two different clamps
            if(eulerangles.x <= 180)
            {
                eulerangles.x = Mathf.Clamp(eulerangles.x, 0.0f, 75.0f);
            }
            else
            {
                eulerangles.x = Mathf.Clamp(eulerangles.x, 295.0f, 360.0f);
            }
            //the z axis slowly collects tiny amounts of rotation (might be rounding) but we reset it to 0.0 since were never rotating in the z
            eulerangles.z = 0.0f;

            //reset the rotation with the clamped values
            m_Containing.RotationPoint.transform.rotation = Quaternion.Euler(eulerangles);
        }
    }

	// second behaviour for rotation (note: only use one rotation beaviour)
	protected class Rotation2 : Behaviour
	{
		public Rotation2(TPCamera containing)
			: base(containing)
		{
		}
		
		public override void behavior()
		{
			//return idf an action area has been found
			if (m_Containing.ActionAreaDetect.m_CurrentActionArea != null)
			{
				return;
			}

			//get the input for camera Rotation
			Vector2 rotationInput = InputManager.getCamera(m_Containing.m_AcceptInputFrom.ReadInputFrom);

			//if there was input
			if(rotationInput.magnitude != 0.0f)
			{
				//rotate in the y to move the camera horizontally, since the camera needs to be on a set horizontal axis this is done in world space
				m_Containing.RotationPoint.transform.Rotate(0.0f, -rotationInput.x * m_Containing.RotationScale.x, 0.0f, Space.World);
				
				//rotate the camera up and down since were spinning inb the y this needs to be done in local space
				m_Containing.RotationPoint.transform.Rotate(rotationInput.y * m_Containing.RotationScale.y, 0.0f, 0.0f, Space.Self);
				
				//get the current euler angles
				Vector3 eulerangles = m_Containing.RotationPoint.transform.rotation.eulerAngles;
				
				//since the we need the angle to stay between 0 - 75 and 295-360 we need two different clamps
				if(eulerangles.x <= 180)
				{
					eulerangles.x = Mathf.Clamp(eulerangles.x, 0.0f, 75.0f);
				}
				else
				{
					eulerangles.x = Mathf.Clamp(eulerangles.x, 295.0f, 360.0f);
				}
				//the z axis slowly collects tiny amounts of rotation (might be rounding) but we reset it to 0.0 since were never rotating in the z
				eulerangles.z = 0.0f;
				
				//reset the rotation with the clamped values
				m_Containing.RotationPoint.transform.rotation = Quaternion.Euler(eulerangles);
			}
			else
			{
				//there was no input,
				//if the player is moving
				if(InputManager.getMove(m_Containing.m_AcceptInputFrom.ReadInputFrom).magnitude != 0.0f)
				{
					//get the camera rotaion point euler angles
                    Vector3 currentEulerAngles = m_Containing.RotationPoint.transform.rotation.eulerAngles;
					//get the rotation of the "player" object
                    Vector3 targetEulerAngles = m_Containing.Player.transform.rotation.eulerAngles;

					//come up with a percentage of how similar the angles are
                    float percentLerp = 1.0f - (Mathf.Abs(currentEulerAngles.y - targetEulerAngles.y) / 180.0f);
					//square it
                    percentLerp *= percentLerp;
					//lerp the camera rotation points rotation to the players rotation 
                    m_Containing.RotationPoint.transform.rotation = Quaternion.Slerp(m_Containing.RotationPoint.transform.rotation, Quaternion.Euler(currentEulerAngles.x, targetEulerAngles.y, targetEulerAngles.z), AUTO_LERP_BASE_AMOUNT * percentLerp);
				
					//end result of this if is the camera lerps behind you while you move
					//and it will lerp faster if its closer to being behind you
				}
			}
		}
	}

    //=================================================================================================

	//lerp the cameras position
    protected class LerpToPosition : Behaviour
    {
        public LerpToPosition(TPCamera containing)
            : base(containing)
        {
        }

        public override void behavior()
        {
			//if an action area has been found return
			if (m_Containing.ActionAreaDetect.m_CurrentActionArea != null)
			{
				return;
			}
            //lerp to the target position
            m_Containing.gameObject.transform.position = Vector3.Lerp(m_Containing.transform.position, m_Containing.LerpTo.transform.position, LERP_AMOUNT);
        }
    }

    //=================================================================================================

	//the look at behaviour
    protected class LookAt : Behaviour
    {
        public LookAt(TPCamera containing)
            : base(containing)
        {
        }

        public override void behavior()
        {
			//if an action area has been found return
			if (m_Containing.ActionAreaDetect.m_CurrentActionArea != null)
			{
				return;
			}

            //for now just look at the rotaion point
            m_Containing.transform.LookAt(m_Containing.RotationPoint.transform.position);
        }
    }

    //=================================================================================================

	//behaviour for camera collision
    protected class Collision : Behaviour
    {
        public Collision(TPCamera containing)
            : base(containing)
        {
        }

        public override void behavior()
        {
			//if an action area has been found return
			if (m_Containing.ActionAreaDetect.m_CurrentActionArea != null)
			{
				return;
			}

			//raycast four times to areas around the cameras point based off the rotation of the camera and the offset
			//each function returns the minimum distance (whether or not its ray cast hit an object first
            float minDist = float.MaxValue;
            minDist = raycast(minDist, m_Containing.transform.right *  m_Containing.RaycastOffset.x,  m_Containing.transform.up * m_Containing.RaycastOffset.y);
            minDist = raycast(minDist, m_Containing.transform.right * -m_Containing.RaycastOffset.x,  m_Containing.transform.up * m_Containing.RaycastOffset.y);
            minDist = raycast(minDist, m_Containing.transform.right *  m_Containing.RaycastOffset.x, -m_Containing.transform.up * m_Containing.RaycastOffset.y);
            minDist = raycast(minDist, m_Containing.transform.right * -m_Containing.RaycastOffset.x, -m_Containing.transform.up * m_Containing.RaycastOffset.y);

			//if none of the ray casts hit anything then the min dist is still the max value
            if (minDist == float.MaxValue)
            {
                minDist = (m_Containing.transform.position - m_Containing.Player.transform.position).magnitude;
            }
			//get the direction to move the camera
            Vector3 direction = m_Containing.transform.position - m_Containing.Player.transform.position;

            direction.Normalize();

			//do we need to draw debug rays?
            if (m_Containing.DrawRays)
            {
				#if DEBUG || UNITY_EDITOR
                	Debug.DrawRay(m_Containing.Player.transform.position, direction * minDist, Color.black);
				#endif
            }

			//if min dist is less that the cameras current distance from the player we need to move the camera
            if (minDist < (m_Containing.transform.position - m_Containing.Player.transform.position).magnitude)
            {
                m_Containing.transform.position = m_Containing.Player.transform.position + (direction * minDist);
            }
        }

		//returnsthe closer distance between the raycast hit and the value passed in relative to the player
        float raycast(float currentMinDist, Vector3 offsetX, Vector3 offsetY)
        {
			//calulate the direction to raycast in
            Vector3 RayDirection = (m_Containing.transform.position + offsetX + offsetY) - m_Containing.Player.transform.position;
            float raycastDistance;

			//calculate the distance we need to raycast
            if ((m_Containing.LerpTo.transform.position - m_Containing.Player.transform.position).magnitude > (m_Containing.transform.position - m_Containing.Player.transform.position).magnitude)
            {
                raycastDistance = (m_Containing.LerpTo.transform.position - m_Containing.Player.transform.position).magnitude;
            }
            else
            {
                raycastDistance = (m_Containing.transform.position - m_Containing.Player.transform.position).magnitude;
            }

			//do the raycast
            RaycastHit raycastInfo;
            Physics.Raycast(m_Containing.Player.transform.position, RayDirection, out raycastInfo, raycastDistance, ~(LayerMask.GetMask(Constants.PLAYER_STRING) | LayerMask.GetMask(CAMERA_IGNORE_COLLISION_LAYER)));

			//do we need to draw debug rays
            if (m_Containing.DrawRays)
            {
				#if DEBUG || UNITY_EDITOR
	                if (raycastInfo.collider != null)
	                {
	                    Debug.DrawRay(m_Containing.Player.transform.position, raycastInfo.point - m_Containing.Player.transform.position, Color.cyan);
	                }
	                else
	                {
	                    Debug.DrawRay(m_Containing.Player.transform.position, m_Containing.transform.position + offsetX + offsetY - m_Containing.Player.transform.position, Color.blue);
	                }
				#endif
            }

			//if we didnt hit anything
            if (raycastInfo.collider == null)
            {
                return currentMinDist;
            }

			//de we return the old min distance or ours (which is smaller)
            if (currentMinDist <= (raycastInfo.point - m_Containing.Player.transform.position).magnitude)
            {
                return currentMinDist;
            }
            else
            {
                return (raycastInfo.point - m_Containing.Player.transform.position).magnitude;
            }
        }
    }

    //=================================================================================================
    #endregion
}
