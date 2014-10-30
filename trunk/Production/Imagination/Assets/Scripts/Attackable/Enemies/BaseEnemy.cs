using UnityEngine;
using System.Collections;

/*
 * Created by Joe Burchill
 * Date: Sept, 26, 2014
 *  
 * This script handles all the base functionality for each enemy.
 * It contains a state machine that uses NavMesh to simulate an organic
 * AI. Any Enemy in our game uses this base state machine and modifies,
 * specific states that are unique to the enemy.
 * 
 */
#region ChangeLog
/* 
 * Changed some Magic Numbers, and Adjusted Speeds for Base - Joe Burchill Oct. 16, 2014
 * Added else statment to default state, fixed combat state bool - Joe Burchill Oct. 29, 2014
 * 
 */
#endregion


//Require our NavMeshAgent for EnemyAI
[RequireComponent(typeof(NavMeshAgent))]
public abstract class BaseEnemy : Destructable
{
	//Enum for Enemy States
    protected enum State
    {
        Default,
        Idle,
        Fight,
        Chase,
        Patrol,
    }

	//Constant for our Minimum and Max random idle number
	private const int IDLE_MIN_RANGE = 1;
	private const int IDLE_MAX_RANGE = 600;

	//Differing Speeds for enemy states
	private const float MIN_PATROL_SPEED = 3.0f;
	private const float MAX_PATROL_SPEED = 5.0f;
	private const float CHASE_SPEED = 5.0f;
	private const float STARTING_SPEED = 6.0f;

	//Const for Reaching a Node distance
	private const float REACHED_NODE_DISTANCE = 2.0f;

    //Boolean to check if the enemy is Alive
    protected bool m_IsAlive = true;
    //Boolean for checking inCombat
    protected bool m_InCombat = false;
    //Boolean to check if we are idling
    protected bool m_Idling = false;
	//Boolean to check if enemy is Active
	protected bool m_IsActive = true;

    //Check to see what State the StateMachine is in
    protected State m_State;

    //Aggro range for distance when the enemy chases the player
    protected float m_AggroRange = 20.0f;
    //Combat range for how close the enemy has to be to hit the player, differs for enemies
    protected float m_CombatRange;
    //Initial Stopping Distance of the NavMeshAgent
    protected float m_InitialStoppingDistance;
    private const float STARTING_STOPPING_DISTANCE = 2.5F;

    //Attack Timer to delay enemy attacks
    protected float m_AttackTimer;

    //const to cap how long the enemy idles
    private const float MAX_IDLE_TIME = 3.0f;
    //Idle Timer to make Enemies seem organic
    protected float m_IdlingTime = MAX_IDLE_TIME;

    //Const for how long the enemy takes to get out of combat
    private const float EXIT_COMBAT_TIME = 2.5f;
    //Timer to track when the enemy leaves the player alone
    protected float m_CombatTimer = EXIT_COMBAT_TIME;

    //Array of GameObjects to track the players in the scene
    protected GameObject[] m_Players;

    //Single node for idleing
    public Transform IdleNode;
    //Array of Transform Path Nodes
    public Transform[] PathNodes;
    //Ability to set Max number of nodes
    public int NodeCount = 3;
    //Count for Nodes to determine which node to go to	
    protected int m_NodeIndex = 0;
    //Transform for the movement of the enemies target
    protected Transform m_Target;
    //Check if the enemy has reached a node
    protected bool m_ReachedNode = false;
    //Check if the enemy is between nodes
    protected bool m_MidNode = false;

    //NavMeshAgent for AI
    protected NavMeshAgent m_Agent;

    //Array of Enemies to handle Group functionality
    public BaseEnemy[] GroupOfEnemies;

