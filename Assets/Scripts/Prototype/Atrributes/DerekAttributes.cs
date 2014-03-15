using UnityEngine;
using System.Collections;

public class DerekAttributes : BaseCharAttrib 
{
	public static DerekAttributes Instance{ get; private set; }
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
		m_Size = Size.Medium;
		m_Fear = Fears.Claustrophobia;
		m_Item1 = PrimaryItems.BoxingGloves;
		m_Item2 = SecondaryItems.VelcroGloves;
	}
}
