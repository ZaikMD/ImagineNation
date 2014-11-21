// Created by Mathieu Elias

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BezierSpline))]
public class BezierSplineInspector : Editor 
{
	private BezierSpline m_Spline;

	private Transform m_HandleTransform;
	private Quaternion m_HandleRotation;
	private const float m_HandleSize = 0.04f;
	private const float m_PickSize = 0.3f;

	private int m_SelectedIndex = -1;
	private float m_SplineSize = 3.0f;

	private static Color[] modeColors = 
	{
		Color.white,
		Color.green,
		Color.blue
	};

	/// <summary>
	/// Draws the splines
	/// </summary>
	private void OnSceneGUI () 
	{
		m_Spline = target as BezierSpline;
		m_HandleTransform = m_Spline.transform;

		if (Tools.pivotRotation == PivotRotation.Local) 
			m_HandleRotation = m_HandleTransform.rotation;
		else
			m_HandleRotation = Quaternion.identity;


		Vector3 point0 = ShowPoint(0);
		for (int i = 1; i < m_Spline.ControlPointCount(); i += 3) 
		{
			Vector3 point1 = ShowPoint(i);
			Vector3 point2 = ShowPoint(i + 1);
			Vector3 point3 = ShowPoint(i + 2);
			
			Handles.color = Color.gray;
			Handles.DrawLine(point0, point1);
			Handles.DrawLine(point2, point3);
			
			Handles.DrawBezier(point0, point3, point1, point2, Color.white, null, m_SplineSize);
			point0 = point3;
		}
	}


	/// <summary>
	/// Shows the point.
	/// </summary>
	/// <returns>The point.</returns>
	/// <param name="index">Index.</param>
	private Vector3 ShowPoint (int index) 
	{
		Vector3 point = m_HandleTransform.TransformPoint(m_Spline.GetControlPoint(index));
		Handles.color = modeColors[(int)m_Spline.GetControlPointMode(index)];
		float size = HandleUtility.GetHandleSize (point);

		if (Handles.Button(point, m_HandleRotation, size*m_HandleSize, size*m_PickSize, Handles.DotCap)) 
		{
			m_SelectedIndex = index;
			Repaint();
		}

		if (m_SelectedIndex == index) 
		{
			EditorGUI.BeginChangeCheck();
			point = Handles.DoPositionHandle(point, m_HandleRotation);

			if (EditorGUI.EndChangeCheck()) 
			{
				Undo.RecordObject(m_Spline, "Move Point");
				EditorUtility.SetDirty(m_Spline);
				m_Spline.SetControlPoint(index, m_HandleTransform.InverseTransformPoint(point));
			}
		}
		return point;
	}

	/// <summary>
	/// Raises the inspector GU event.
	/// </summary>
	public override void OnInspectorGUI () 
	{
		DrawDefaultInspector();
		//m_Spline = target as BezierSpline;

		if (m_SelectedIndex >= 0 && m_SelectedIndex < m_Spline.ControlPointCount()) 
		{
			DrawSelectedPointInspector();
		}


		if (GUILayout.Button("Add Verts"))
		{
			Undo.RecordObject(m_Spline, "Add Verts");
			m_Spline.AddVerts();
			EditorUtility.SetDirty(m_Spline);
		}

		if (GUILayout.Button("Remove Verts"))
		{
			Undo.RecordObject(m_Spline, "Remove Verts");
			m_Spline.RemoveVerts();
			EditorUtility.SetDirty(m_Spline);
		}
	}

	/// <summary>
	/// Draws the selected point inspector.
	/// </summary>
	private void DrawSelectedPointInspector() 
	{
		GUILayout.Label("Selected Point");
		EditorGUI.BeginChangeCheck();
		Vector3 point = EditorGUILayout.Vector3Field("Position", m_Spline.GetControlPoint(m_SelectedIndex));

		if (EditorGUI.EndChangeCheck()) 
		{
			Undo.RecordObject(m_Spline, "Move Point");
			m_Spline.SetControlPoint(m_SelectedIndex, point);
			EditorUtility.SetDirty(m_Spline);
		}

		EditorGUI.BeginChangeCheck();
		BezierMoveMode mode = (BezierMoveMode)
			EditorGUILayout.EnumPopup("Mode", m_Spline.GetControlPointMode(m_SelectedIndex));

		if (EditorGUI.EndChangeCheck()) 
		{
			Undo.RecordObject(m_Spline, "Change Point Mode");
			m_Spline.SetControlPointMode(m_SelectedIndex, mode);
			EditorUtility.SetDirty(m_Spline);
		} 
	}
}