    // Use this for initialization
    protected void Start()
    {
        //Find our NavMeshAgent
        m_Agent = this.gameObject.GetComponent<NavMeshAgent>();

		//Set speed of the enemy
		m_Agent.speed = STARTING_SPEED;

		//Find both the players based off their tag
		m_Players = GameObject.FindGameObjectsWithTag (Constants.PLAYER_STRING);

        //Set Starting Stopping Distance
        m_Agent.stoppingDistance = STARTING_STOPPING_DISTANCE;

        //Set Initial Stopping Distance for the NavMeshAgent
        m_InitialStoppingDistance = m_Agent.stoppingDistance;
        
		//Set to make sure CombatRange does not exceed Aggro Range
        if (m_AggroRange < m_CombatRange)
        {
            m_CombatRange = m_AggroRange;
        }

		//Set our current State for enemies
		m_State = State.Default;		
	}
	
	public void Reset()
    {
        //Reset Function to bring the enemy back to a default state
        m_InCombat = false;
        m_IsAlive = true;
		m_IsActive = true;
        m_Target = null;
        m_CombatTimer = EXIT_COMBAT_TIME;
        m_State = State.Default;
    }

    // Update is called once per frame
    protected void Update()
    {
        if (m_IsActive == false)
        {
            DisableEnemy();
        }

		if (m_Health <= 0) 
		{
			onDeath();
		}

        //Call our Update State function
		UpdateState ();

		FindPlayer ();

		base.Update ();
    }

	protected void FindPlayer ()
	{
		m_Players = GameObject.FindGameObjectsWithTag (Constants.PLAYER_STRING);
	}

	protected void UpdateState()
	{
		//Check if our enemy is active
		if(m_IsActive)
		{
  	      	//Check if Enemy is alive then run our State Machine
			if (m_IsAlive)
			{
  	          //Case for each State the Enemy can be in, and calls
  	          //the corresponding function for each state
				switch (m_State)
				{
					case State.Default:
						Default();
						break;
					case State.Idle:
						Idle();
						break;
  	              case State.Fight:
  	                  Fight();
  	                  break;
					case State.Chase:
						Chase();
  	                  break;
					case State.Patrol:
						Patrol();
						break;
					default:
						break;
				}
			}
		}
	}

