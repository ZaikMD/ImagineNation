using UnityEngine;
using System.Collections;

[RequireComponent(typeof (CharacterController))]

public class MovingBlock : Destructable
{

	Vector3 m_Respawn;

	public float m_Distance;

	public Material[] m_Materials;

	int m_CurrentMaterial = 0;

	Vector3 m_Destination;

	bool m_Hit = false;
	bool m_Moving = false;

	protected int m_SaveHealth;


	// Use this for initialization
	void Start () 
	{
		gameObject.renderer.material = m_Materials [m_CurrentMaterial];

		m_Respawn = transform.position;

		m_SaveHealth = m_Health;

		m_Destination = transform.position;

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

		transform.position = m_Destination;
	}

	void respawn()
	{
		m_CurrentMaterial = 0;
		transform.position = m_Respawn;

		gameObject.renderer.material = m_Materials [m_CurrentMaterial];
		m_Health = m_SaveHealth;
	}

	public override void onHit(PlayerProjectile proj)
	{
		if(!m_Hit)
		{
			m_Health --;
			m_CurrentMaterial ++;

			if(m_CurrentMaterial >= m_Materials.Length)
			{
				m_CurrentMaterial = 0;
			}
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

		if(obj.gameObject.tag == "PlayerProjectile")
		{
			GameObject newObj = obj.gameObject;
			setDestination (obj.gameObject);
		}
	}

	void setDestination(GameObject obj)
	{
		Vector3 rayDirection = transform.position - obj.transform.position;

		Ray ray = new Ray(obj.transform.position, rayDirection);

		RaycastHit rayHit;


		Physics.Raycast (ray, out rayHit);



		Vector3 normal = rayHit.normal;

		normal = rayHit.transform.TransformDirection (normal);



		if(normal == rayHit.transform.right)
		{
			//Hit right side of bock
			m_Destination = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z);
		}

		if(normal == -rayHit.transform.right)
		{
			//hit left side
			m_Destination = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z);
		}

		if(normal == rayHit.transform.forward)
		{
			//hit left side
			m_Destination = new Vector3(transform.position.x , transform.position.y, transform.position.z - 1);
		}

		if(normal == -rayHit.transform.forward)
		{
			//hit left side
			m_Destination = new Vector3(transform.position.x , transform.position.y, transform.position.z + 1);
		}


	}

}
