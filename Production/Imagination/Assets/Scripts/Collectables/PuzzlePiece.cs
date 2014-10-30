using UnityEngine;
using System.Collections;

public class PuzzlePiece : BaseCollectable
{
	CharacterController m_Controller;
	CollectableManager m_CollectableManager;
	
	// Use this for initialization
	void Start()
	{
		m_Controller = gameObject.GetComponent<CharacterController>();
		m_CollectableManager = GameObject.FindGameObjectWithTag(Constants.COLLECTABLE_MANAGER).GetComponent<CollectableManager>();
	}
	
	// Update is called once per frame
	void Update()
	{
		//This will apply gravity for us
		Vector3 speed = Vector3.zero;
		m_Controller.SimpleMove(speed);
	}
	
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == Constants.PLAYER_STRING)
		{
			//Tell GameData this peg was collected
			GameData.Instance.PuzzlePieceCollected(m_ID);

			//increment collectable counter
			m_CollectableManager.IncrementPuzzleCounter();
			
			//destroy this gameobject
			Destroy(this.gameObject);            
		}
	}
	
	
}