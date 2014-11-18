using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(AcceptInputFrom))]
public class ThirdPersonCamera : MonoBehaviour 
{
	public GameObject FollowPoint;
	public GameObject Player;

	public Vector2 RaycastOffset;

	public Players CurrentPlayer;

	public bool DrawRays = false;

	AcceptInputFrom m_AcceptInputFrom;
	Camera m_Camera;
	// Use this for initialization
	void Start () 
	{
		m_Camera = gameObject.GetComponent<Camera> ();
		m_AcceptInputFrom = gameObject.GetComponent<AcceptInputFrom> ();

		switch(CurrentPlayer)
		{
		case Players.PlayerOne:
			m_Camera.rect = new Rect(0.0f, 0.5f, 1.0f, 0.5f);
			break;
		case Players.PlayerTwo:
			m_Camera.rect = new Rect(0.0f, 0.0f, 1.0f, 0.5f);
			break;
		default:
			Debug.LogError("set the player");
			break;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
        //rotate();
		//lerpPosition ();
		//collision();
		//transform.LookAt (Player.transform.position + (Player.transform.forward * 2.0f));
	}

	void lerpPosition()
	{
		transform.position = Vector3.Lerp (transform.position, FollowPoint.transform.position, 0.05f);
	}

	void collision()
	{
		float minDist = float.MaxValue;
		minDist = raycast(minDist, transform.right *  RaycastOffset.x,  transform.up * RaycastOffset.y);
		minDist = raycast(minDist, transform.right * -RaycastOffset.x,  transform.up * RaycastOffset.y);
		minDist = raycast(minDist, transform.right *  RaycastOffset.x, -transform.up * RaycastOffset.y);
		minDist = raycast(minDist, transform.right * -RaycastOffset.x, -transform.up * RaycastOffset.y);

		if(minDist == float.MaxValue)
		{
			minDist = (transform.position - Player.transform.position).magnitude;
		}

		Vector3 direction = transform.position - Player.transform.position;

		direction.Normalize ();

		if(DrawRays)
		{
			Debug.DrawRay(Player.transform.position, direction * minDist, Color.black);
		}

		if(minDist < (transform.position - Player.transform.position).magnitude)
		{
			transform.position = Player.transform.position + (direction * minDist);
		}
	}

	float raycast(float currentMinDist, Vector3 offsetX, Vector3 offsetY)
	{
		Vector3 RayDirection = (transform.position + offsetX + offsetY) - Player.transform.position;
		float raycastDistance;

		if((FollowPoint.transform.position - Player.transform.position).magnitude > (transform.position - Player.transform.position).magnitude)
		{
			raycastDistance = (FollowPoint.transform.position - Player.transform.position).magnitude;
		}
		else
		{
			raycastDistance = (transform.position - Player.transform.position).magnitude;
		}

		RaycastHit raycastInfo;
		Physics.Raycast (Player.transform.position, RayDirection, out raycastInfo, raycastDistance, ~(LayerMask.GetMask ("Player") | LayerMask.GetMask ("CameraCollisionIgnore")));

		if(DrawRays)
		{
			if(raycastInfo.collider != null)
			{
				Debug.DrawRay(Player.transform.position, raycastInfo.point - Player.transform.position, Color.cyan);
			}
			else
			{
				Debug.DrawRay(Player.transform.position, transform.position + offsetX + offsetY - Player.transform.position, Color.blue);
			}
		}

		if(raycastInfo.collider == null)
		{
			return currentMinDist;
		}


		if(currentMinDist <= (raycastInfo.point - Player.transform.position).magnitude)
		{
			return currentMinDist;
		}
		else
		{
			return (raycastInfo.point - Player.transform.position).magnitude;
		}
	}
}
