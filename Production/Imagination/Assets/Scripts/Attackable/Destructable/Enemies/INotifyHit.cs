/*
 * Created by Joe Burchill December 03/2014
 * An interface for notifying when the enemy gets hit by the player.
 * Primarily used for Enemy Behaviour state machines.
 * 
 */

using UnityEngine;
using System.Collections;

public interface INotifyHit 
{
	//Function to add enemy functionality for when the enemy is hit by a player
	void NotifyHit();
}
