using UnityEngine;
using System.Collections;

[RequireComponent(typeof (CharacterController))]

public class MovingBlock : Destructable
{

	Vector3 m_Respawn;

	public float m_Distance;

	public Material[] m_Materials;

	public float m_Speed;

	int m_CurrentMaterial = 0;

	Vector3 m_Destination;
	Vector3 m_ZeroVector = Vector3.zero;

	bool m_Hit = false;
	bool m_Moving = false;

	protected int m_SaveHealth;

	float m_HitTimer = 1.0f;

	protected float m_SaveHitTimer;

	public GameObject m_BoxPrefab;

	float m_Gravity = -10.0f;



	// Use this for initialization
	void Start () 
	{
		m_BoxPrefab.renderer.material = m_Materials [m_CurrentMaterial];

		m_Respawn = transform.position;

		m_SaveHealth = m_Health;

		m_Destination = transform.position;

		m_SaveHitTimer = m_HitTimer;

	}
	
	// Update is called once per frame
	void Update () 
	{
		if(m_Health <= 0)
		{
			respawn();
		}


		CharacterController controller = GetComponent<CharacterController>();

		Vector3 direction = m_Destination - transform.position;


		if(direction !=  m_ZeroVector)
		{
			controller.Move(direction * m_Speed* Time.deltaTime);

		} 

		if(m_Hit)
		{
			m_HitTimer -= Time.deltaTime;
		}
		if(m_HitTimer <= 0.0f)
		{
			m_Hit = false;
			m_HitTimer = m_SaveHitTimer;

		}

		fall ();
	}

	void respawn()
	{
		m_CurrentMaterial = 0;
		transform.position = m_Respawn;
		m_Destination = m_Respawn;
		m_BoxPrefab.renderer.material = m_Materials [m_CurrentMaterial];
		m_Health = m_SaveHealth;
	}

	public override void onHit(PlayerProjectile proj)
	{
		return;
	}
	
	public override void onHit(EnemyProjectile proj)
	{
		return;
	}

	void OnTriggerEnter(Collider obj)
	{
		if(!m_Hit)
		{
			if(obj.gameObject.tag == "PlayerProjectile")
			{
				GameObject newObj = obj.gameObject;
				setDestination (obj.gameObject);
			}
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
			//Hit right side of block
			m_Destination = new Vector3(transform.position.x - m_Distance, transform.position.y, transform.position.z);
			m_Health --;
			m_CurrentMaterial ++;
		}

		if(normal == -rayHit.transform.right)
		{
			//hit left side
			m_Destination = new Vector3(transform.position.x + m_Distance, transform.position.y, transform.position.z);
			m_Health --;
			m_CurrentMaterial ++;
		}

		if(normal == rayHit.transform.forward)
		{
			//hit front side
			m_Destination = new Vector3(transform.position.x , transform.position.y, transform.position.z - m_Distance);
			m_Health --;
			m_CurrentMaterial ++;
		}

		if(normal == -rayHit.transform.forward)
		{
			//hit back side
			m_Destination = new Vector3(transform.position.x , transform.position.y, transform.position.z + m_Distance);
			m_Health --;
			m_CurrentMaterial ++;
		}
		
		if(m_CurrentMaterial >= m_Materials.Length)
		{
			m_CurrentMaterial = 0;
		}
		m_BoxPrefab.renderer.material = m_Materials [m_CurrentMaterial];
		m_Hit = true;

	}

	public void setPressurePlateDestination(Vector3 destination)
	{
		m_Destination = destination;
	}

	void fall()
	{
		Vector3 rayDirection = -transform.up;
		
		Ray ray = new Ray (transform.position, rayDirection);
		
		RaycastHit rayHit;
		
		Physics.Raycast (ray, out rayHit, 10.0f);

		if(rayHit.point != null)
		{
			m_Destination.y = rayHit.point.y;
		}

	}

	protected override void onDeath ()
	{
		respawn ();
	}

}
