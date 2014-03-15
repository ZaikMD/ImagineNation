using UnityEngine;
using System.Collections;

public class AlexAttributes : BaseCharAttrib 
{
	public static AlexAttributes Instance{ get; private set; }
	// Use this for initialization
	
	void Awake()
	{
		//if theres another instance (there shouldnt be) destroy it... there can be only one
		if(Instance != null && Instance != this)
		{
			//destroy all other instances
			Destroy(gameObject);
		}
		
		//set the instance
		Instance = this;
		
		//prevents this object being destroyed between scene loads
		DontDestroyOnLoad(gameObject);
	}
	
	void Start () 
	{
		m_Size = Size.Large;
		m_Fear = Fears.Heights;
		m_Item1 = PrimaryItems.NerfGun;
		m_Item2 = SecondaryItems.RcToy;
	}
}
