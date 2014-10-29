using UnityEngine;
using System.Collections;

/*Created by Kole
 * 
 * Light peg will act simularly to coins in mario
 * When a peg is Collected, it will increment the total  
 *  
 */

[RequireComponent(typeof(CharacterController))]

public class LightPeg : BaseCollectable
{
  //  public  int m_ID;

    CharacterController m_Controller;

    LightPegManager m_LightPegManager;

    // Use this for initialization
    void Start()
    {
        m_Controller = gameObject.GetComponent<CharacterController>();
        m_LightPegManager = GameObject.FindGameObjectWithTag(Constants.COLLECTABLE_MANAGER).GetComponent<LightPegManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //This will apply gravity for us
        Vector3 speed = Vector3.zero;
        m_Controller.SimpleMove(speed);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == Constants.PLAYER_STRING)
        {
            //Tell GameData this peg was collected
            GameData.Instance.LightPegCollected(m_ID);

            //increment collectable counter
            m_LightPegManager.IncrementCounter();

            //destroy this gameobject
            Destroy(this.gameObject);            
        }
    }
 

}