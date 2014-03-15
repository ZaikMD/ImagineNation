using UnityEngine;
using System.Collections;

public enum Fears
{
	Heights,
	Claustrophobia,
	Dark
}

public enum PrimaryItems
{
	NerfGun,
	BoxingGloves,
	StickyHand
}

public enum SecondaryItems
{
	RcToy,
	VelcroGloves,
	Cape
}

public enum Size
{
	Small,
	Medium,
	Large
}

public abstract class BaseCharAttrib : MonoBehaviour 
{
	protected Size m_Size;
	protected Fears m_Fear;
	protected PrimaryItems m_Item1;
	protected SecondaryItems m_Item2;

	public virtual Size getWeight()
	{
		return m_Size;
	}
	
	public virtual Size getHeight()
	{
		return m_Size;
	}
	
	public virtual Fears getFear ()
	{
		return m_Fear;
	}
	
	public virtual PrimaryItems getFirstItem ()
	{
		return m_Item1;
	}
	
	public virtual SecondaryItems getSecondItem ()
	{
		return m_Item2;
	}
}
