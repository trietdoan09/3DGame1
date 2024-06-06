using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CubeMovement : MonoBehaviour
{
	enum MoveToCarDoor
    {
		none,
		leftDoor,
		rightDoor
    }
	[Tooltip("Is this character currently controllable by the player"), SerializeField]
	public bool isHandlingInput = true;

	[Tooltip("The speed the player will move"), SerializeField]
	private float speed = 1f;
	private Vector3 movement;
	[Tooltip("The amount of upwards force to apply to the character when they jump"), SerializeField]
	private float jumpVelocity = 20;
	[Tooltip("The maximum velocity the character should be able to reach"), SerializeField]
	private Vector3 maxVelocity;

	private bool hasJumped = false;
	private bool isGrounded;

	private Rigidbody rb;
	[Tooltip("The character will consider anything in this LayerMask to be 'Ground'"), SerializeField]
	private LayerMask groundLayerMask;

	[Header("Animator")]
	[SerializeField] private Animator animator;

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
	[SerializeField] private MoveToCarDoor carDoor;
	[SerializeField] private bool moveToCar;
	[SerializeField] private bool isFisrtTimeOpenDoor;

	void Awake()
	{
		rb = GetComponent<Rigidbody>();
	}
    private void Start()
    {
		maxVerticalInput = 0.5f;
		maxHorizontalInput = 0.5f;
		gameManager = GameObject.FindObjectOfType<GameManager>();
		carDoor = MoveToCarDoor.none;
		isFisrtTimeOpenDoor = true;
	}
    void FixedUpdate()
	{
		ApplyJumpPhysics();
		//CapVelocity();
		horizontalInput = Input.GetAxis("Horizontal");
		verticalInput = Input.GetAxis("Vertical");
		Movement();
		//if (!moveToCar)
		//{
		//	Movement();
		//}
  //      else
  //      {
		//	if (carDoor == MoveToCarDoor.leftDoor)
		//	{
		//		var doorPosition = gameManager.carDriving.GetComponent<CarController>().leftDoor.transform.position;
		//		transform.position = Vector3.MoveTowards(transform.position, doorPosition, speed / 5 * Time.deltaTime);
		//		carDoor = MoveToCarDoor.none;
		//	}
		//	else
		//	{
		//		var doorPosition = gameManager.carDriving.GetComponent<CarController>().rightDoor.transform.position;
		//		transform.position = Vector3.MoveTowards(transform.position, doorPosition, speed / 5 * Time.deltaTime);
		//		carDoor = MoveToCarDoor.none;
		//	}
  //          if (isFisrtTimeOpenDoor)
		//	{
		//		StartCoroutine(moveToCarDoor());
		//		isFisrtTimeOpenDoor = false;
		//	}
		//}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Ground"))
		{
			isGrounded = true;
		}
        if (collision.gameObject.CompareTag("Vehicle"))
        {
			Debug.Log("Touch Vehicle");
			isTouchVehicle = true;
			carDoor = transform.position.x - collision.transform.position.x > 0 ? MoveToCarDoor.rightDoor : MoveToCarDoor.leftDoor;
		}
	}
    private void OnCollisionStay(Collision collision)
    {
		if (collision.gameObject.CompareTag("Vehicle"))
		{
			Debug.Log("Touch Vehicle");
			isTouchVehicle = true;
			carDoor = transform.position.x - collision.transform.position.x > 0 ? MoveToCarDoor.rightDoor : MoveToCarDoor.leftDoor;
		}
	}

    private void OnCollisionExit(Collision collision)
	{
		if (collision.gameObject.CompareTag("Ground"))
		{
			isGrounded = false;
		}
		if (collision.gameObject.CompareTag("Vehicle"))
		{
			Debug.Log("Touch Vehicle");
			isTouchVehicle = false;
			carDoor = MoveToCarDoor.none;
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
				moveToCar = true;
			}
        }
	}
	IEnumerator moveToCarDoor()
    {
		yield return new WaitForSeconds(1f);
		moveToCar = false;
		gameManager.playerDrive = !gameManager.playerDrive;
		StartCoroutine(AnimOpenCar());
	}
	IEnumerator AnimOpenCar()
	{
		animator.SetBool("isEnterVehicle", gameManager.playerDrive);
		yield return new WaitForSeconds(2f);
		gameObject.SetActive(!gameManager.playerDrive);
	}
	IEnumerator JumpAfterDelay(float delay)
	{
		yield return new WaitForSeconds(delay);
		Jump();
	}
    private void Movement()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            maxVerticalInput = 1f;
            maxHorizontalInput = 1f;
        }
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
		if(h == 1 || v == 1)
		{
			movement = (movement.normalized * speed / 2) * Time.deltaTime;
		}
        else
        {
			movement = (movement.normalized * speed / 5) * Time.deltaTime;
		}
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
}
