using UnityEngine;
using System.Collections;

public class Rotation : MonoBehaviour 
{
	public float m_XIncrement = 0.0f;
	public float m_YIncrement = 0.0f;
	public float m_ZIncrement = 0.0f;

	// Update is called once per frame
	void Update () 
	{
		transform.Rotate(new Vector3(m_XIncrement ,m_YIncrement, m_ZIncrement));
	}
}
