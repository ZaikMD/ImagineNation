using UnityEngine;
using System.Collections;

public class RemoveDebris : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
		StartCoroutine (DestroyDebris ());

		Component[] colliders = gameObject.GetComponentsInChildren (typeof(Collider));

		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");

		for(int i = 0; i < colliders.Length; i++)
		{
			Physics.IgnoreCollision((Collider)colliders[i], players[0].gameObject.collider);
		}

	}
	
	IEnumerator DestroyDebris()
	{
		yield return new WaitForSeconds (5.0f);
		Destroy (this.gameObject);
	}
}
