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
* 15/10/2014 Edit: Added the tag check in onHit - Mathieu Elias
* 
* 23/11/2014 Edit: Added the set health function because I need to raise one of the enemys health while he is fighting - Mathieu Elias
* 
* 03/12/2014 Edit: Changed Health to float value, to coincide with damage - Joe Burchill
* 
* 04/12/2014 Edit: Changed Enemy Damage to a const float for Enemy Projectile onHit. - Joe Burchill
*/
#endregion
public class Destructable : MonoBehaviour, Attackable
{
    public float m_Health;
    public GameObject m_Ragdoll;
	public float m_RagdollHeightOffset;
	public SFXManager m_SFX;

	protected const float ENEMY_DAMAGE = 1.0f;

	protected void Start()
	{
		m_SFX = SFXManager.Instance;
	}

	// Update is called once per frame
	protected void Update () 
    {
        if (m_Health <= 0)
        {
			onDeath();
        }
	}
    //Onhit will get called by the Player and Enemy projectiles

    public virtual void onHit(LightCollider proj, float damage)
    {
        if (this.tag != Constants.PLAYER_STRING)
            m_Health -= damage; 
    }

    public virtual void onHit(HeavyCollider proj, float damage)
    {
        if (this.tag != Constants.PLAYER_STRING)
            m_Health -= damage; 
    }

    public virtual void onHit(EnemyProjectile proj)
    {
		if (this.tag == Constants.PLAYER_STRING)
		m_Health -= ENEMY_DAMAGE;

    }

	public virtual void onHit(EnemyProjectile proj, Vector3 KnockBackDirection)
	{
		if (this.tag == Constants.PLAYER_STRING)
			m_Health -= ENEMY_DAMAGE;
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

	public void SetHealth(float health)
	{
		m_Health = health;
	}
}
