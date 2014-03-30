using UnityEngine;
using System.Collections;

public class RemoveDebris : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
		StartCoroutine (DestroyDebris ());
	}
	
	IEnumerator DestroyDebris()
	{
		yield return new WaitForSeconds (2.0f);
		Destroy (this.gameObject);
	}
}
