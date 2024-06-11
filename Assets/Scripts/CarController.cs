using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public enum Axel
    {
        Front,
        Back
    }
    [Serializable]
    public struct WheelController
    {
        public GameObject wheel;
        public WheelCollider wheelCollider;
        public Axel axel;
    }
    [SerializeField] private float maxAcceleration;
    [SerializeField] private float brakeAcceleration;
    [SerializeField] private float turnSensitive;
    [SerializeField] private float maxSteerAngle;
    public List<WheelController> wheelControllers;
    float moveInput;
    float steerInput;
    [SerializeField] private Rigidbody rigidbody;
    private Vector3 _centerOfMess;

    private Vector3 movement;
    [SerializeField] private float moveSpeed;
    private GameManager gameManager;
    private CarCameraMovement cameraMovement;
    public bool carDrive;
    public bool touchPlayer;
    public bool isDriving;
    public GameObject leftDoor;
    public GameObject rightDoor;


    // Start is called before the first frame update
    void Start()
    {
        maxAcceleration = 30f;
        brakeAcceleration = 50f;
        turnSensitive = 1f;
        maxSteerAngle = 30f;
        gameManager = GameObject.FindObjectOfType<GameManager>();
        cameraMovement = GameObject.FindObjectOfType<CarCameraMovement>();
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.centerOfMass = _centerOfMess;
        carDrive = false;
    }

    // Update is called once per frame
    void Update()
    {
        CheckEnterCar();
        GetInput();
        AnimateWheel();
    }
    private void FixedUpdate()
    {
        if (isDriving && carDrive && touchPlayer)
        {
            CarMovement();
        }
        else if(!isDriving && carDrive)
        {
            touchPlayer = false;
            carDrive = false;
        }
    }
    public void GetInput()
    {
        moveInput = Input.GetAxis("Vertical");
        steerInput = Input.GetAxis("Horizontal");
    }
    public void CarMovement()
    {
        if (isDriving)
        {

            Movement();
            Steer();
            Brake();
        }
    }
    public void Movement()
    {
        foreach (var wheel in wheelControllers)
        {
            wheel.wheelCollider.motorTorque = moveInput * maxAcceleration * Time.deltaTime * 600;
        }
    }
    public void Steer()
    {
        foreach(var wheel in wheelControllers)
        {
            if(wheel.axel == Axel.Front)
            {
                var _steerAngle = steerInput * turnSensitive * maxSteerAngle;
                wheel.wheelCollider.steerAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, _steerAngle, 0.6f);
            }
        }
    }
    public void AnimateWheel()
    {
        foreach(var wheel in wheelControllers)
        {
            Quaternion rot;
            Vector3 pos;
            wheel.wheelCollider.GetWorldPose(out pos, out rot);
            wheel.wheel.transform.position = pos;
            wheel.wheel.transform.rotation = rot;
        }
    }
    public void Brake()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            foreach(var wheel in wheelControllers)
            {
                wheel.wheelCollider.brakeTorque = 300 * brakeAcceleration * Time.deltaTime;
            }
        }
        else
        {
            foreach (var wheel in wheelControllers)
            {
                wheel.wheelCollider.brakeTorque = 0;
            }
        }
    }
    private void CheckEnterCar()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
        if (touchPlayer && gameManager.playerDrive)
        {
            isDriving = true;
            StartCoroutine(WaitingEnterCar());
            ChangeCameraTarget();
        }
    }
    IEnumerator WaitingEnterCar()
    {
        yield return new WaitForSeconds(0.9f);
    }
    private void ChangeCameraTarget()
    {
        if (!carDrive)
        {
            cameraMovement.carTarget = transform;
            carDrive = true;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            touchPlayer = true;
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            touchPlayer = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            touchPlayer = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            touchPlayer = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            touchPlayer = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            touchPlayer = false;
        }
    }
}
