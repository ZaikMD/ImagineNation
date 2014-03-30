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
		//PlayerInput.Instance.getEnviromentInteraction()
		if(obj.CompareTag("CrawlSpace") && Input.GetKeyDown(KeyCode.F))
		{
			CrawlSpaces crawlSpace = (CrawlSpaces)obj.GetComponent<CrawlSpaces>();
			if(crawlSpace != null)
			{
				crawlSpace.OnUse();
			}
		}
		else if(obj.name == "DivingBoard" && Input.GetKeyDown(KeyCode.F))
		{
			DivingBoard divingBoard = (DivingBoard)obj.GetComponent<DivingBoard>();
			if(divingBoard != null)
			{
				divingBoard.notifySeeSaw(this.gameObject);
				m_Movement.setCanMove(false);
			}
		}
	}
	
	void onCollionStay (Collision obj)
	{
		if(obj.collider.CompareTag("MoveableBlock") && Input.GetKeyDown(KeyCode.F))
		{
			MoveableBlock moveableBlock = (MoveableBlock)obj.collider.GetComponent<MoveableBlock>();
			if(moveableBlock != null)
			{
				moveableBlock.makeChild(gameObject);
				m_Movement.setCanMove(false);
			}
		}
	}
}
