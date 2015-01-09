using UnityEngine;
using System.Collections;

/*	Created: Kole Tackney
 * 	Date: 01 / 09 / 2015
 * 
 * This class is being created to have objects bob
 *  
 * 
 */

public class Bob : MonoBehaviour {

	public float m_BobRange;
	public float m_BobSpeed;

	float Offset;

	void Start()
	{
		Offset = Random.Range (0.0f, 1.0f);	
	}

	// Update is called once per frame
	void Update () 
	{
		float MovementAmount;
		MovementAmount = Mathf.Sin((Time.time + Offset) * m_BobSpeed) * (m_BobRange * 0.01f);
		transform.position = new Vector3(transform.position.x, transform.position.y + MovementAmount, transform.position.z); 
	}
}
