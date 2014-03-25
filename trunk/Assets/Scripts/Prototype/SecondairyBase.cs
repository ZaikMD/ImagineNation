using UnityEngine;
using System.Collections;

public abstract class SecondairyBase : MonoBehaviour {

    protected bool m_Enabled = false;
    protected PlayerMovement m_playerMovement;
	// Use this for initialization
	void Start () 
    {
        m_playerMovement = gameObject.GetComponent<PlayerMovement>();
	}
	
	// Update is called once per frame
	void Update ()
    {
	

	}

    bool isEnabled
    {
        get { return m_Enabled; }
        set { m_Enabled = true; }
    }

    public abstract void Move();
    
}
