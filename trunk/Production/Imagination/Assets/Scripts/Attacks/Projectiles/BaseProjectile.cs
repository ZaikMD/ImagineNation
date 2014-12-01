using UnityEngine;
using System.Collections;
/// <summary>
/// Base projectile.
/// Created by Zach Dubuc
/// 
/// The projectile that the attacks use.
/// 
/// </summary>

#region ChangeLog
/*
* 8/10/2014 Edit: Fully Commented- Zach Dubuc
*
* 
*/
#endregion
public class BaseProjectile : MonoBehaviour 
{


	Vector3 m_InitialPosition;


	public float m_MoveSpeed = 10;
	
	public float m_Range = 2;

	public bool m_Hidden = false;

	Characters m_Character;



	// Use this for initialization
	void Start () 
	{
		m_InitialPosition = transform.position;
		if(m_Hidden)
		{
			renderer.enabled = false;
		}
	}


	
	// Update is called once per frame
	void Update () 
	{ 
        if (PauseScreen.IsGamePaused){return;}

		transform.position += transform.forward * m_MoveSpeed * Time.deltaTime; //Move the projectile

		float distance = Vector3.Distance (m_InitialPosition, transform.position); //Get the distanace it's travelled

		if(distance > m_Range)
		{
			Destroy(this.gameObject); //Check the range, if it the distance travelled is greater than it's range, destroy it
		}
	}

	public void setCharacter(Characters character)
	{
		m_Character = character;
	}


	public Characters getCharacter()
	{
		return m_Character;
	}


}