    protected virtual void Default()
    {
        //Check if the enemy is in combat
        if (m_InCombat)
        {
			if (m_Target == null)
			{
				Reset();
				return;
			}

            //Cycle through our enemy if it is considered a group
            for (int enemyIndex = 0; enemyIndex < GroupOfEnemies.Length; enemyIndex++)
            {
                //Check if the elements of the group array are null
                if (GroupOfEnemies[enemyIndex] != null)
                {
                    //Cycle through our Player objects
                    for (int i = 0; i < m_Players.Length; i++)
                    {
                        //Check if our player is the target or not
                        if (m_Players[i] != m_Target)
                        {
                            //If the player isnt the target, check the distance between the enemy and the player
                            float distance = Vector3.Distance(gameObject.transform.position, m_Players[i].transform.position);
                            
                            //Check the distance against the enmy target
                            if (GetDistanceToTarget() >= distance)
                            {
                                //If the target is greater or equal to the distance of the player, set our target to the player
                                m_Target = m_Players[i].gameObject.transform;

                                //Check if the target is within aggro range
                                if (GetDistanceToTarget() <= m_AggroRange)
                                {
                                    //Check Combat Range of the target
                                    if (GetDistanceToTarget() <= m_CombatRange)
                                    {
                                        //Enter fight state
                                        GroupOfEnemies[enemyIndex].SetState(State.Fight);

                                        //Reset combat timer
                                        m_CombatTimer = EXIT_COMBAT_TIME;
                                        return;
                                    }
                                    //If the target is it will set the target to the player for every enemy in the group
									GroupOfEnemies[enemyIndex].SetTarget(m_Players[i].gameObject.transform);
                                    //This will set the state Chase to each enemy in the group, so if you aggro one
                                    //You aggro them all
									GroupOfEnemies[enemyIndex].SetState(State.Chase);
                                    //Reset our Combat Timer cause we are remaining in combat
                                    m_CombatTimer = EXIT_COMBAT_TIME;  
                                }
                            }
                        }
                    }
                }
            }

            //Loop through our PlayerObjects
            for (int i = 0; i < m_Players.Length; i++)
            {
                //Check if the Player isn't our target
                if (m_Players[i] != m_Target)
                {
                    //Check distance between Enemy and Player
                    float distance = Vector3.Distance(gameObject.transform.position, m_Players[i].transform.position);

                    //Check the distance to our target is greater than our distance
                    if (GetDistanceToTarget() >= distance)
                    {
                        //Set our target to the player
                        m_Target = m_Players[i].gameObject.transform;

                        //Check aggro range of the Target
                        if (GetDistanceToTarget() <= m_AggroRange)
                        {
                            //Check Combat Range of the target
                            if (GetDistanceToTarget() <= m_CombatRange)
                            {
                                //Enter fight state
                                m_State = State.Fight;

                                //Reset combat timer
                                m_CombatTimer = EXIT_COMBAT_TIME;
                                return;
                            }
                            //Enter Chase state
                            m_State = State.Chase;

                            //Reset combat timer
                            m_CombatTimer = EXIT_COMBAT_TIME;
                            return;
                        }
                    }
                }
            }

            //Check if the Combat Timer has reached 0 or less than 0
            if (m_CombatTimer <= 0)
            {
                //Reset Timer
                m_CombatTimer = EXIT_COMBAT_TIME;
                //Set Enemy out of Combat
                m_InCombat = false;
                //Enemy back to patrol
                m_State = State.Patrol;
            }
            else
            {
                //Decrement timer by deltaTime
                m_CombatTimer -= Time.deltaTime;
                //Set back to default
                m_State = State.Default;
            }

        }
		else
		{
        	//Loop through player objects
        	for (int i = 0; i < m_Players.Length; i++)
        	{
        	    //Check distance from the enemy to the player
        	    float distance = Vector3.Distance(transform.position, m_Players[i].transform.position);
        	    //Check if that distance is within aggro range
        	    if (distance <= m_AggroRange)
        	    {
        	        //Set Combat to true, the target to the player and return to default
        	        m_InCombat = true;
        	        m_Target = m_Players[i].gameObject.transform;
        	        m_State = State.Default;
        	    }
        	    else
        	    {
        	        //Get a random number from 1 to 600
        	        int idleRandom = Random.Range(IDLE_MIN_RANGE, IDLE_MAX_RANGE);
        	        //Check if it is 1
        	        if (idleRandom == IDLE_MIN_RANGE)
        	        {
        	            //Set idling to true, clear target, and set to idle
        	            m_Idling = true;
						m_Target = null;
        	            m_State = State.Idle;
        	        }
        	        else
        	        {
        	            //Clear target, and set state to Patrol, if the random number is not 1
						m_Target = null;
        	            m_State = State.Patrol;
        	        }
        	    }
        	}
		}
    }

