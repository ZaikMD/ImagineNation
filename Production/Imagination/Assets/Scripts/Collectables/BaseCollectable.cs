using UnityEngine;
using System.Collections;

/*Created By: Kole
 * 
 * this is the base class for our collectable
 * the only difference between the puzzle piece 
 * and light peg functionanlly is the data sent on pick up
 * 
 */

[RequireComponent(typeof(CharacterController))]

public abstract class BaseCollectable : MonoBehaviour {

	// used so we know which element of the array we are;
   	protected int m_ID;

	//References to other Components.
	protected SFXManager m_SFX;
	protected CharacterController m_Controller;	
	protected CollectableManager m_CollectableManager;

	void Start ()
	{
		//Setting our references
		m_SFX = GameObject.FindGameObjectWithTag (Constants.SOUND_MANAGER).GetComponent<SFXManager>();
		m_Controller = gameObject.GetComponent<CharacterController>();
		m_CollectableManager = GameObject.FindGameObjectWithTag(Constants.COLLECTABLE_MANAGER).GetComponent<CollectableManager>();
	}

	void Update()
	{
		//This will apply gravity for us
		Vector3 speed = Vector3.zero;
		m_Controller.SimpleMove(speed);
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
			m_SFX.playSound(this.transform.position, Sounds.Collectable);
		}
		else
		{
#if DEBUG || UNITY_EDITOR
			Debug.LogError("Sound Manager was not found");
#endif
		}
	}
}
