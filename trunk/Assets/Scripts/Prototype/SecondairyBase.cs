/*
3/29/2014 - Kris
Commented out lines 23,24,26,30
script wasnt compileing 

*/


/*

3/29/2014 - Jason
	Now has a load function inheriting classes can call in their start functions
	Now is enabled by default

*/


using UnityEngine;
using System.Collections;

public abstract class SecondairyBase : MonoBehaviour {

//<<<<<<< .mine
    //public bool m_Enabled = false;
    public PlayerMovement m_playerMovement;
//=======
    protected bool m_Enabled = true;
    protected PlayerMovement m_PlayerMovement;
	protected PlayerInput m_InputInstance;
//>>>>>>> .r42
	// Use this for initialization
	void Start ()
    {
		//m_PlayerMovement = gameObject.GetComponent<PlayerMovement>();
		Load ();
	}

	/// <summary>
	/// Load this instances movement and input member variables.
	/// </summary>
	protected void Load ()
	{
		m_PlayerMovement = (PlayerMovement)gameObject.GetComponent<PlayerMovement>();
		m_InputInstance = PlayerInput.Instance;
	}

	/// <summary>
	/// Gets or sets a value indicating whether this item can be used.
	/// </summary>
	/// <value><c>true</c> if is enabled; otherwise, <c>false</c>.</value>
    public bool isEnabled
    {
        get { return m_Enabled; }
        set { m_Enabled = true; }
    }

	/// <summary>
	/// Move the player according to this item's interaction
	/// </summary>
    public abstract void Move();
    
}
