using UnityEngine;
using System.Collections;

public class ZoeMovementWings : BaseMovementAbility {


	private bool m_CanGlide;

	// Use this for initialization
	void Start () 
	{
		base.Start ();
		//m_CanGlide = true;
	}
	
	// Update is called once per frame
	void Update () 
	{  
		//if(GetIsGrounded())
		//{
		//	m_CanGlide = true;
		//}
		//
		//if(InputManager.getJumpUp(m_AcceptInputFrom.ReadInputFrom))
		//{
		//	m_CanGlide = false;
		//}

		base.Update ();



	}


}
