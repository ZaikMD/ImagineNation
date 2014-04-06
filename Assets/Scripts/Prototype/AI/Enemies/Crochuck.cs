using UnityEngine;
using System.Collections;

//Last updated 04/06/2014

public class Crochuck : BaseEnemy 
{

	// Use this for initialization
	void Start () 
	{

	}

	protected override void die()
	{
		//TODO:play death animation and instantiate ragdoll
		Destroy (this.gameObject);
	}

	protected override void fightState()
	{
		//TODO:play attack animation and attack sound
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
