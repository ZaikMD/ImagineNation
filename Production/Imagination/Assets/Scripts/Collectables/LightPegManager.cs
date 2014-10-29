using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LightPegManager : MonoBehaviour {

    public GameObject m_LightPegPrefab;

    public GameObject[] m_LightPegsForCheckPointOne;
    public GameObject[] m_LightPegsForCheckPointTwo;
    public GameObject[] m_LightPegsForCheckPointThree;
    public GameObject[] m_LightPegsForCheckPointFour;

    bool[] m_LightPegCollected;

    const float OnScreenTime = 3;

    bool m_DisplayCounter;
    float m_Timer;
    int m_NumberOfLightPegsCollect;

	// Use this for initialization
	void Start ()
    {
        m_Timer = OnScreenTime;
        m_NumberOfLightPegsCollect = 0;

        int lengthOfLightPegCollected = m_LightPegsForCheckPointOne.Length + m_LightPegsForCheckPointTwo.Length + m_LightPegsForCheckPointThree.Length + m_LightPegsForCheckPointFour.Length;

        m_LightPegCollected = new bool[lengthOfLightPegCollected];

        SpawnLightPegs();
	}
	
    // Update is called once per frame
	void Update () 
    {
        if (m_Timer > 0)
        {
            m_Timer -= Time.deltaTime;
        }
        else
        {
            m_DisplayCounter = false;
        }
    }

    void SpawnLightPegs()
    {
        if (GameData.Instance.FirstTimePlayingLevel)
        {
            //first time the level is being played
            //We need to set gamedatas light pegs collected to all false
            GameData.Instance.SetCollectedPegs(m_LightPegCollected.Length);
        }
        else
        { 
            //Played this level before
            m_LightPegCollected = GameData.Instance.CollectedLightPegs();
        }
        
        //Check which CheckPoint we are starting at
        switch(GameData.Instance.CurrentCheckPoint)
        {
            case CheckPoints.CheckPoint_1:
            //Loop through all list and spawn a new light peg

            //Set counter to 0
            m_NumberOfLightPegsCollect = 0;
            break;
            
            case CheckPoints.CheckPoint_2:
            //loop through all after check point two and spawn, check if collected for checkpoint one
            //set counter to number of pegs collect in list for checkpoint one
            m_NumberOfLightPegsCollect = 0;

            for(int i = 0; i < m_LightPegsForCheckPointOne.Length; i++)
            {
                if (m_LightPegCollected[i])
                {
                    //This light peg is collected
                }
                else
                { 
                   //this light peg is not collected 
                }           
            }
                break;


            case CheckPoints.CheckPoint_3:
            //loop through all after check point three and spawn, check if collected for checkpoint one and two
            //set counter to number of pegs collect in list for checkpoint one and two
            break;
        
        }


        //Cycle throught our list and spawn appropriate

    }

    public void IncrementCounter()
    {
        m_NumberOfLightPegsCollect ++;
        DisplayCounter();
    }

    void DisplayCounter()
    {
        m_DisplayCounter = true;
        m_Timer = OnScreenTime;
    }

    void OnGUI()
    {
        if (m_DisplayCounter)
        { 
            //Show the Number of pegs collected
        }
    }
}
