using UnityEngine;
using System.Collections;
/// <summary>
/// Creatred by Zach Dubuc.
/// 
/// Destructable is a base class for anything that can be destroyed by a player or enemy.
/// </summary>

#region ChangeLog
/*
* 8/10/2014 Edit: Fully Commented- Zach Dubuc
*
* 
*/
#endregion
public class Destructable : MonoBehaviour, Attackable
{

    public int m_Health;
    public GameObject m_Ragdoll;

	// Update is called once per frame
	protected void Update () 
    {
        if (m_Health <= 0)
        {
			onDeath();
        }
	}
    //Onhit will get called by the PLayer and Enemy projectiles
    public virtual void onHit(PlayerProjectile proj)
    {        
        m_Health -= 1;        
    }

    public virtual void onHit(EnemyProjectile proj)
    {
		m_Health -= 1;
    }
    //To instantkill the object
	public virtual void instantKill()
	{
		m_Health = 0;
		onDeath ();
	}
    //Controlls what happens when the object dies
	protected virtual void onDeath()
	{
		if(m_Ragdoll != null)
		{
			Instantiate(m_Ragdoll, transform.position, transform.rotation);
		}
		Destroy(this.gameObject);
	}
}
