using UnityEngine;
using System.Collections;

public class DestructableWall : Destructable 
{

	public GameObject[] m_GroupOne;
	public GameObject[] m_GroupTwo;
	public GameObject[] m_GroupThree;

	private GameObject[][] m_Groups;
	

	// Use this for initialization
	void Start () 
	{
		m_Groups = new GameObject[3][];

		m_Groups[0] = m_GroupThree;
		m_Groups[1] = m_GroupTwo;
		m_Groups[2] = m_GroupOne;
	}

	public override void onHit (HeavyCollider proj, float damage)
	{
		m_Health -= 1;
		
		if(m_Health >= 0)
			DestroyGroup (m_Groups [(int)m_Health]);
		
		if (m_Health <= 0)
			onDeath ();
	}


	public override void onHit (LightCollider proj, float damage)
	{
		m_Health -= 1;
		 
		if(m_Health >= 0)
			DestroyGroup (m_Groups [(int)m_Health]);
		
		if (m_Health <= 0)
			onDeath ();
	}

	private void DestroyGroup(GameObject[] group)
	{
		for (int i = 0; i < group.Length; i++)
		{
			group[i].transform.parent = null;
			group[i].AddComponent<Rigidbody>();
			group[i].AddComponent<CapsuleCollider>();
			group[i].GetComponent<DestructableWallRagDoll>().enabled = true;
		}
	}

	protected override void onDeath ()
	{
		Destroy (this.gameObject);
	}
}
