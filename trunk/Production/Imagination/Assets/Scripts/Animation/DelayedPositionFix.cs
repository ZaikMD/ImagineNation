using UnityEngine;
using System.Collections;

public class DelayedPositionFix : MonoBehaviour 
{
	public Transform i_Child;
	const float DELAY = 0.3f;
	// Use this for initialization
	void Start () 
	{
	
		StartCoroutine ("delayedParenting");
	}
	
	IEnumerator delayedParenting()
	{
		yield return new WaitForSeconds (DELAY);

		i_Child.position = transform.position;
		i_Child.rotation = transform.rotation;
		i_Child.parent = transform;
	}
}
