using UnityEngine;
using System.Collections;

public class BeanBag : MonoBehaviour
{
	private Vector3 m_LaunchDirection;
	private float m_ProjectileSpeed;

	const ScriptPauseLevel PAUSE_LEVEL = ScriptPauseLevel.Cutscene;

	// Use this for initialization
	void SetVelocity(Vector3 NewLaunchDirection, float NewSpeed)
	{
		m_LaunchDirection = NewLaunchDirection;	
		m_ProjectileSpeed = NewSpeed;
	}

	// Update is called once per frame
	void Update ()
	{
		if (PauseScreen.shouldPause(PAUSE_LEVEL)) { return; }

		transform.position += m_LaunchDirection * m_ProjectileSpeed * Time.deltaTime;
	} 

	void OnTriggerEnter(Collider obj)
	{
		if(obj.gameObject.GetComponent(typeof(Attackable)) as Attackable != null)//checks to see if the object that has been hit is attackable
		{
			Attackable attackable = obj.gameObject.GetComponent(typeof(Attackable)) as Attackable; //if so call the onhit function and pass in the gameobject

			EnemyProjectile tempProjectile = new EnemyProjectile();

			attackable.onHit(tempProjectile);
		} 
		//destroy gameobject;
		Destroy(this.gameObject);
	}
}
