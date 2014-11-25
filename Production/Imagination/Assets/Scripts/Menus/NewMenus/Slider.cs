using UnityEngine;
using System.Collections;

public class Slider : MonoBehaviour 
{
	public Transform i_MaxPos;
	public Transform i_MinPos;

	public Transform i_Pointer;

	float m_MaxValue = 1.0f;
	public float MaxValue
	{
		get{ return m_MaxValue; }
		set{ m_MaxValue = value; }
	}

	float m_MinValue = 1.0f;
	public float MinValue
	{
		get{ return m_MinValue; }
		set{ m_MinValue = value; }
	}

	float m_Value = 1.0f;
	public float Value
	{
		get{ return m_Value; }
		set
		{ 
			m_Value = value; 
			updatePointerPos();
		}
	}

	void updatePointerPos ()
	{
		i_Pointer.position = i_MinPos.position + ((i_MaxPos.position - i_MinPos.position) * (m_Value / m_MaxValue));
	}
}
