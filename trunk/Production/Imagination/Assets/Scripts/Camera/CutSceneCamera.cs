using UnityEngine;
using System.Collections;

public class CutSceneCamera : MonoBehaviour 
{
	public Transform i_Follow;
	public float i_LerpSpeed = 0.05f;
	public float i_RotationalLerpSpeed = 0.05f;

	// Update is called once per frame
	void Update () 
	{
		transform.position = Vector3.Lerp (transform.position, i_Follow.position, i_LerpSpeed);
		transform.rotation = Quaternion.Lerp(transform.rotation, i_Follow.transform.rotation, i_RotationalLerpSpeed);
	}
}
