using UnityEngine;
using System.Collections;

/*Created By: Kole
 * 
 * this is the base class for our collectable
 * the only difference between the puzzle piece 
 * and light peg functionanlly is the data sent on pick up
 * 
 */

public abstract class BaseCollectable : MonoBehaviour {

	// used so we know which element of the array we are;
   	protected int m_ID;

	//References to other Components.
	protected SFXManager m_SFX;
	protected CollectableManager m_CollectableManager;

	void Start ()
	{
		//Setting our references
		m_SFX = SFXManager.Instance;
		m_CollectableManager = GameObject.FindGameObjectWithTag(Constants.COLLECTABLE_MANAGER).GetComponent<CollectableManager>();

		SetOnGround();
	}

	void SetOnGround()
	{
		//This will later be handle by a tool. 
		//RayCast Down, and place object at loction plus our desired height retrived from collectable manager

		Vector3 StartingPlace = transform.position;
		Vector3 StraightDown = transform.up * -1;
		Vector3 DistanceFromGround = new Vector3(0, m_CollectableManager.DistanceFromGround, 0);

		Ray Direction = new Ray(StartingPlace, StraightDown);
		RaycastHit HitInfo;

		Physics.Raycast (Direction, out HitInfo);

		transform.position = HitInfo.point + DistanceFromGround;
	}

	//Sets our if and Type
	public void SetInfo(int id)
	{
		m_ID = id;
	}

	protected void PlaySound()
	{
		if(m_SFX != null)
		{
			m_SFX.playSound(transform, Sounds.Collectable);
		}
		else
		{
#if DEBUG || UNITY_EDITOR
			Debug.LogError("Sound Manager was not found");
#endif
		}
	}
}
