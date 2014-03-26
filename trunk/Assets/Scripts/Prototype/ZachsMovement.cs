using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]

public class ZachsMovement : MonoBehaviour {
	
	bool m_InteractionKeyPressed;
	GameObject m_Interactable;
	public float movementSpeed = 5.0f;
	public float MaxSpeed = 10.0f;
	public float turnSpeed = 10.0f;
	public float jumpSpeed = 5.0f;
	public float strafeSpeed = 10.0f;
	
	Vector3 forwardDirection = Vector3.zero;
	float gravity = 15;
	
	bool isGrounded = false;
	CharacterController characterController;

	// Use this for initialization
	void Start ()
	{
		characterController = this.GetComponent<CharacterController> ();
	}
	
	// Update is called once per frame
	void Update ()
	{

		if (isGrounded) {
							if (Input.GetAxis ("Vertical") > 0.1f)
							{
								movementSpeed = MaxSpeed;
							} 
							else if (Input.GetAxis ("Vertical") < -0.1f)
							{
								movementSpeed = MaxSpeed / 2;
							} 

						if (Input.GetKeyDown ("t")) {
								m_InteractionKeyPressed = true;
						}

						if (Input.GetKeyUp ("t")) {
								m_InteractionKeyPressed = false;
						}

			if (Input.GetKeyDown ("space")) {
				if (isGrounded == true) {
					//forwardDirection.y + jumpSpeed;
					forwardDirection.y += jumpSpeed;
					this.transform.Translate (forwardDirection);
				}
			}
				}
	}

	void FixedUpdate()
	{
			if (isGrounded)
			{
				float y = 2 * Input.GetAxis ("Horizontal");
				transform.eulerAngles = new Vector3 (transform.eulerAngles.x, transform.eulerAngles.y + y, transform.eulerAngles.z);
				
				forwardDirection = new Vector3 (0, 0, Input.GetAxis ("Vertical"));
				forwardDirection = transform.TransformDirection (forwardDirection);
			}
			forwardDirection.y -= gravity * Time.deltaTime;
			isGrounded = (characterController.Move (forwardDirection * (Time.deltaTime * movementSpeed)) & CollisionFlags.Below) != 0;
	}

	void OnTriggerStay(Collider obj)
	{
		if(obj.name == "CrawlSpace" && m_InteractionKeyPressed == true)
		{
			CrawlSpaces crawlSpace = obj.GetComponent<CrawlSpaces>();
			if(crawlSpace != null)
			{
				crawlSpace.OnUse();
				m_InteractionKeyPressed = false;
			}
		}

		if(obj.name == "DivingBoard" && m_InteractionKeyPressed == true)
		{
			DivingBoard divingBoard = obj.GetComponent<DivingBoard>();
			if(divingBoard != null)
			{
				divingBoard.notifySeeSaw(this.gameObject);
				m_InteractionKeyPressed = false;
			}
		}
	}
}
