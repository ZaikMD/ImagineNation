using UnityEngine;
using System.Collections;

public class GnomeMage : BaseEnemy 
{
	public GameObject m_ProjectilePrefab; 

	private float m_ShieldHealth = 2.0f;

	// Use this for initialization
	void Start () 
	{
		base.Start ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		base.Update ();
	}


	protected override void Chase ()
	{
		//This enemy does not Chase He will just start shooting the player
		Instantiate (m_ProjectilePrefab, transform.position, transform.rotation);

	}

	protected override void Fight ()
	{

	}

	protected override void Die ()
	{
		throw new System.NotImplementedException ();
	}
}
