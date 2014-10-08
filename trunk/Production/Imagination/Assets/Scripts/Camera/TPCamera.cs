using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(AcceptInputFrom))]
public class TPCamera : MonoBehaviour
{
	const string CAMERA_IGNORE_COLLISION_LAYER = "CameraCollisionIgnore";


    List<Behaviour> m_Behaviours = new List<Behaviour>();
    AcceptInputFrom m_AcceptInputFrom;

    public GameObject Player;

    public GameObject RotationPoint;
    public Vector2 RotationScale;

    public GameObject LerpTo;
    const float LERP_AMOUNT = 0.16f;

    public bool DrawRays = false;
    public Vector2 RaycastOffset;

    Camera m_Camera;

    const float AUTO_LERP_BASE_AMOUNT = 0.08f;
    const float AUTO_LERP_DEAD_ZONE_ANGLE_DEGREEES = 40.0f;

    public ActionAreaDetector ActionAreaDetect;

	// Use this for initialization
	void Start ()
    {
        m_Camera = gameObject.GetComponent<Camera>();

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
                Debug.LogError("parent is named wrong");
                currentCharacter = Characters.Zoe;
                break;
        }
		/*
        if (GameData.Instance.PlayerOneCharacter == currentCharacter)
        {
           m_Camera.rect = new Rect(0.0f, 0.5f, 1.0f, 0.5f);
        }
        else
        {
            m_Camera.rect = new Rect(0.0f, 0.0f, 1.0f, 0.5f);
        }*/

		if (GameData.Instance.PlayerOneCharacter == currentCharacter)
		{
			m_Camera.rect = new Rect(0.0f, 0.0f, 0.5f, 1.0f);
		}
		else
		{
			m_Camera.rect = new Rect(0.5f, 0.0f, 0.5f, 1.0f);
		}

        m_AcceptInputFrom = gameObject.GetComponent<AcceptInputFrom>();

