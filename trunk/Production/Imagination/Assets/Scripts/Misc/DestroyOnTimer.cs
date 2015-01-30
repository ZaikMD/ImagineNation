using UnityEngine;
using System.Collections;

public class DestroyOnTimer : MonoBehaviour {

	public float m_TimeToDestroy;

	private float m_CurrentTime;

	void Start()
	{
		m_CurrentTime = m_TimeToDestroy;
	}
	
	// Update is called once per frame
	void Update() 
	{
		m_CurrentTime -= Time.deltaTime;

		if(m_CurrentTime < 0)
		{
			Destroy(this.gameObject);
		}
	}
}
