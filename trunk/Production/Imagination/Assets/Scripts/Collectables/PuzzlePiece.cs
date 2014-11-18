﻿using UnityEngine;
using System.Collections;

/*Created by: kole
 * 
 * handles the collision of the Puzzle pieces
 * this class inherits from Base collecable 
 */

public class PuzzlePiece : BaseCollectable
{
	//All the puzzle pieces should have a trigger on them, when enter, this function will be called
	void OnTriggerEnter(Collider other)
	{
		//checks to see if the object in our trigger is a player.
		if (other.gameObject.tag == Constants.PLAYER_STRING)
		{
			//Tell GameData this peg was collected
			GameData.Instance.PuzzlePieceCollected(m_ID);

			//increment collectable counter
			m_CollectableManager.IncrementPuzzleCounter();

			//Play the collected sound
			PlaySound();
			
			//destroy this gameobject
			Destroy(this.gameObject);            
		}
	}	
}