    //Idle state for when the enemy is standing still
    protected virtual void Idle()
    {
        //If target is null set it to the Idle Node
		if(m_Target == null)
		{
        	m_Target = IdleNode;
		}

        //Set Nav Mesh destination to the target
        m_Agent.SetDestination(m_Target.position);

        //If we have reached our target enter this statement
        if (GetDistanceToTarget() <= REACHED_NODE_DISTANCE)
        {
            //Check Idle Timer
            if (m_IdlingTime <= 0)
            {
                //Reset timer, set idling to false and return to default
                m_IdlingTime = MAX_IDLE_TIME;
                m_Idling = false;
                m_State = State.Default;
            }
            else
            {
                //Decrement timer, so it is still idling
                m_IdlingTime -= Time.deltaTime;
                //Check for any Player Aggro when idling
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
    }

    //Enter Chase state for when the player aggros the enemy
    protected virtual void Chase()
    {
        //Speed the Enemy up when chasing
		m_Agent.speed = CHASE_SPEED;
        //Set Stopping Distance back to initial so it doesnt stop on top of player
        m_Agent.stoppingDistance = m_InitialStoppingDistance;

        //If target isn't null it will set destination
        if (m_Target != null)
        {
            m_Agent.SetDestination(m_Target.position);
        }

        //Return to default
        m_State = State.Default;
    }

    //Patrol state when the enemy needs to patrol between nodes
    protected virtual void Patrol()
    {
        //Check if the players array isn't null
        if (m_Players != null)
        {
            //Set stopping Distance to reach node without stopping short
            m_Agent.stoppingDistance = 0;

            //If the target is null it will set the Patrol Path Node
            if (m_Target == null)
            {
                m_Target = PathNodes[m_NodeIndex].transform;
            }

            //Check if you have reached the node, set boolean if so
			if (GetDistanceToTarget() <= REACHED_NODE_DISTANCE)
            {
                m_ReachedNode = true;
            }

            //Check if enemy has reached the node
            if (m_ReachedNode == true)
            {
                //We are no longer mid node, increment our index
                m_MidNode = false;
                m_NodeIndex++;

                //Randomize enemy speed between 3 and 7 after reaching each node, organic feel
                float randomSpeed = Random.Range(MIN_PATROL_SPEED, MAX_PATROL_SPEED);
                m_Agent.speed = randomSpeed;
            }
            else
            {
                //Haven't reached node, still mid node
                m_ReachedNode = false;
                m_MidNode = true;
            }

            //Check if we are mid node, if we are and have a target set the destination
            if (m_MidNode == true)
            {
                if (m_Target != null)
                {
                    m_Agent.SetDestination(m_Target.position);
                }
            }
            else
            {
                //If we reach the end of our array, the last patrol node, set it back to 0
                if (m_NodeIndex >= NodeCount)
                {
                    m_NodeIndex = 0;
                }

                //Set target to patrol path node, we are now mid node
                m_Target = PathNodes[m_NodeIndex].transform;
                m_MidNode = true;
                m_ReachedNode = false;
            }
        }

        //Return to Default
        m_State = State.Default;

    }

    //Returns the distance between the enemy and the Target
    protected float GetDistanceToTarget()
    {
        //Returns the distance between the enemy and the target
        return Vector3.Distance(transform.position, m_Target.position);
    }
    
    //Functionality for if the enemy is hit by a player projectile
    public override void onHit(PlayerProjectile proj)
    {
		base.onHit (proj); 
    }

    //If Enemy gets hit by Enemy Projectile
    public override void onHit(EnemyProjectile proj)
    {
        base.onHit(proj);
    }

    //If our enemy dies, calls abstract die function for inheritance
    protected override void onDeath()
    {
        Die();
		m_IsActive = false;
		m_IsAlive = false;
     	
		base.onDeath ();
    }

    //Set whether the enemy is alive or not
    public void SetIsAlive(bool isAlive)
    {
        m_IsAlive = isAlive;
    }

    //Get function to check IsAlive boolean
    public bool GetIsAlive()
    {
        return m_IsAlive;
    }

	public void SetIsActive(bool isActive)
	{
		m_IsActive = isActive;
	}

	public bool GetIsActive()
	{
		return m_IsActive;
	}

    protected void DisableEnemy()
    {
        this.gameObject.SetActive(false);
    }

    //Set aggroRange for custom areas within the game
    public void SetAggroRange(float range)
    {
        m_AggroRange = range;
    }

    //Ability to set state
    protected void SetState(State state)
    {
        m_State = state;
    }

    //Set target for the enemy
	protected void SetTarget(Transform target)
	{
		m_Target = target;
	}

    //Attack state when the player is in range to hit
    protected abstract void Fight();

    //Die function will clear the enemy and instantiate rigidbody
    protected abstract void Die();

}
