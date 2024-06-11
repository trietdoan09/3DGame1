using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool playerDrive;
    private CubeMovement playerController;
    private CarController carController;
    private CameraMovement playerCamera;
    private CarCameraMovement carCamera;
    public GameObject carDrive;
    // Start is called before the first frame update
    [SerializeField] private bool carCameraActive;


    void Start()
    {
        carController = GameObject.FindObjectOfType<CarController>();
        playerCamera = GameObject.FindObjectOfType<CameraMovement>();
        carCamera = GameObject.FindObjectOfType<CarCameraMovement>();
        playerController = GameObject.FindObjectOfType<CubeMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        ChangeCameraFollow();
        CheckPlayerExitCar();
    }
    private void ChangeCameraFollow()
    {
        playerCamera.enabled = playerDrive ? false : true;
        carCamera.enabled = playerDrive ? true : false;
        carCameraActive = playerDrive;
    }
    private void SetPlayerPosition()
    {
        playerController.transform.position = carDrive.transform.position + new Vector3(-1,0,0);
    }
    private void CheckPlayerExitCar()
    {
        if (carCameraActive)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                playerDrive = false;
               // SetPlayerPosition();
            }
        }
    }
}