        m_Behaviours.Add(new Rotation2(this));
        m_Behaviours.Add(new LerpToPosition(this));
        m_Behaviours.Add(new LookAt(this));
        m_Behaviours.Add(new Collision(this));
		m_Behaviours.Add(new ActionArea (this));
	}
	
	// Update is called once per frame
	void Update ()
    {
        RotationPoint.transform.position = Vector3.Lerp(RotationPoint.transform.position, Player.transform.position, LERP_AMOUNT);
        for (int i = 0; i < m_Behaviours.Count; i++)
        {
            m_Behaviours[i].behavior();
        }
	}

    //=================================================================================================
    //=================================================================================================
    //behaviors
    #region Behaviors
    protected abstract class Behaviour
    {
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

    protected class ActionArea : Behaviour
    {
        public ActionArea(TPCamera containing)
            : base(containing)
        {
        }

        public override void behavior()
        {
			if(m_Containing.ActionAreaDetect.m_CurrentActionArea != null)
			{
                m_Containing.transform.forward = Helpers.lerpVector3(m_Containing.transform.forward, m_Containing.ActionAreaDetect.m_CurrentActionArea.CameraMountPoint.transform.forward, AUTO_LERP_BASE_AMOUNT * 2.0f);

                m_Containing.gameObject.transform.position = Helpers.lerpVector3(m_Containing.transform.position, m_Containing.ActionAreaDetect.m_CurrentActionArea.CameraMountPoint.transform.position, LERP_AMOUNT / 2.0f);
            }
		}

        public static Vector3 lerpVector3(Vector3 from, Vector3 to, float amount)
        {
            Vector3 direction = new Vector3((to.x - from.x) * amount, (to.y - from.y) * amount, (to.z - from.z) * amount);

            direction = Vector3.ClampMagnitude(direction * amount, (to - from).magnitude);

            return from + direction;
        }
    }

    //=================================================================================================

    protected class Rotation : Behaviour
    {
        public Rotation(TPCamera containing)
            : base(containing)
        {
        }

        public override void behavior()
        {
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

	protected class Rotation2 : Behaviour
	{
		public Rotation2(TPCamera containing)
			: base(containing)
		{
		}
		
		public override void behavior()
		{
			if (m_Containing.ActionAreaDetect.m_CurrentActionArea != null)
			{
				return;
			}

			//get the input for camera Rotation
			Vector2 rotationInput = InputManager.getCamera(m_Containing.m_AcceptInputFrom.ReadInputFrom);

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
				if(InputManager.getMove(m_Containing.m_AcceptInputFrom.ReadInputFrom).magnitude != 0.0f)
				{
                    Vector3 currentEulerAngles = m_Containing.RotationPoint.transform.rotation.eulerAngles;
                    Vector3 targetEulerAngles = m_Containing.Player.transform.rotation.eulerAngles;

                    float percentLerp = 1.0f - (Mathf.Abs(currentEulerAngles.y - targetEulerAngles.y) / 180.0f);
                    percentLerp *= percentLerp;

                    m_Containing.RotationPoint.transform.rotation = Quaternion.Slerp(m_Containing.RotationPoint.transform.rotation, Quaternion.Euler(currentEulerAngles.x, targetEulerAngles.y, targetEulerAngles.z), AUTO_LERP_BASE_AMOUNT * percentLerp);
				}
			}
		}
	}

    //=================================================================================================

    protected class LerpToPosition : Behaviour
    {
        public LerpToPosition(TPCamera containing)
            : base(containing)
        {
        }

        public override void behavior()
        {
			if (m_Containing.ActionAreaDetect.m_CurrentActionArea != null)
			{
				return;
			}
            //lerp to the target position
            m_Containing.gameObject.transform.position = Vector3.Lerp(m_Containing.transform.position, m_Containing.LerpTo.transform.position, LERP_AMOUNT);
        }
    }

    //=================================================================================================

    protected class LookAt : Behaviour
    {
        public LookAt(TPCamera containing)
            : base(containing)
        {
        }

        public override void behavior()
        {
			if (m_Containing.ActionAreaDetect.m_CurrentActionArea != null)
			{
				return;
			}

            //for now just look at the rotaion point
            m_Containing.transform.LookAt(m_Containing.RotationPoint.transform.position);
        }
    }

    //=================================================================================================

    protected class Collision : Behaviour
    {
        public Collision(TPCamera containing)
            : base(containing)
        {
        }

        public override void behavior()
        {
			if (m_Containing.ActionAreaDetect.m_CurrentActionArea != null)
			{
				return;
			}

            float minDist = float.MaxValue;
            minDist = raycast(minDist, m_Containing.transform.right *  m_Containing.RaycastOffset.x,  m_Containing.transform.up * m_Containing.RaycastOffset.y);
            minDist = raycast(minDist, m_Containing.transform.right * -m_Containing.RaycastOffset.x,  m_Containing.transform.up * m_Containing.RaycastOffset.y);
            minDist = raycast(minDist, m_Containing.transform.right *  m_Containing.RaycastOffset.x, -m_Containing.transform.up * m_Containing.RaycastOffset.y);
            minDist = raycast(minDist, m_Containing.transform.right * -m_Containing.RaycastOffset.x, -m_Containing.transform.up * m_Containing.RaycastOffset.y);

            if (minDist == float.MaxValue)
            {
                minDist = (m_Containing.transform.position - m_Containing.Player.transform.position).magnitude;
            }

            Vector3 direction = m_Containing.transform.position - m_Containing.Player.transform.position;

            direction.Normalize();

            if (m_Containing.DrawRays)
            {
                Debug.DrawRay(m_Containing.Player.transform.position, direction * minDist, Color.black);
            }

            if (minDist < (m_Containing.transform.position - m_Containing.Player.transform.position).magnitude)
            {
                m_Containing.transform.position = m_Containing.Player.transform.position + (direction * minDist);
            }
        }

        float raycast(float currentMinDist, Vector3 offsetX, Vector3 offsetY)
        {
            Vector3 RayDirection = (m_Containing.transform.position + offsetX + offsetY) - m_Containing.Player.transform.position;
            float raycastDistance;

            if ((m_Containing.LerpTo.transform.position - m_Containing.Player.transform.position).magnitude > (m_Containing.transform.position - m_Containing.Player.transform.position).magnitude)
            {
                raycastDistance = (m_Containing.LerpTo.transform.position - m_Containing.Player.transform.position).magnitude;
            }
            else
            {
                raycastDistance = (m_Containing.transform.position - m_Containing.Player.transform.position).magnitude;
            }

            RaycastHit raycastInfo;
            Physics.Raycast(m_Containing.Player.transform.position, RayDirection, out raycastInfo, raycastDistance, ~(LayerMask.GetMask(Constants.PLAYER_STRING) | LayerMask.GetMask(CAMERA_IGNORE_COLLISION_LAYER)));

            if (m_Containing.DrawRays)
            {
                if (raycastInfo.collider != null)
                {
                    Debug.DrawRay(m_Containing.Player.transform.position, raycastInfo.point - m_Containing.Player.transform.position, Color.cyan);
                }
                else
                {
                    Debug.DrawRay(m_Containing.Player.transform.position, m_Containing.transform.position + offsetX + offsetY - m_Containing.Player.transform.position, Color.blue);
                }
            }

            if (raycastInfo.collider == null)
            {
                return currentMinDist;
            }


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
