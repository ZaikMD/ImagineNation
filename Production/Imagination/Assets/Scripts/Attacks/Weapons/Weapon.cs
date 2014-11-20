using UnityEngine;
using System.Collections;
/// <summary>
/// Combat item.
/// Created by Zach Dubuc
/// 
/// Handles the combos for players, and calls the attack function from the attacks
/// 
/// </summary>
public class Weapon : MonoBehaviour
{
    SFXManager m_SFX;
    AnimationState m_AnimState;

    BaseAttack m_CurrentAttack;//The current Attack
    BaseAttack m_PreviousAttack;

    public GameObject m_LightProjectilePrefab;
    public GameObject m_HeavyProjectilePrefab;

    string m_Inputs;
    //The constants for the inputs of the attacks, as well as the 
    //combos the players can do. L = light attack  H = Heavy attack
    const string L = " L";
    const string LL = " L L";
    const string DOUBLEHIT = " L L L";
    const string LH = " L H";
    const string AOE = " L L H";
    const string FRONTLINE = " L H H";
    const string H = " H";
    const string HH = " H H";
    const string FRONTCONE = " H H H";

    string m_LastInput;

    ArrayList m_AttackList = new ArrayList();

    AcceptInputFrom m_ReadInput;
	// Use this for initialization
	void Start () 
    {
        m_ReadInput = gameObject.GetComponent<AcceptInputFrom>();

        m_SFX = GameObject.FindGameObjectWithTag(Constants.SOUND_MANAGER).GetComponent<SFXManager>();
        m_AnimState = GetComponent<AnimationState>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (InputManager.getAttackDown(m_ReadInput.ReadInputFrom))
        {
            m_LastInput = "";
            m_Inputs += L;
            m_LastInput += L;
        }

        updateAttacks();
	}

    void setCurrentAttack()
    {

        m_PreviousAttack = m_CurrentAttack;
        switch (m_Inputs)
        {
            case L:
                {
                    LightAttack attack = new LightAttack();
                    attack.loadPrefab(m_LightProjectilePrefab);
                    attack.startAttack(transform.position, transform.rotation);
                    m_CurrentAttack = attack;
                }

                break;

            case LL:
                {
                    LightAttack attack = new LightAttack();
                    attack.loadPrefab(m_LightProjectilePrefab);
                    attack.startAttack(transform.position, transform.rotation);
                    m_CurrentAttack = attack;
                }
                break;

            case DOUBLEHIT:
                {

                }
                break;

            case LH:
                {
                    HeavyAttack attack = new HeavyAttack();
                    attack.loadPrefab(m_HeavyProjectilePrefab);
                    attack.startAttack(transform.position, transform.rotation);
                    m_CurrentAttack = attack;
                }
                break;

            case AOE:
                {

                }
                break;

            case FRONTLINE:
                {
                }
                break;

            case H:
                {
                    HeavyAttack attack = new HeavyAttack();
                    attack.loadPrefab(m_HeavyProjectilePrefab);
                    attack.startAttack(transform.position, transform.rotation);
                    m_CurrentAttack = attack;
                }
                break;

            case HH:
                {
                    HeavyAttack attack = new HeavyAttack();
                    attack.loadPrefab(m_HeavyProjectilePrefab);
                    attack.startAttack(transform.position, transform.rotation);
                    m_CurrentAttack = attack;
                }
                break;

            case FRONTCONE:
                {

                }
                break;

            default:
                {
                    m_Inputs = m_LastInput;
                }
                break;
        }
    }

    void updateAttacks()
    {
        if (m_PreviousAttack != null)
        {
            if (!m_PreviousAttack.getAttacking()) //Check if the character is attacking
            {
                if (m_PreviousAttack.getGraceTimer() <= 0.0f) //Check if the grace timer is over
                {
                    m_CurrentAttack = null; //If so, the combo gets reset
                    m_PreviousAttack.resetGraceTimer();
                }


                else
                {
                    setCurrentAttack();
                    // PlaySoundAndAnim();
                }
            }

            m_CurrentAttack.Update();
            m_PreviousAttack.Update();
        }

        else
        {
            setCurrentAttack();
        }
    }
}
