using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    public Transform m_Player;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        Vector3 vect = new Vector3(m_Player.position.x, m_Player.position.y, m_Player.position.z);
        transform.position = vect;
	}
}
