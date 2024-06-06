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
    [SerializeField] private GameObject player;
    // Start is called before the first frame update
    [SerializeField] private bool carCameraActive;

    public GameObject carDriving;

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

    }
    private void ChangeCameraFollow()
    {
        playerCamera.enabled = playerDrive ? false : true;
        carCamera.enabled = playerDrive ? true : false;
        carCameraActive = playerDrive;
        CheckPlayerExitCar();
    }
    private void SetPlayerPosition()
    {
        playerController.transform.position = carDriving.transform.position + new Vector3(-1,0,0);
    }
    private void CheckPlayerExitCar()
    {
        if (carCameraActive)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                player.SetActive(true);
                playerDrive = false;
                SetPlayerPosition();
            }
        }
    }
}
