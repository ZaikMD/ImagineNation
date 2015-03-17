using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class PosTracker : MonoBehaviour 
{
	public Transform m_Target;
	Vector2 m_TargetPos;

	StreamWriter m_Writer;
	string m_StreamPath;

	int m_NumbOfSamples = 0;	
	const float m_SampleRate = 0.01f;
	float m_Timer = 0.0f;

	GameData m_GameData;

	// Use this for initialization
	void Start () 
	{
		m_GameData = GameData.Instance;

		m_StreamPath = Environment.GetFolderPath (Environment.SpecialFolder.Desktop);
		m_StreamPath += "/ImagineNation_Recorded_Data";
		
		if (!Directory.Exists (m_StreamPath))
			Directory.CreateDirectory (m_StreamPath);

		switch (m_GameData.CurrentSection)
		{
			case Sections.Sections_1:
			m_StreamPath = m_StreamPath + "/SectionOne_PositionData";
			break;

			case Sections.Sections_2:
			m_StreamPath = m_StreamPath + "/SectionTwo_PositionData";
			break;

			case Sections.Sections_3:
			m_StreamPath = m_StreamPath + "/SectionThree_PositionData";
			break;
		}
		CheckIfFileExists ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		
		if (m_Timer <= 0.0f)
		{
			m_TargetPos.x = (int) m_Target.position.x;
			m_TargetPos.y = (int) m_Target.position.z;
			
			StreamWriter stream = new StreamWriter (m_StreamPath, true, System.Text.Encoding.GetEncoding("Windows-1252"));
			stream.Write (m_TargetPos.x);
			stream.Write ("\r\n");
			stream.Write (m_TargetPos.y);
			stream.Write ("\r\n");
			stream.Close ();
			
			m_NumbOfSamples++;
			m_Timer = m_SampleRate;
		}
		
		m_Timer -= Time.deltaTime;
	}


	void CheckIfFileExists()
	{
		if (File.Exists(m_StreamPath + ".txt"))
		{
			m_StreamPath += "1";
			CheckIfFileExists();
		}
		else
		m_StreamPath += ".txt";
	}
}
