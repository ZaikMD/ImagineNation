using UnityEngine;
using System.Collections;

public class CrochuckTeeth : Subject, Destructable
{
	public void applyDamage(int amount)
	{
		sendEvent (ObeserverEvents.Destroyed);
		Destroy (this.gameObject);
	}
}
