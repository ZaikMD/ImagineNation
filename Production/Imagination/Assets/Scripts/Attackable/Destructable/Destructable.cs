using UnityEngine;
using System.Collections;

public class Destructable : MonoBehaviour, Attackable
{

    public int m_Health;
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
			onDeath();
        }
	}

    public virtual void onHit(PlayerProjectile proj)
    {        
        m_Health -= 1;        
    }

    public virtual void onHit(EnemyProjectile proj)
    {
        
    }

	public virtual void instantKill()
	{
		m_Health = 0;
		onDeath ();
	}

	protected virtual void onDeath()
	{
		Instantiate(m_Ragdoll, transform.position, transform.rotation);
		Destroy(this.gameObject);
	}
}
