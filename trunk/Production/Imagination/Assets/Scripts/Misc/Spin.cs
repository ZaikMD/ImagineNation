/*
*Spin
*
*spins things
*
*Created by: Kris Matis
*/

#region ChangeLog
/*
*
* 27/10/2014 edit: made the script
 * 
*/
#endregion

using UnityEngine;
using System.Collections;

public class Spin : MonoBehaviour 
{
	//the speed to rotate the object in each axis
	public Vector3 Speed;

	//the euler angles 
	Vector3 m_EulerAngles;

	void Start()
	{
		//set the initial euler angles
		m_EulerAngles = transform.eulerAngles;
	}

	// Update is called once per frame
	void Update () 
	{
		//update the euler angles
		m_EulerAngles += (Speed * Time.deltaTime);
		//set the euler angles
		transform.eulerAngles = m_EulerAngles;
	}
}
