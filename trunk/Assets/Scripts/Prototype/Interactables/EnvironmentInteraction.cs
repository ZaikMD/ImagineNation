/*

Already instantiated by attaching PlayerMovement

Created by Zach

3/29/2014
	Changed script to enviromental interaction.
	Now works with player movement.
	Now works with blocks.
	Is now instantiated by Player movement.
*/




using UnityEngine;
using System.Collections;

public class EnvironmentInteraction : MonoBehaviour
{
	PlayerMovement m_Movement;
	
	void Start ()
	{
		m_Movement = gameObject.GetComponent<PlayerMovement> ();
	}
	
	void OnTriggerStay(Collider obj)
	{
		//Start Interaction
		if (PlayerInput.Instance.getEnviromentInteraction())
		{
			//PlayerInput.Instance.getEnviromentInteraction()
			if(obj.CompareTag("CrawlSpace"))
			{
				if (gameObject.name == "Derek")
				{
					return;
				}
				
				CrawlSpaces crawlSpace = (CrawlSpaces)obj.GetComponent<CrawlSpaces>();
				if(crawlSpace != null)
				{
					//crawlSpace.OnUse();
				}
			}
			else if(obj.name == "DivingBoard")
			{
				DivingBoard divingBoard = (DivingBoard)obj.GetComponent<DivingBoard>();
				if(divingBoard != null)
				{
					divingBoard.notifySeeSaw(this.gameObject);
					m_Movement.setCanMove(false);
				}
			}
			else if(obj.CompareTag("MoveableBlock"))
			{
				if (obj.transform.parent != null)
				{
					return;
				}
				
				MoveableBlock moveableBlock = (MoveableBlock)obj.GetComponent<MoveableBlock>();
				if(moveableBlock != null)
				{
					moveableBlock.makeChild(gameObject);
				}
			}
		}
	}
}
