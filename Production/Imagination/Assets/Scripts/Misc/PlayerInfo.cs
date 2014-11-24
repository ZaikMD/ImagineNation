using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInfo : MonoBehaviour 
{
    static List<PlayerInfo> m_Players = new List<PlayerInfo>();
    public static List<PlayerInfo> PlayerList
    {
        get { return m_Players; }
    }

    public Characters i_Character;
    public Players m_Player { get; protected set; }

    void Awake()
    {
        m_Players.Add(this);
    }

    void Start()
    {
        if (i_Character == GameData.Instance.PlayerOneCharacter)
        {
            m_Player = Players.PlayerOne;
        }
        else if (i_Character == GameData.Instance.PlayerTwoCharacter)
        {
            m_Player = Players.PlayerTwo;
        }
        else
        {
            //error
        }
    }

    void OnDestroy()
    {
        for (int i = 0; i < m_Players.Count; i++)
        {
            if (m_Players[i] == this)
            {
                m_Players.RemoveAt(i);
                break;
            }
        }
    }

    public static PlayerInfo getPlayer(Players player)
    {
        for (int i = 0; i < m_Players.Count; i++)
        {
            if (m_Players[i].m_Player == player)
                return m_Players[i];
        }
        //error
        return null;
    }

    public static PlayerInfo getCharacter(Characters character)
    {
        for (int i = 0; i < m_Players.Count; i++)
        {
            if (m_Players[i].i_Character == character)
                return m_Players[i];
        }
        //error
        return null;
    }
}
