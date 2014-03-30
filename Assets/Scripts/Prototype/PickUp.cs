using UnityEngine;
using System.Collections;

public class PickUp : MonoBehaviour {
	
	public float BounceMultiplier = 3.0f;
	public bool hasPickup = false;

	Vector3 startPosition;
	
	
	// Use this for initialization
	void Start () {
		
		startPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {

		if(hasPickup == false)
		{
		float bounce = Mathf.Sin (Time.time * BounceMultiplier) * 0.2f + startPosition.y;
		
		transform.position = new Vector3 (startPosition.x, bounce, startPosition.z);
		
		transform.Rotate (0, 1, Mathf.Sin (Time.time * BounceMultiplier) *0.2f + 1);
		}
	}
	
	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.name == "DropZone")
		{
			this.transform.parent = 
				other.transform.Find ("DropZonePoint");
			this.transform.localPosition = Vector3.zero;
			this.transform.localRotation = Quaternion.identity;
		}

		else if(other.gameObject.CompareTag("Player"))
		{
			//Destroy(this.gameObject);
			this.transform.parent = 
				other.transform.Find ("ItemPickPoint");
			this.transform.localPosition = Vector3.zero;
			this.transform.localRotation = Quaternion.identity;



			hasPickup = true;
		}
	}
}
