using UnityEngine;
using System.Collections;

public class ActivatableMovingPlatform : Activatable 
{
	bool m_CanBeActivated;
	bool m_IsActive;
	bool m_AtFinalDestination;
	bool m_IsReversing;
	float m_AtCheckPointTimer;
	public GameObject[] m_ForwardDestinations;
	public GameObject[] m_ReverseDestinations;

	void Start () 
	{
	
	}

	void Update () 
	{
	
	}
}
