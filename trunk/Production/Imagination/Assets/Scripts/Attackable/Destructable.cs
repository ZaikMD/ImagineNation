using UnityEngine;
using System.Collections;

public class Destructable : MonoBehaviour, Attackable
{

    public float m_Health;
    public GameObject m_Ragdoll;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	protected void Update () 
    {
        if (m_Health <= 0)
        {
            Instantiate(m_Ragdoll, transform.position, transform.rotation);
            Destroy(this.gameObject);
        }
	}

    public override  void OnHit(PlayerProjectile proj)
    {        
        m_Health -= 1;        
    }

    public override void OnHit(EnemyProjectile proj)
    {
        
    }
}
