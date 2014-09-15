using UnityEngine;
using System.Collections;

public class MovingBlock : Destructable
{

	public GameObject m_Respawn;

	public float m_Distance;

	public Material[] m_Materials;

	int m_CurrentMaterial = 0;

	Vector3 m_Destination;

	bool m_Hit = false;
	bool m_Moving = false;


	// Use this for initialization
	void Start () 
	{
		gameObject.renderer.material = m_Materials [m_CurrentMaterial];
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(!m_Moving)
		{
			m_Hit = false;
		}


		if(m_Health <= 0)
		{
			respawn();
		}
	}

	void respawn()
	{
		m_CurrentMaterial = 0;
		transform.position = m_Respawn.transform.position;
	}

	public override void onHit(PlayerProjectile proj)
	{
		if(!m_Hit)
		{
			m_CurrentMaterial ++;
			gameObject.renderer.material = m_Materials [m_CurrentMaterial];
			m_Hit = true;
		}
	}
	
	public override void onHit(EnemyProjectile proj)
	{
		return;
	}

	void OnTriggerEnter(Collider obj)
	{
		GameObject newObj = obj.gameObject;
	}

	void setDestination(GameObject obj)
	{

	}

}
