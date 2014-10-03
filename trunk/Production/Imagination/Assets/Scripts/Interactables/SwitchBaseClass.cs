using UnityEngine;
using System.Collections;

public class SwitchBaseClass : MonoBehaviour
{
    protected bool m_Activated;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    public bool getActive()
    {
        return m_Activated;
    }
}
