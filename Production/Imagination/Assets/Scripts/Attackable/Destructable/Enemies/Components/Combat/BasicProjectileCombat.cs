using UnityEngine;
using System.Collections;

public class BasicProjectileCombat : BaseCombat 
{
	public override void Combat ()
	{
		if (m_ProjectilePrefab != null)
			Instantiate ((Object)m_ProjectilePrefab, transform.position, transform.rotation);
	}
}
