using UnityEngine;
using System.Collections;

/*Created by Kole
 * 
 * Light peg will act simularly to coins in mario
 * When a peg is Collected, it will increment the total  
 *  
 * This Class inherits from BaseCollecatable
 */

public class LightPeg : BaseCollectable
{
	//All the puzzle pieces should have a trigger on them, when enter, this function will be called
    void OnTriggerEnter(Collider other)
    {
		//checks to see if the object in our trigger is a player.
        if (other.gameObject.tag == Constants.PLAYER_STRING)
        {
            //Tell GameData this peg was collected
            GameData.Instance.LightPegCollected(m_ID);

            //increment collectable counter
            m_CollectableManager.IncrementCounter();

			//Play Collected sound 
			PlaySound();

            //destroy this gameobject
            Destroy(this.gameObject);            
        }
    }
}