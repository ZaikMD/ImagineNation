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
	public Vector3 m_EulerAngles;
	
	Quaternion m_OriginalRotation;

	protected void Start()
	{
		//set the initial euler angles
		m_EulerAngles = transform.eulerAngles;

		m_OriginalRotation = gameObject.transform.rotation;
	}

	// Update is called once per frame
	protected void Update () 
	{
		//update the euler angles
		m_EulerAngles += (Speed * Time.deltaTime);
		//set the euler angles
		transform.eulerAngles = m_EulerAngles;
	}

	public void resetAngles()
	{
		m_EulerAngles = m_OriginalRotation.eulerAngles;
	}
}
