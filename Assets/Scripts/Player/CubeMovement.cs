using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CubeMovement : MonoBehaviour
{
	[Tooltip("Is this character currently controllable by the player"), SerializeField]
	public bool isHandlingInput = true;

	[Tooltip("The speed the player will move"), SerializeField]
	private Vector3 movement;
	[Tooltip("The amount of upwards force to apply to the character when they jump"), SerializeField]
	private float jumpVelocity = 20;
	[Tooltip("The maximum velocity the character should be able to reach"), SerializeField]
	private Vector3 maxVelocity;

	private bool hasJumped = false;
	private bool isGrounded;

	[Tooltip("The character will consider anything in this LayerMask to be 'Ground'"), SerializeField]
	private LayerMask groundLayerMask;

	[Header("Animator")]
	[SerializeField] private Animator animator;
	private Rigidbody rb;
	private BoxCollider boxCollider;

	public static int score;
	public float showH;
	public float showV;
	private float maxVerticalInput;
	private float maxHorizontalInput;
	private float verticalInput;
	private float horizontalInput;

	private GameManager gameManager;
	[Header("Action")]
	[SerializeField] private bool isTouchVehicle;
	[SerializeField] private bool openTheDoor;
	[SerializeField] private bool exitCar;
	public bool isDriving;
	private float speed = 1f;
	private bool isRunning;

	private PlayerManager playerManager;

	void Awake()
	{
		rb = GetComponent<Rigidbody>();
		boxCollider = GetComponent<BoxCollider>();
		playerManager = GetComponent<PlayerManager>();
	}
    private void Start()
    {
		maxVerticalInput = 0.5f;
		maxHorizontalInput = 0.5f;
		gameManager = GameObject.FindObjectOfType<GameManager>();
		isDriving = false;
		openTheDoor = false;
		exitCar = false;
		isRunning = false;
	}
    void FixedUpdate()
	{
		ApplyJumpPhysics();
        //CapVelocity();
        horizontalInput = Input.GetAxis("Horizontal");
		verticalInput = Input.GetAxis("Vertical");
        if (!openTheDoor && !playerManager.runOutStamina)
		{
			Movement();
		}
	}


	void Update()
	{
		ReceiveKeyInput();
		if (movement != Vector3.zero)
		{
			rb.transform.rotation = Quaternion.LookRotation(movement);
		}
		if (isGrounded == true)
		{
			animator.SetBool("isJumping", false);
		}
		SetPlayerPosition();
    }
	private void SetPlayerPosition()
	{
		if (isDriving)
		{
			transform.parent = gameManager.carDrive.transform;
			transform.position = gameManager.carDrive.transform.position;
			transform.rotation = gameManager.carDrive.transform.rotation;
		}
        if (exitCar)
		{
			exitCar = false;
			StartCoroutine(AnimExitCar());
        }
	}
	private void ReceiveKeyInput()
	{
		// Lock mouse
		if (Input.GetKeyDown(KeyCode.Q))
		{
			Cursor.visible = !Cursor.visible;
			Cursor.lockState = Cursor.visible ? CursorLockMode.None : CursorLockMode.Locked;
		}
		if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
		{
			animator.SetBool("isJumping", true);
			StartCoroutine(JumpAfterDelay(0.2f));
		}
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (isTouchVehicle)
			{
				openTheDoor = true;
				gameManager.playerDrive = !gameManager.playerDrive;
				StartCoroutine(AnimOpenCar());
			}
            if (!exitCar && !isTouchVehicle && openTheDoor)
            {
				exitCar = true;
			}
        }
	}
	IEnumerator AnimOpenCar()
	{
		animator.SetBool("isEnterVehicle", gameManager.playerDrive);
		rb.useGravity = false;
		boxCollider.isTrigger = true;
		yield return new WaitForSeconds(2f);
		isDriving = true;
		animator.SetBool("isDriving",true); 
	}
	IEnumerator AnimExitCar()
	{
		animator.SetBool("isDriving", false);
		gameManager.PlayerExitCar();
		yield return new WaitForSeconds(5f);
		transform.parent = null;
		rb.useGravity = true;
		boxCollider.isTrigger = false;
		isDriving = false;
		openTheDoor = false;
        transform.position = gameManager.carDrive.transform.position + new Vector3(-1.2f, 0, 0);
		animator.SetBool("isEnterVehicle", gameManager.playerDrive);
	}
	IEnumerator JumpAfterDelay(float delay)
	{
		yield return new WaitForSeconds(delay);
		Jump();
	}
	private void CheckPlayerRun()
    {
		if (Input.GetKey(KeyCode.LeftShift) && playerManager.currentStaminaPoint > 1 && !playerManager.runOutStamina)
		{
			if (verticalInput != 0 || horizontalInput != 0)
			{
				maxVerticalInput = 1f;
				maxHorizontalInput = 1f;
				playerManager.currentStaminaPoint -= Time.deltaTime * 10;
				isRunning = true;
				if (playerManager.currentStaminaPoint < 1)
				{
					playerManager.runOutStamina = true;
					StartCoroutine(playerManager.RunOutStamina());
				}
			}
			else
			{
				playerManager.currentStaminaPoint = playerManager.currentStaminaPoint > playerManager.maxStaminaPoint ? playerManager.maxStaminaPoint : playerManager.currentStaminaPoint + (Time.deltaTime * 5);
				isRunning = false;
			}
		}
		else
		{
			playerManager.currentStaminaPoint = playerManager.currentStaminaPoint > playerManager.maxStaminaPoint ? playerManager.maxStaminaPoint : playerManager.currentStaminaPoint + (Time.deltaTime * 5);
			isRunning = false;
		}
	}
    private void Movement()
	{
		CheckPlayerRun();

        if (verticalInput > maxVerticalInput || horizontalInput > maxHorizontalInput)
        {
            verticalInput = verticalInput >= maxVerticalInput ? maxVerticalInput : verticalInput;
            horizontalInput = horizontalInput >= maxHorizontalInput ? maxHorizontalInput : horizontalInput;
        }
        else if (verticalInput < -maxVerticalInput || horizontalInput < -maxHorizontalInput)
        {
            verticalInput = verticalInput < -maxVerticalInput ? -maxVerticalInput : verticalInput;
            horizontalInput = horizontalInput < -maxHorizontalInput ? -maxHorizontalInput : horizontalInput;
		}
		ManageMovement(horizontalInput, verticalInput);
		maxVerticalInput = 0.5f;
        maxHorizontalInput = 0.5f;
    }
    public void ManageMovement(float h, float v)
	{
		if (!isHandlingInput)
		{
			return;
		}

		Vector3 forwardMove = Vector3.Cross(Camera.main.transform.right, Vector3.up);
		Vector3 horizontalMove = Camera.main.transform.right;

		movement = forwardMove * v + horizontalMove * h;
		speed = isRunning ? 3 : 1;
		movement = (movement.normalized * speed) * Time.deltaTime;
		GetComponent<Rigidbody>().MovePosition(transform.position + movement);

		animator.SetFloat("moveX", h);
		animator.SetFloat("moveZ", v);
		if(h!=0 || v != 0)
		{
			animator.SetBool("isMoving", true);
		}
        else
		{
			animator.SetBool("isMoving", false);
		}
	}
	public void Jump()
	{
		hasJumped = true;
	}
	private void ApplyJumpPhysics()
	{
		if (hasJumped)
		{
			rb.velocity = new Vector3(rb.velocity.x, jumpVelocity, rb.velocity.z);
			hasJumped = false;
		}
	}
	void CapVelocity()
	{
		Vector3 _velocity = GetComponent<Rigidbody>().velocity;
		_velocity.x = Mathf.Clamp(_velocity.x, -maxVelocity.x, maxVelocity.x);
		_velocity.y = Mathf.Clamp(_velocity.y, -maxVelocity.y, maxVelocity.y);
		_velocity.z = Mathf.Clamp(_velocity.z, -maxVelocity.z, maxVelocity.z);
		rb.velocity = _velocity;
	}
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Vehicle"))
		{
			isTouchVehicle = true;
			gameManager.carDrive = other.transform.parent.gameObject;
		}
    }
    private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag("Vehicle"))
		{
			isTouchVehicle = false;
		}
	}
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Ground"))
		{
			isGrounded = true;
		}
	}
	private void OnCollisionStay(Collision collision)
	{
	}
	private void OnCollisionExit(Collision collision)
	{
		if (collision.gameObject.CompareTag("Ground"))
		{
			isGrounded = false;
		}
	}
}
