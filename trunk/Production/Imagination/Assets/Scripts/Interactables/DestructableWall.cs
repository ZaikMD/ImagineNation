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
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public override void onHit (PlayerProjectile proj)
	{
		m_Health -= 1;

		DestroyGroup (m_Groups [m_Health]);

		if (m_Health <= 0)
			onDeath ();
	}

	private void DestroyGroup(GameObject[] group)
	{
		for (int i = 0; i < group.Length; i++)
		{
			group[i].transform.parent = null;
			group[i].AddComponent<Rigidbody>();
			group[i].GetComponent<DestructableWallRagDoll>().enabled = true;
		}
	}

	protected override void onDeath ()
	{
		Destroy (this.gameObject);
	}
}
