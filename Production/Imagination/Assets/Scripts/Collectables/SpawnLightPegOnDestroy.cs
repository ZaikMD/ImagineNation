using UnityEngine;
using System.Collections;

public class SpawnLightPegOnDestroy : MonoBehaviour 
{
	public int m_MinimumPossibillity;
	public int m_MaximunPossibillity;

	private CollectableManager m_Manager;

	void Start()
	{
		m_Manager = GameObject.FindGameObjectWithTag(Constants.COLLECTABLE_MANAGER).GetComponent<CollectableManager>();
	}

	void OnDestroy()
	{
		if(!Application.isPlaying)
		{
			return;
		}

		int tempNumberOfPegs;

		if(m_MinimumPossibillity == m_MaximunPossibillity)
		{
			tempNumberOfPegs = m_MaximunPossibillity;
		}
		else
		{
			tempNumberOfPegs = Random.Range(m_MinimumPossibillity, m_MaximunPossibillity);
		}

	//	Debug.Log (tempNumberOfPegs);

	//	m_Manager.SpawnLightPegAtLocation (this.gameObject, tempNumberOfPegs);
	}
}
