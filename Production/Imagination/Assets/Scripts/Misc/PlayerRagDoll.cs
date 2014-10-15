using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animation))]
public class PlayerRagDoll : MonoBehaviour 
{
	public GameObject GhostPrefab;
	GameObject m_Ghost;

	Animation m_Animator;

	float m_Timer = DeadPlayerManager.Instance.m_RespawnTimer;

	public TPCamera m_PlayerCamera;

	// Use this for initialization
	void Start () 
	{
		m_Animator = gameObject.GetComponent<Animation> ();
		m_Animator.Play (Constants.Animations.DEATH);

		//Collider[] colliders = gameObject.GetComponent<Collider> ();
		//ignore player collision?
		m_Ghost = (GameObject)GameObject.Instantiate (GhostPrefab, transform.position, transform.rotation);

		m_PlayerCamera.Player = m_Ghost;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(m_Ghost != null)
		{
			m_Ghost.transform.Rotate (new Vector3(0.0f,1.0f,0.0f));
			m_Ghost.transform.position = m_Ghost.transform.position + (new Vector3 (0.0f, 1.0f, 0.0f) * Time.deltaTime);
		}

		m_Timer -= Time.deltaTime;
		if(m_Timer < 0.0f)
		{
			Destroy(m_Ghost);
			Destroy(this.gameObject);
		}
	}
}
