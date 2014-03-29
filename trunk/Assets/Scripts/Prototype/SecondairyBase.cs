/*

3/29/2014 - Jason
	Now has a load function inheriting classes can call in their start functions
	Now is enabled by default

*/


using UnityEngine;
using System.Collections;

public abstract class SecondairyBase : MonoBehaviour {

    protected bool m_Enabled = true;
    protected PlayerMovement m_PlayerMovement;
	protected PlayerInput m_InputInstance;
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
    bool isEnabled
    {
        get { return m_Enabled; }
        set { m_Enabled = true; }
    }

	/// <summary>
	/// Move the player according to this item's interaction
	/// </summary>
    public abstract void Move();
    
}
