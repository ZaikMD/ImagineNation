using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System;


public class CreateHeatMap : EditorWindow 
{
	string m_Path = "Select File";
	string m_SaveLoc = "Save Location";

	bool m_ValidPath = false;
	bool m_SavePath = false;

	int m_NumbOfPoints;
	
	Vector2[] m_Positions;
	
	int m_SmallestPosX = 0;
	int m_SmallestPosY = 0;
	
	int m_LargestPosX = 0;
	int	m_LargestPosY = 0;
	
	Texture2D m_Texture;
	
	double[,] m_PassCount;	

	//Will be 100% red
	int m_MostPassed;

	int m_TexSize = 500;

	[MenuItem ("Tools/CreateHeatMap")]
	public static void ShowWindow()
	{

		// Show the window
		EditorWindow.GetWindow(typeof(CreateHeatMap));
	}
	
	void OnGUI()
	{
		if (GUILayout.Button (m_Path))
		{
			m_Path = EditorUtility.OpenFilePanel ("Select The Text File", Environment.GetFolderPath (Environment.SpecialFolder.Desktop), "*.*");

			if (m_Path.Contains(".txt"))
			m_ValidPath = true;
		}

		if (GUILayout.Button (m_SaveLoc))
		{
			m_SaveLoc = EditorUtility.OpenFolderPanel ("Select Save Location", Environment.GetFolderPath (Environment.SpecialFolder.Desktop), "*.*");

			m_SavePath = true;
		}

		if (m_ValidPath && m_SavePath)
		{
			if (GUILayout.Button ("Create"))
			{	
				Read ();
				FindSmallestPosition ();
				FindLargestPosition ();
				CalculatePassCount ();
				CreateTexture ();
				CreateAndSavePNG ();
				Close ();
			}
		}
		
	}

	void Read()
	{
		m_NumbOfPoints = File.ReadAllLines(m_Path).Length/2;
		m_Positions = new Vector2[m_NumbOfPoints];

		StreamReader reader = new StreamReader(m_Path, System.Text.Encoding.GetEncoding("Windows-1252"));
		for (int i = 0; i < m_NumbOfPoints; i++)
		{
			m_Positions[i].x = int.Parse(reader.ReadLine());
			m_Positions[i].y = int.Parse(reader.ReadLine());
		}

		reader.Close ();
	}
	void FindSmallestPosition ()
	{
		for (int i = 0; i < m_NumbOfPoints; i++)
		{
			if (m_Positions[i].x < m_SmallestPosX)
				m_SmallestPosX = (int)m_Positions[i].x;

			if (m_Positions[i].y < m_SmallestPosY)
				m_SmallestPosY = (int)m_Positions[i].y;
		}

		m_SmallestPosX = Mathf.Abs (m_SmallestPosX);
		m_SmallestPosY = Mathf.Abs (m_SmallestPosY);

		for (int i = 0; i < m_NumbOfPoints; i++)
		{
			m_Positions[i].x += m_SmallestPosX;
			m_Positions[i].y += m_SmallestPosY;
		}
	}

	void FindLargestPosition ()
	{
		for (int i = 0; i < m_NumbOfPoints; i++)
		{
			if (m_Positions[i].x > m_LargestPosX)
				m_LargestPosX = (int)m_Positions[i].x;
			
			if (m_Positions[i].y > m_LargestPosY)
				m_LargestPosY = (int)m_Positions[i].y;
		}
	}

	void CalculatePassCount ()
	{
		m_PassCount = new double[m_NumbOfPoints,m_NumbOfPoints];

		for (int x = 0; x < m_NumbOfPoints; x++)
		{
			for (int y = 0; y < m_NumbOfPoints; y++)
			{
				m_PassCount[x,y] = 0;
			}
		}

		for (int x = 0; x < m_NumbOfPoints; x++)
		{
			for (int y = 0; y < m_NumbOfPoints; y++)
			{
				for (int i = 0; i < m_NumbOfPoints; i++)
				{
					if ((int) m_Positions[i].x == x && (int) m_Positions[i].y == y)
						m_PassCount[x,y] += 1;
				}
			}
		}

		for (int x = 0; x < m_NumbOfPoints; x++)
		{
			for (int y = 0; y < m_NumbOfPoints; y++)
			{
				if (m_PassCount[x,y] > m_MostPassed)
					m_MostPassed = (int)m_PassCount[x,y];
			}
		}
	}

	void CreateTexture ()
	{
		int cellSizeX = m_TexSize / m_LargestPosX;
		int cellSizeY = m_TexSize / m_LargestPosY;

		m_Texture = new Texture2D (m_TexSize, m_TexSize, TextureFormat.RGB24, false);
		Color white = new Color (1.0f, 1.0f, 1.0f);

		for (int x = 0; x < m_LargestPosX; x++)
		{
			for (int y = 0; y < m_LargestPosY; y++)
			{
				if (m_PassCount[x,y] == 0)
				{
					for (int i = x*cellSizeX; i < cellSizeX*(x+1); i++)
					{
						for (int j = y*cellSizeY; j < cellSizeY*(y+1); j++)
						{
							m_Texture.SetPixel(i,j,white);
						}
					}
				}
				
				else 
				{
					int countNumb = (int) m_PassCount[x,y];
					float Redperc =(float) countNumb/ (float) m_MostPassed;
					float BluePerc = 1.0f - Redperc;
					Color col = new Color(Redperc, 0.0f, BluePerc);
					
					for (int i = x*cellSizeX; i < cellSizeX*(x+1); i++)
					{
						for (int j = y*cellSizeY; j < cellSizeY*(y+1); j++)
						{								
							m_Texture.SetPixel(i,j, col);
						}
					}
				}
			}
		}
	}

	void CreateAndSavePNG ()
	{
		m_SaveLoc += "/HeatMap";
		CheckIfFileExists ();

		Byte[] pngBytes = m_Texture.EncodeToPNG ();
		File.WriteAllBytes (m_SaveLoc, pngBytes);

	}

	void CheckIfFileExists()
	{
		if (File.Exists(m_SaveLoc + ".png"))
		{
			m_SaveLoc += "1";
			CheckIfFileExists();
		}
		else
			m_SaveLoc += ".png";
	}
}
