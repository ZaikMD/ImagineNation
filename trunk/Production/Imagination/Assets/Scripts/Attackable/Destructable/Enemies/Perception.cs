using UnityEngine;
using System.Collections;

public class Perception : MonoBehaviour
{


    //<====================================================================== NEED A LAYER FOR ENEMIES TO MAKE SURE WE DONT BLOCK THE RAY CAST WITH ANOTHER ENEMY
	//<====================================================================== THIS SCRIPT IS MOSTLY UNTESTED

    /// <summary>
    /// how are the methods of detection weighted
    /// only Field Of View, Distance = 0
    /// only Residual Threat = 1
    /// </summary>
    [Range(0.0f, 1.0f)]
    public float i_FOVD_RT = 0.5f;

	public float i_FieldOfView = 90.0f;
	public float i_ViewDist = 10.0f;

    /// <summary>
    /// whats the threshold where the player is guarenteed to be seen
    /// </summary>
	public float i_PerfectVisionPercent = 0.7f;

    /// <summary>
    /// how Long it takes before the player is considered completely unseen
    /// assuming the player deosnt re-enter sight
    /// </summary>
    public float i_ResidualThreatTimerSight = 5.0f;

    public float i_ResidualThreatTimerAttacks = 5.0f;

    float i_ThreatThreshold = 0.65f;

    Perception_Player[] m_Players;

    void Start()
    {
		string test = gameObject.name;
        m_Players = new Perception_Player[PlayerInfo.PlayerList.Count];
        for (int i = 0; i < m_Players.Length; i++)
        {
            m_Players[i] = new Perception_Player(PlayerInfo.PlayerList[i]);
        }
    }

    void Update()
    {

        if (PauseScreen.IsGamePaused) { return; }

        for (int i = 0; i < m_Players.Length; i++)
        {
            m_Players[i].update();
        }
    }

	public PlayerInfo getHighestThreatPlayer()
	{
		int highestThreat = 0;
		float threat = getThreat(m_Players[highestThreat]);

		for (int i = 1; i < m_Players.Length; i++)
		{
			float temp = getThreat(m_Players[i]);
			if(temp> threat)
			{
				threat = temp;
				highestThreat = i;
			}
		}
		return  m_Players [highestThreat].PlayerInfo;
	}

    public float getThreat(Perception_Player player)
    {
        if (player.AttackedUs)
        {
            return (player.AttackedUsTimer / i_ResidualThreatTimerAttacks) + (getDistanceThreat(player) * i_FOVD_RT) + (getResidualDistanceThreat(player) * (1.0f - i_FOVD_RT));
        }

		return (getDistanceThreat(player) * (1.0f - i_FOVD_RT)) + (getResidualDistanceThreat(player) * i_FOVD_RT);
		//return (getDistanceThreat (player) * i_FOVD_RT);
		//return (getResidualDistanceThreat(player) * (1.0f - i_FOVD_RT));
    }

    float getDistanceThreat(Perception_Player player)
    {
        //distance to player
        float distance = (transform.position - player.Player.position).magnitude;

        //is player out of view distance?
        if (distance > i_ViewDist)
        {
            return 0.0f;
        }

        // is the player behind something?
        RaycastHit raycastData;
        if (!Physics.Raycast(transform.position, player.Player.position - transform.position, out raycastData, distance +1.0f, ~LayerMask.GetMask("Enemy")))// TODO: replace with constant
        {
			Debug.DrawLine(transform.position,player.Player.position - transform.position, Color.red);
            return 0.0f;
        }
        else
        {
            //did we hit the player?
            if (raycastData.collider.tag != Constants.PLAYER_STRING)
            {
				Debug.DrawLine(transform.position,player.Player.position - transform.position, Color.red);
                return 0.0f;
            }
        }

        //in the field of view?
        float dot = Vector3.Dot(transform.forward.normalized, (player.Player.position - transform.position).normalized);
        float angle = Mathf.Acos(dot / (transform.forward.normalized.magnitude * (player.Player.position - transform.position).normalized.magnitude)) * 100.0f;

        if (i_FieldOfView < angle)
        {
            return 0.0f;
        }

        //we can see the player
        player.playerSeen(i_ResidualThreatTimerSight);

        return Mathf.Clamp(((i_ViewDist * i_PerfectVisionPercent) / distance), 0.0f, i_FOVD_RT);
    }

    float getResidualDistanceThreat(Perception_Player player)
    {
        return player.SawPlayerTimer / i_ResidualThreatTimerSight;
    }

	public void attackedUs(PlayerInfo player)
	{
		for (int i = 0; i < m_Players.Length; i++)
		{
			if(m_Players[i].PlayerInfo == player)
			{
				m_Players[i].attackedUs(i_ResidualThreatTimerAttacks);
				break;
			}
		}
	}
}

public class Perception_Player
{
    float m_SawPlayerTimer = 0.0f;
    public float SawPlayerTimer
    {
        get { return m_SawPlayerTimer; }
    }

    bool m_AttackedUs = false;
    public bool AttackedUs
    {
        get { return m_AttackedUs; }
    }

    float m_AttackedUsTimer = 0.0f;
    public float AttackedUsTimer
    {
        get { return m_AttackedUsTimer; }
    }

    PlayerInfo m_Player;
    public Transform Player
    {
		get { return m_Player.transform.FindChild("\"Centre Point\""); }
    }

	public PlayerInfo PlayerInfo
	{
		get { return m_Player; }
	}

	public Perception_Player(PlayerInfo player)
	{
        m_Player = player;
	}

    public void playerSeen(float timerLength)
    {
        if (timerLength > m_SawPlayerTimer)
        {
            m_SawPlayerTimer = timerLength;
        }
    }

    public void attackedUs(float timerLength)
    {
        m_AttackedUs = true;
        if (timerLength > m_AttackedUsTimer)
        {
            m_AttackedUsTimer = timerLength;
        }
    }

    public void update()
    {
        if (m_SawPlayerTimer > 0.0f)
            m_SawPlayerTimer -= Time.deltaTime;

        if (!m_AttackedUs) 
            return;

        if (m_AttackedUsTimer > 0.0f)
        {
            m_AttackedUsTimer -= Time.deltaTime;
        }
        else
        {
            m_AttackedUs = false;
        }
    }
}
