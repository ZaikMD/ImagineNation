using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public abstract class BaseEnemy : MonoBehaviour, Attackable
{
    protected enum State
    {
        Default,
        Fight,
        Chase,
        Patrol,
        Idle,
    }

    //Boolean to check if the enemy is Alive
    protected bool m_IsAlive = true;
    //Boolean for checking inCombat
    protected bool m_InCombat = false;
    //Boolean to check if we are idling
    protected bool m_Idling = false;

    //Check to see what State the StateMachine is in
    protected State m_State;

    //Health for combat damage
    protected float m_Health = 1.0f;
    //Aggro range for distance when the enemy chases the player
    protected float m_AggroRange = 20.0f;
    //Combat range for how close the enemy has to be to hit the player
    protected float m_CombatRange = 0.0f;
    //Initial Stopping Distance of the NavMeshAgent
    protected float m_InitialStoppingDistance = 0.0f;

    //Attack Timer to delay enemy attacks
    protected float m_AttackTimer;

    //const to cap how long the enemy idles
    private const float MAX_IDLE_TIME = 10.0f;
    //Idle Timer to make Enemies seem organic
    protected float m_IdlingTime = MAX_IDLE_TIME;

    //Const for how long the enemy takes to get out of combat
    private const float EXIT_COMBAT_TIME = 1.0f;
    //Timer to track when the enemy leaves the player alone
    protected float m_CombatTimer = EXIT_COMBAT_TIME;

    //Array of GameObjects to track the players in the scene
    protected GameObject[] m_Players;

    //Single node for idleing
    public Transform m_IdleNode;
    //Array of Transform Path Nodes
    public Transform[] m_PathNodes;
    //Ability to set Max number of nodes
    public int m_MaxNodes = 3;
    //Count for Nodes to determine which node to go to	
    protected int m_NodeCount = 0;
    //Transform for the movement of the enemies target
    protected Transform m_Target;
    //Check if the enemy has reached a node
    protected bool m_ReachedNode = false;
    //Check if the enemy is between nodes
    protected bool m_MidNode = false;

    //NavMeshAgent for AI
    protected NavMeshAgent m_Agent;

    // Use this for initialization
    protected void Start()
    {
        //Find our NavMeshAgent
        m_Agent = this.gameObject.GetComponent<NavMeshAgent>();

		//Set speed of the enemy
		m_Agent.speed = 6.0f;

		//Find both the players based off their tag
		m_Players = GameObject.FindGameObjectsWithTag ("Player");

        //Set Initial Stopping Distance for the NavMeshAgent
        m_InitialStoppingDistance = m_Agent.stoppingDistance;

		//Set to make sure CombatRange does not exceed Aggro Range
        if (m_AggroRange <= m_CombatRange)
        {
            m_CombatRange = m_AggroRange;
        }

		//Set our current State for enemies
		m_State = State.Default;		
	}

	//public void load()
	//{
	//	//Find both the players based off their tag
	//	m_Players = GameObject.FindGameObjectsWithTag("Player");
	//}
	
	public void Reset()
    {
        m_InCombat = false;
        m_IsAlive = true;
        m_Target = null;
        m_CombatTimer = EXIT_COMBAT_TIME;
        m_State = State.Default;
    }

    // Update is called once per frame
    protected void Update()
    {
		if (m_Health <= 0.0f)
        {
            Die();
        }

		UpdateState ();
    }

	protected void UpdateState()
	{
		if (m_IsAlive)
		{
			switch (m_State)
			{
				case State.Default:
					Default();
					Debug.Log("Default");
					break;
				case State.Idle:
					Idle();
					Debug.Log("Idle");
					break;
				case State.Chase:
					Chase();
					Debug.Log("Chase");
					break;
				case State.Fight:
					Fight();
					Debug.Log("Fight");
					break;
				case State.Patrol:
					Patrol();
					Debug.Log("Patrol");
					break;
				default:
					break;
			}
		}
	}

    protected virtual void Default()
    {
        if (m_InCombat)
        {
            for (int i = 0; i < m_Players.Length; i++)
            {
                if (m_Players[i] != m_Target)
                {
                    float distance = Vector3.Distance(gameObject.transform.position, m_Players[i].transform.position);

                    if (GetDistanceToTarget() >= distance)
                    {
                        m_Target = m_Players[i].gameObject.transform;

                        if (GetDistanceToTarget() <= m_AggroRange)
                        {
                            if (GetDistanceToTarget() <= m_CombatRange)
                            {
                                m_State = State.Fight;

                                m_CombatTimer = EXIT_COMBAT_TIME;
                                return;
                            }
                            m_State = State.Chase;

                            m_CombatTimer = EXIT_COMBAT_TIME;
                            return;
                        }
                    }
                }
            }

            if (m_CombatTimer <= 0)
            {
                m_CombatTimer = EXIT_COMBAT_TIME;
                m_InCombat = false;
                m_State = State.Patrol;
            }
            else
            {
                m_CombatTimer -= Time.deltaTime;
                m_State = State.Default;
            }

        }

        for (int i = 0; i < m_Players.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, m_Players[i].transform.position);
            if (distance <= m_AggroRange)
            {
                m_InCombat = true;
                m_Target = m_Players[i].gameObject.transform;
                m_State = State.Default;
            }
            else
            {
                int idleRandom = Random.Range(1, 600);
                if (idleRandom == 1)
                {
                    m_Idling = true;
					m_Target = null;
                    m_State = State.Idle;
                }
                else
                {
					m_Target = null;
                    m_State = State.Patrol;
                }
            }
        }
    }

    //Idle state for when the enemy is standing still
    protected virtual void Idle()
    {
		if(m_Target == null)
		{
        	m_Target = m_IdleNode;
		}

        m_Agent.SetDestination(m_Target.position);

        if (m_IdlingTime <= 0)
        {
            m_IdlingTime = MAX_IDLE_TIME;
            m_Idling = false;
            m_State = State.Default;
        }
        else
        {
            m_IdlingTime -= Time.deltaTime;
			for (int i = 0; i < m_Players.Length; i++)
			{
				float distance = Vector3.Distance(transform.position, m_Players[i].transform.position);
				if (distance <= m_AggroRange)
				{
					m_InCombat = true;
					m_Target = m_Players[i].gameObject.transform;
					m_State = State.Default;
				}
			}

        }
    }

    //Enter Chase state for when the player aggros the enemy
    protected virtual void Chase()
    {
		m_Agent.speed = 15.0f;
        m_Agent.stoppingDistance = m_InitialStoppingDistance;
        if (m_Target != null)
        {
            m_Agent.SetDestination(m_Target.position);
        }

        m_State = State.Default;
    }

    //Patrol state when the enemy needs to patrol between nodes
    protected virtual void Patrol()
    {
        if (m_Players != null)
        {
            m_Agent.stoppingDistance = 0;
            if (m_Target == null)
            {
                m_Target = m_PathNodes[m_NodeCount].transform;
            }

            if (GetDistanceToTarget() <= 5.0f)
            {
                m_ReachedNode = true;
            }

            if (m_ReachedNode == true)
            {
                m_MidNode = false;
                m_NodeCount++;

                float randomSpeed = Random.Range(3.0f, 7.0f);
                m_Agent.speed = randomSpeed;
            }
            else
            {
                m_ReachedNode = false;
                m_MidNode = true;
            }

            if (m_MidNode == true)
            {
                if (m_Target != null)
                {
                    m_Agent.SetDestination(m_Target.position);
                }
            }
            else
            {
                if (m_NodeCount >= m_MaxNodes)
                {
                    m_NodeCount = 0;
                }

                m_Target = m_PathNodes[m_NodeCount].transform;
                m_MidNode = true;
                m_ReachedNode = false;
            }
        }

        m_State = State.Default;

    }

    //Returns the distance between the enemy and the Target
    protected float GetDistanceToTarget()
    {
        return Vector3.Distance(transform.position, m_Target.position);
    }

    //Functionality for if the enemy is hit by a player projectile
    public void onHit(PlayerProjectile proj)
    {
        m_Health--;   
    }

    //If Enemy gets hit by Enemy Projectile
    public void onHit(EnemyProjectile proj)
    {
        return;
    }

    public void SetIsAlive(bool isAlive)
    {
        m_IsAlive = isAlive;
    }

    public bool GetIsAlive()
    {
        return m_IsAlive;
    }

    //Attack state when the player is in range to hit
    protected abstract void Fight();

    //Die function will clear the enemy and instantiate rigidbody
    protected abstract void Die();

}
