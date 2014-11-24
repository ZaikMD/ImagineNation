// Created by Mathieu Elias
using UnityEngine;
using System.Collections;
using System;

// Enum to of the different Modes a control point can have
public enum BezierMoveMode 
{
	Free,
	Aligned,
	Mirrored
}

public class BezierSpline : MonoBehaviour 
{
	//Four starting locations for the initial control points
	[SerializeField]
	public Vector3[] m_Points = new Vector3[] 
	{ 
		new Vector3(0.0f, 0.0f, 0.0f),
		new Vector3(10.0f, 0.0f, 0.0f),
		new Vector3(20.0f, 0.0f, 0.0f),
		new Vector3(30.0f, 0.0f, 0.0f)
	};

	
	[SerializeField]
	private BezierMoveMode[] m_Mode = new BezierMoveMode[]
	{
		BezierMoveMode.Free,
		BezierMoveMode.Free
	
	};

	/// <summary>
	/// Adds three more vertices
	/// </summary>
	public void AddVerts () 
	{
		// Grabbing the final point in the spline
		Vector3 finalPoint = m_Points[m_Points.Length - 1];
	
		// Adding three to the size of the array 
		Array.Resize(ref m_Points, m_Points.Length + 3);

		// Adding three points to the array in a straight line spaced out on the x axis
		finalPoint.x += 10f;
		m_Points[m_Points.Length - 3] = finalPoint;

		finalPoint.x += 10f;
		m_Points[m_Points.Length - 2] = finalPoint;

		finalPoint.x += 10f;
		m_Points[m_Points.Length - 1] = finalPoint;

		//Adding one to the size of the mode array to accomadate the next main vert
		Array.Resize (ref m_Mode, m_Mode.Length + 1);
		m_Mode [m_Mode.Length - 1] = m_Mode [m_Mode.Length - 2];

		// Makeing sure they follow their selected mode
		EnforceMode(m_Points.Length - 4);
	}

	public void RemoveVerts()
	{
		if (m_Points.Length > 4)
		{
			Array.Resize (ref m_Points, m_Points.Length - 3);
			Array.Resize (ref m_Mode, m_Mode.Length - 1);
		}
	}
	
	/// <summary>
	/// Return the amount of control points
	/// </summary>
	/// <returns>The point count.</returns>
	public int ControlPointCount ()
	{
			return m_Points.Length;		
	}

	/// <summary>
	/// Return the control point at the specified index
	/// </summary>
	/// <returns>The control point.</returns>
	/// <param name="index">Index.</param>
	public Vector3 GetControlPoint (int index)
	{
		EnforceMode (index);
		return m_Points[index];

	}

	/// <summary>
	/// Set the control point from specified index
	/// </summary>
	/// <param name="index">Index.</param>
	/// <param name="point">Point.</param>
	public void SetControlPoint (int index, Vector3 point) 
	{
		// If the control point selected is one of the main verts
		if (index % 3 == 0) 
		{
			Vector3 movDif = point - m_Points[index];

			// If we are not the first vert in the spline
			if (index > 0) 
			{
				//Move along with the main vert
				m_Points[index - 1] += movDif;
			}

			//If we are not the last vert in the spline
			if (index + 1 < m_Points.Length) 
			{
				//Move along with the main vert
				m_Points[index + 1] += movDif;
			}
		}

		m_Points[index] = point;
		EnforceMode (index);
	}

	/// <summary>
	/// Get a specific point along the spline
	/// </summary>
	/// <returns>The point.</returns>
	/// <param name="t">T.</param>
	public Vector3 GetPoint (float t) 
	{
		int index;
		if (t >= 1f) 
		{
			t = 1f;
			index = m_Points.Length - 4;
		}
		else 
		{
			t = Mathf.Clamp01(t) * CurveCount;
			index = (int)t;
			t -= index;
			index *= 3;
		}
	
		return transform.TransformPoint(Bezier.GetPoint(m_Points[index], m_Points[index + 1], m_Points[index + 2], m_Points[index + 3], t));
	}	

	/// <summary>
	/// Return the amount of curves in the spline
	/// </summary>
	/// <value>The curve count.</value>
	public int CurveCount 
	{
		get {
			return (m_Points.Length - 1) / 3;
			}
	}

	/// <summary>
	/// Gets the mode of the control point at specified index
	/// </summary>
	/// <returns>The control point mode.</returns>
	/// <param name="index">Index.</param>
	public BezierMoveMode GetControlPointMode (int index) 
	{
		return m_Mode[(index + 1) / 3];
	}

	/// <summary>
	/// Sets the mode of control point at specified index
	/// </summary>
	/// <param name="index">Index.</param>
	/// <param name="mode">Mode.</param>
	public void SetControlPointMode (int index, BezierMoveMode mode) 
	{
		m_Mode[(index + 1) / 3] = mode;
	}

	/// <summary>
	/// Enforces the mode on the control points
	/// </summary>
	/// <param name="index">Index.</param>
	private void EnforceMode (int index) 
	{

		int modeIndex = (index + 1) / 3;

		BezierMoveMode mode = m_Mode[modeIndex];
		// If the mode is free or we are on the 1st or last control point then do nothing
		if (mode == BezierMoveMode.Free || modeIndex == 0 || modeIndex == m_Mode.Length - 1) 
		{
			return;
		}

		int middleIndex = modeIndex * 3;
		int fixedIndex;
		int enforcedIndex;

		if (index <= middleIndex) 
		{
			fixedIndex = middleIndex - 1;
			enforcedIndex = middleIndex + 1;
		}
		else
		{
			fixedIndex = middleIndex + 1;
			enforcedIndex = middleIndex - 1;
		}

		Vector3 middle = m_Points[middleIndex];

		Vector3 enforcedTangent = middle - m_Points[fixedIndex];

		if (mode == BezierMoveMode.Aligned) 
		{
			enforcedTangent = enforcedTangent.normalized * Vector3.Distance(middle, m_Points[enforcedIndex]);
		}

		m_Points[enforcedIndex] = middle + enforcedTangent;
	}

}
